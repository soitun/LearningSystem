$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            datas: [],      //所有数据
            form: {
                orgid: '', search: '', isuse: true
            },
            defaultProps: {
                children: 'children',
                label: 'Qp_Name'
            },
            expanded: [],        //树形默认展开的节点
            expanded_storage: 'quespart_for_admin_tree',  //用于记录展开节点的storage名称
            filterText: '',      //查询过虑树形的字符
            total: 0,       //当前机构下的试题分类总数
            //是否折叠
            fold: false,

            loadstate: {
                init: false,        //初始化
                def: false,         //默认
                get: false,         //加载数据
                update: false,      //更新数据
                del: false          //删除数据
            }
        },
        mounted: function () {
            this.$refs.btngroup.addbtn({
                text: '展开/折叠', tips: '展开树形或折叠树形',
                id: 'fold_open', type: 'primary',
                icon: 'e6ea'
            });
            var th = this;
            th.form.orgid = window.org.Org_ID;
            th.getTreeData();
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
            //过滤树形数据
            filterText: function (val) {
                this.$refs.tree.filter(val);
                this.fold = true;
            },
            //树形是否折叠
            fold: function (nv, ov) {
                if (nv) this.unFoldAll2(this.datas);
                else this.collapseAll2(this.datas);
            }
        },
        methods: {
            //所取专业的数据，为树形数据
            getTreeData: function () {
                var th = this;
                th.loading = true;
                $api.get('ExamQues/PartTree', th.form).then(function (req) {
                    if (req.data.success) {
                        th.datas = th.clacCount(req.data.result);
                        //获取默认展开的节点
                        var arr = $api.storage(th.expanded_storage);
                        if ($api.getType(arr) == 'Array') {
                            th.expanded = arr;
                        }
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //计算课程数，ques数，test数
            clacCount: function (datas) {
                this.total = 0;
                this.calcSerial(datas);
                datas.forEach(d => this.ergodic_clacCount(d, 'QP_Count', 'QuesCount'));
                return datas;
            },
            //遍历计算各个专业的课程数，包括当前专业的子专业
            //field:要计算的字段
            //result:计算结果的字段名，主要为了保留field原始值，方便恢复
            ergodic_clacCount: function (sbj, field, result) {
                let count = sbj[field];
                if (sbj.children && sbj.children.length > 0) {
                    let datas = sbj.children;
                    for (let i = 0; i < datas.length; i++)
                        count += this.ergodic_clacCount(datas[i], field, result);
                }
                sbj[result] = count;
                return count;
            },
            //计算序号
            calcSerial: function (item, lvl) {
                var childarr = Array.isArray(item) ? item : (item.children ? item.children : null);
                if (childarr == null) return;
                for (let i = 0; i < childarr.length; i++) {
                    childarr[i].serial = (lvl ? lvl : '') + (i + 1) + '.';
                    //childarr[i]['CourseCount'] = 0;
                    childarr[i]['QuesCount'] = 0;
                    //childarr[i]['TestCount'] = 0;
                    childarr[i]['calcChild'] = this.calcChild(childarr[i]);
                    this.total++;
                    this.calcSerial(childarr[i], childarr[i].serial);
                }
            },
            calcChild: function (sbj) {
                if (!sbj.children) return 0;
                var count = sbj.children.length;
                for (var i = 0; i < sbj.children.length; i++) {
                    count += this.calcChild(sbj.children[i]);
                }
                return count;
            },
            //过滤树形
            filterNode: function (value, data) {
                if (!value) return true;
                var txt = $api.trim(value.toLowerCase());
                if (txt == '') return true;
                return data.Qp_Name.toLowerCase().indexOf(txt) !== -1;
            },
            // 全部展开
            unFoldAll2: function (data) {
                let self = this;
                data.forEach((el) => {
                    self.$refs.tree.store.nodesMap[el.Qp_ID].expanded = true;
                    el.children && el.children.length > 0 ? self.unFoldAll2(el.children) : ""; // 子级递归
                });
            },
            // 全部折叠
            collapseAll2: function (data) {
                let self = this;
                data.forEach((el) => {
                    self.$refs.tree.store.nodesMap[el.Qp_ID].expanded = false;
                    el.children && el.children.length > 0 ? self.collapseAll2(el.children) : ""; // 子级递归
                });
            },
        },
        filters: {

        },
        components: {

        }
    });
});