$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                orgid: '',
                use: '',
                search: '',
                size: 20,
                index: 1
            },
            organ: {},       //当前机构
            config: {},

            loading: false,
            loadingid: 0,        //当前操作中的对象id
            datas: [], //数据源           

            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [] //数据表中选中的行
        },
        watch: {
            'loading': function (val, old) {
                //console.log(val);
            }
        },
        computed: {
        },
        created: function () {
            var th = this;
            $api.bat(
                $api.get('Organization/Current')
            ).then(axios.spread(function (organ) {
                vapp.loading_init = false;
                //获取结果             
                th.organ = organ.data.result;
                th.form.orgid = th.organ.Org_ID;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.handleCurrentChange(1);
            })).catch(function (err) {
                console.error(err);
            });
        },
        methods: {            
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算        
                var maxheight = document.documentElement.clientHeight;
                maxheight = maxheight < 1 ? window.screen.availHeight - 200 : maxheight;
                var area = maxheight - 100;
                th.form.size = Math.floor(area / 42);
                th.form.size = th.form.size <= 10 ? 10 : th.form.size;
                th.form.size = th.form.size >= 100 ? 100 : th.form.size;
                th.loading = true;
                $api.get("Account/SortPager", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        throw d.data.message;
                    }
                    th.rowdrop();
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    console.error(err);
                });
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loading = true;
                $api.delete('Account/SortDelete', { 'id': datas }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    console.error(err);
                });
            },
            //更改使用状态
            changeState: function (row) {
                var th = this;
                this.loadingid = row.Sts_ID;
                $api.post('Account/SortUpdateUse', { 'stsid': row.Sts_ID, 'use': row.Sts_IsUse }).then(function (req) {
                    if (req.data.success) {
                        vapp.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                    th.loadingid = 0;
                }).catch(function (err) {
                    vapp.$alert(err, '错误');
                    th.loadingid = 0;
                });
            },
            //行的拖动
            rowdrop: function () {
                // 首先获取需要拖拽的dom节点            
                const el1 = document.querySelectorAll('table > tbody')[0];
                if (el1 == null) return;
                Sortable.create(el1, {
                    disabled: false, // 是否开启拖拽
                    ghostClass: 'sortable-ghost', //拖拽样式
                    handle: '.draghandle',     //拖拽的操作元素
                    animation: 150, // 拖拽延时，效果更好看
                    group: { // 是否开启跨表拖拽
                        pull: false,
                        put: false
                    },
                    onStart: function (evt) {
                    },
                    onMove: function (evt, originalEvent) {
                        if ($dom('table tr.expanded').length > 0) {
                            return false;
                        };
                        
                        evt.dragged; // dragged HTMLElement
                        evt.draggedRect; // TextRectangle {left, top, right и bottom}
                        evt.related; // HTMLElement on which have guided
                        evt.relatedRect; // TextRectangle
                        originalEvent.clientY; // mouse position
                        
                    },
                    onEnd: (e) => {
                        let arr = this.datas; // 获取表数据
                        arr.splice(e.newIndex, 0, arr.splice(e.oldIndex, 1)[0]); // 数据处理
                        this.$nextTick(function () {
                            this.datas = arr;
                            for (var i = 0; i < this.datas.length; i++) {
                                this.datas[i].Sts_Tax = (this.form.size * (this.form.index - 1)) + i + 1;
                            }
                            this.changeTax();
                        });
                    }
                });
            },
            //更新排序
            changeTax: function () {
                var arr = $api.clone(this.datas);
                for (var i = 0; i < arr.length; i++) {
                    delete arr[i]['childs'];
                }
                $api.post('Account/SortUpdateTaxis', { 'items': arr }).then(function (req) {
                    if (req.data.success) {
                        vapp.$notify({
                            type: 'success',
                            message: '修改顺序成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //设置默认组
            setDefault: function (id) {
                var th = this;
                this.loadingid = id;
                $api.get('Account/SortSetDefault', { 'orgid': th.organ.Org_ID, 'id': id }).then(function (req) {
                    th.loadingid = 0;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '设置默认组成功!',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loadingid = 0;
                    console.error(err);
                });
            },
            //设置成员的按钮事件
            setaccount: function (item) {
                var title = '设置“' + item.Sts_Name + '”的成员';
                var url = $api.url.set('SortAccount', { id: item.Sts_ID });
                this.$refs.btngroup.pagebox(url, title, null, 800, 600);
            },
            //设置课程的按钮事件
            setcourse: function (item) {
                var title = '设置“' + item.Sts_Name + '”的关联课程';
                var url = $api.url.set('SortCourse', { id: item.Sts_ID });
                this.$refs.btngroup.pagebox(url, title, null, 800, '80%');
            }
        }
    });    
    //分组下的课程数
    Vue.component('course_count', {
        props: ["sort"],
        data: function () {
            return {
                count: 0,
                loading: true
            }
        },
        watch: {
            'sort': {
                handler: function (nv, ov) {
                    this.getcount(nv, nv.Sts_ID);
                }, immediate: true
            }
        },
        computed: {},
        mounted: function () { },
        methods: {
            getcount: function (item, sortid) {
                var th = this;
                th.loading = true;
                $api.get('Student/SortCoursOfNumber', { 'sortid': sortid }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.count = req.data.result;
                        item.count = th.count;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            }
        },
        template: `<span title="课程数" class="course_count" v-if="count>0">
                    <span class="el-icon-loading" v-if="loading"></span>
                    <el-tooltip v-else effect="light" content="当前组的课程数" placement="right">
                        <span>( {{count}} )</span>
                    </el-tooltip>      
                 </span> `
    });
});