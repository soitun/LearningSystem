$ready(['Components/papertype.js'],
    function () {
        window.vapp = new Vue({
            el: '#vapp',
            data: {
                org: window.org,    //当前机构
                types: [],        //试题类型，来自web.config中配置项
                admin: {},          //当前登录用户
                //试题的查询条件
                form: { "orgid": "", "accid": "", "search": "", "isdeleted": false, "diff": "", "use": "", "size": 10, "index": 1 },
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
                this.form.orgid = window.org.Org_ID;
                this.handleCurrentChange(1);
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
                    if (index != null) this.form.index = index;
                    var th = this;
                    //每页多少条，通过界面高度自动计算
                    var area = document.documentElement.clientHeight - 100;
                    th.form.size = Math.floor(area / 64);
                    th.loading = true;
                    var loading = this.$fulloading();
                    $api.get("ExamTestPaper/Pager", th.form).then(function (d) {
                        if (d.data.success) {
                            var result = d.data.result;
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
                        th.loading = false;
                        th.$nextTick(function () {
                            loading.close();
                        });
                    });
                },
                //双击事件
                rowdblclick: function (row, column, event) {
                    //console.log(row);
                    this.$refs.btngroup.modify(row[this.$refs.btngroup.idkey], null, {
                        'ico': 'e810'
                    });
                },
                //更改使用状态
                changeState: function (row) {
                    var th = this;
                    if (th.loadingid > 0) return;
                    th.loadingid = row.Tp_Id;
                    $api.post('TestPaper/ModifyState', { 'id': row.Tp_Id, 'use': row.Tp_IsUse, 'rec': row.Tp_IsRec }).then(function (req) {
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
                //批量修改状态
                batchState: function (use) {
                    use = Boolean(use);
                    var th = this;
                    this.$confirm('批量更改当前页面的试卷为“' + (use ? '启用' : '禁用') + '”, 是否继续?', '提示', {
                        confirmButtonText: '确定',
                        cancelButtonText: '取消',
                        type: 'warning'
                    }).then(() => {
                        var ids = '';
                        for (var i = 0; i < th.datas.length; i++) {
                            ids += th.datas[i].Tp_Id;
                            if (i < th.datas.length - 1) ids += ',';
                        }
                        var loading = this.$fulloading();
                        $api.post('TestPaper/ModifyState', { 'id': ids, 'use': use, 'rec': null }).then(function (req) {
                            if (req.data.success) {
                                th.$notify({
                                    type: 'success',
                                    message: '修改状态成功!',
                                    center: true
                                });
                                th.handleCurrentChange();
                                th.$nextTick(function () {
                                    loading.close();
                                });
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                        }).finally(() => { });
                    }).catch(() => {

                    });
                },
                btnadd: function (btn, ctr) {
                    let couid = $api.querystring('id');
                    var url = $api.url.set(ctr.path, 'couid', couid);
                    //console.error(url);
                    ctr.add(url, { 'ico': 'e810' });
                },
                //查看成绩
                viewResults: function (row) {
                    var url = $api.url.set('../TestPaper/Results', 'tpid', row.Tp_Id);
                    this.$refs.btngroup.pagebox(url, '《' + row.Tp_Name + '》的成绩', null, 1000, 800, { 'ico': 'e696' });
                },
                //刷新数据行
                fresh_row: function (tpid) {
                    if (tpid == null || tpid == '' || tpid == 0) return this.handleCurrentChange();
                    if (this.datas.length < 1) return;
                    //要刷新的行数据
                    let entity = this.datas.find(item => item.Etp_Id == tpid);
                    if (entity == null) return;
                    var th = this;
                    th.loadingid = tpid;
                    $api.get('ExamTestPaper/ForID', { 'id': tpid }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            let index = th.datas.findIndex(item => item.Etp_Id == result.Etp_Id);
                            //console.error(index);
                            if (index >= 0) {
                                th.$set(th.datas, index, result);
                                th.$message({
                                    message: '刷新试卷 “' + result.Etp_Name + '” 成功',
                                    type: 'success'
                                });
                            }
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => th.loadingid = 0);
                },
            },
            filters: {

            },
            components: {

            }
        });
    });