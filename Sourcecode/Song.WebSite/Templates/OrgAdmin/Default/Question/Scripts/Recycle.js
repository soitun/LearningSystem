$ready([], function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            org: {},
            form:
            {
                'orgid': '', 'sbjids': '', 'thid': '', 'use': '', 'del': true, 'live': '', 'free': '',
                'search': '', 'order': 'def',
                'size': 1, 'index': 1
            },
            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行
            exception: '',

            loadstate: {
                recycle: false,         //还原
                get: false,         //加载数据     

                remove: false          //删除数据
            }
        },
        mounted: function () {

        },
        created: function () {
            var th = this;
            th.org = window.org;
            th.form.orgid = window.org.Org_ID;
            th.handleCurrentChange(1);
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
            },
             //当前选择的课程
             'courses': function () {
                if (this.form.sbjid == '') return this.courses_all;
                //获取选中的所有专业id
                var sbjid = this.form.sbjid;
                var sbjlist = this.$refs['subject'].subjects;
                getsbjid(sbjid, sbjlist);
                function getsbjid(sbjid, sbjs) {
                    for (let i = 0; i < sbjs.length; i++) {
                        if (sbjid == 0) {
                            sbjlist.push(sbjs[i].Sbj_ID);
                            if (sbjs[i].children && sbjs[i].children.length > 0)
                                getsbjid(0, sbjs[i].children);
                            continue;
                        }
                        if (sbjs[i].Sbj_ID == sbjid) {
                            sbjlist.push(sbjs[i].Sbj_ID);
                            if (sbjs[i].children && sbjs[i].children.length > 0)
                                getsbjid(0, sbjs[i].children);
                        } else {
                            if (sbjs[i].children && sbjs[i].children.length > 0)
                                getsbjid(sbjid, sbjs[i].children);
                        }
                    }
                }
                //console.log(sbjlist);
                //所有专业下的课程（包括子专业）
                var cou_arr = [];
                for (let j = 0; j < sbjlist.length; j++) {
                    var sbj = sbjlist[j];
                    for (let i = 0; i < this.courses_all.length; i++) {
                        const cou = this.courses_all[i];
                        if (cou.Sbj_ID == sbj) {
                            cou_arr.push(cou);
                        }
                    }
                }
                return cou_arr;
            }

        },
        watch: {

        },
        methods: {
             //获取课程
             getCourses: function (val) {
                var th = this;
                th.loading = true;
                var orgid = th.org.Org_ID;
                $api.cache('Course/Pager', {
                    'orgid': orgid, 'sbjids': 0, 'thid': '', 'use': '', 'del':false,'live': '', 'free': '',
                    'search': '', 'order': '', 'size': -1, 'index': 1
                }).then(function (req) {
                    if (req.data.success) {
                        th.courses_all = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(function () {
                    //th.handleCurrentChange(1);
                });
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                let area = $dom.height() - 100;
                th.form.size = Math.floor(area / 57);
                th.loadstate.get = true;
                th.exception = '';
                $api.get("Course/Pager", th.form).then(function (d) {
                    if (d.data.success) {
                        th.datas = d.data.result;
                        //console.log(th.datas);
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        th.exception = d.data.message;
                        throw d.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadstate.get = false);
            },
            //回收
            recycle: function (btn, datas) {
                var th = this;
                th.$confirm('是否还原选中的数据?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    th.loadstate.recycle = true;
                    $api.post("Course/Recycle", { "id": datas })
                        .then(req => {
                            if (req.data.success) {
                                let result = req.data.result;
                                th.handleCurrentChange();
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(err => console.error(err))
                        .finally(() => th.loadstate.recycle = false);
                }).catch(() => { });
            },
            //彻底删除
            remove: function (datas) {
                var th = this;
                th.$confirm('此操作将永久删除该内容, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'error'
                }).then(() => {
                    th.loadstate.remove = true;
                    $api.delete("Course/Remove", { "id": datas })
                        .then(req => {
                            if (req.data.success) {
                                let result = req.data.result;
                                th.handleCurrentChange();
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(err => console.error(err))
                        .finally(() => th.loadstate.remove = false);
                }).catch(() => { });
            },
        },
        filters: {

        },
        components: {

        }
    });
});