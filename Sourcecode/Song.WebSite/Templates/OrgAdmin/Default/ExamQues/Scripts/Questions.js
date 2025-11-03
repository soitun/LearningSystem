$ready([
    '/Utilities/Components/question/function.js',
    '../Question/Components/ques_type.js',
    'Components/ques_diff.js',
    'Components/ques_collect.js'
], function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            types: [],        //试题类型，来自web.config中配置项
            admin: {},          //当前登录用户
            //试题的查询条件
            form: { "orgid": -1, "isdeleted":false,"qpid": "", "tagid": "", "knlid": "", "type": "", "diff": "", "size": 10, "index": 1 },
            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            loadstate: {
                init: false,        //初始化
                def: false,         //默认
                get: false,         //加载数据
                update: false,      //更新数据
                del: false          //删除数据
            },
            loadingid: 0,
        },
        mounted: function () {
            this.$refs.btngroup.addbtn([{
                text: '全选/取消', tips: '选择标签',
                id: 'select', type: 'primary',
                icon: 'a057'
            }]);

            var th = this;
            th.form.orgid = window.org.Org_ID;
            th.loadstate.init = true;
            $api.cache('Question/Types:99999').then(req => {
                if (req.data.success) {
                    th.types = req.data.result;
                    th.handleCurrentChange(1);
                } else {
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loadstate.init = false);

            //当前登录的管理员
            $api.login.current('admin', function (d) {
                th.admin = d;
            });
        },
        created: function () {

        },
        computed: {
            loading: function () {
                if (!this.loadstate) return false;
                for (let key in this.loadstate) {
                    if (this.loadstate.hasOwnProperty(key)
                        && this.loadstate[key])
                        return true;
                }
                return false;
            }
        },
        watch: {

        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                var th = this;
                if (index != null) this.form.index = index;
                var loading = this.$fulloading();
                $api.get("ExamQues/QuesPager", th.form).then(function (d) {
                    if (d.data.success) {
                        var result = d.data.result;
                        for (let i = 0; i < result.length; i++) {
                            result[i] = window.ques.parseAnswer(result[i]);
                            //result[i].Qus_Title = result[i].Qus_Title.replace(/(<([^>]+)>)/ig, "");
                        }
                        th.datas = result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => {
                    th.$nextTick(function () {
                        loading.close();
                    });
                });
            },
            //批量选择
            selectall: function () {
                let isselected = true;   //是否全选
                for (let i = 0; i < this.datas.length; i++) {
                    if (this.datas[i].checked == null || !this.datas[i].checked) {
                        isselected = false;
                        break;
                    }
                }
                for (let i = 0; i < this.datas.length; i++) {
                    this.$set(this.datas[i], 'checked', !isselected);
                }
            },
            //获取选中的id
            getselectid: function () {
                return this.datas.filter(item => item.checked).map(item => item.Qus_ID);
            },
            //批量删除
            btnbatdel: function (id) {
                if (id != null && id != '') return this.deleteData(id);
                var arr = this.getselectid();
                if (arr.length < 1) {
                    this.$message({
                        message: '请选中要操作的数据项',
                        type: 'error'
                    });
                    return false;
                }
                var th = this;
                this.$confirm('是否确认删除这 ' + arr.length + ' 项数据? ', '谨慎操作', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(function () {
                    th.deleteData(arr.join(","));
                }).catch(function () { });
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                var loading = this.$fulloading();
                $api.delete('ExamQues/QuesDelete', { 'id': datas }).then(function (req) {
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
                    alert(err);
                    console.error(err);
                }).finally(() => {
                    th.$nextTick(function () {
                        loading.close();
                        //th.loading = false;
                    });
                });
            },
            //更改使用状态
            changeState: function (row) {
                var th = this;
                th.loadingid = row.Qus_ID;
                $api.post('Question/ChangeUse', { 'id': row.Qus_ID, 'use': row.Qus_IsUse }).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err, '错误');
                }).finally(() => th.loadingid = 0);
            },
            //导出
            output: function (btn) {
                var title = btn.tips;
                this.$refs.btngroup.pagebox('Export', title, null, 800, 600);
            },
            //导入
            input: function (btn) {
                var title = btn.tips;
                this.$refs.btngroup.pagebox('Import', title, null, 900, 650);
            },
        },
        filters: {

        },
        components: {

        }
    });
});