﻿$ready(function () {
    window.vue = new Vue({
        el: '#app',
        data: {
            form: {
                lvid: '',
                search: '',
                size: 20,
                index: 1
            },
            organs: [],
            current: {},         //当前机构

            levels: [],
            domain: '',  //主域

            loading: false,
            loadingid: 0,        //当前操作中的对象id

            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [] //数据表中选中的行
        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.bat(
                $api.get('Organization/LevelAll'),
                $api.get('Organization/Current'),
                $api.get('Platform/Domain')
            ).then(axios.spread(function (level, current, domain) {
                th.loading = false;
                //获取结果
                th.levels = level.data.result;
                th.current = current.data.result;
                th.handleCurrentChange(1);
                th.domain = domain.data.result;
            })).catch(function (err) {
                th.loading = false;
                console.error(err);
                Vue.prototype.$alert(err);
            });


        },
        methods: {
            //删除
            deleteData: function (datas) {
                $api.delete('Organization/Delete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vue.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        window.vue.handleCurrentChange();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 41);
                $api.get("Organization/Pager", th.form).then(function (d) {
                    if (d.data.success) {
                        th.organs = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);

                });
            },
            //二级域名的链接，
            domainLink: function (domain) {
                return "http://" + domain + "." + this.domain;
            },
            //双击事件
            rowdblclick: function (row, column, event) {
                var rowkey = this.$refs.datatables.rowKey;
                this.$refs.btngroup.modify(row[rowkey]);
            },
            //更改使用状态
            changeUse: function (row) {
                var th = this;
                this.loadingid = row.Org_ID;
                $api.post('Organization/Modify', { 'entity': row }).then(function (req) {
                    if (req.data.success) {
                        vue.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                    th.loadingid = 0;
                }).catch(function (err) {
                    vue.$alert(err, '错误');
                });
            },
            //更改默认状态
            changeDefault: function (row) {
                var th = this;
                this.loadingid = row.Org_ID;
                $api.post('Organization/SetDefault', { 'id': row.Org_ID }).then(function (req) {
                    if (req.data.success) {
                        vue.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        throw req.data.message;
                    }
                    th.loadingid = 0;
                }).catch(function (err) {
                    vue.$alert(err, '错误');
                });
            },
            //下拉菜单的事件
            handleCommand: function (cmd) {
                if (cmd == null) return;
                switch (cmd) {
                    case 'setAdmin':
                        console.log(cmd);
                        break;
                    case 'config':
                        console.log(cmd);
                        break;
                }


            },
            //查看机构参数
            viewConfig: function (row) {
                var th = this;
                var url = '/manage/organs/config?id=' + row.Org_ID;
                var title = "“" + row.Org_Name + "”的参数";
                var boxid = window.name + '_' + row.Org_ID + '[config]';
                this.$refs.btngroup.pagebox(url, title, boxid, 800, 600, { full: false });
                console.log(row);
            },
            //设置管理员
            setAdministrator: function (row) {
                var th = this;
                var url = '/manage/organs/Administrator?id=' + row.Org_ID;
                var title = "“" + row.Org_Name + "”的管理员";
                var boxid = window.name + '_' + row.Org_ID + '[administrator]';
                this.$refs.btngroup.pagebox(url, title, boxid, 800, 600, { full: false });
                console.log(row);
            }
        }
    });

});