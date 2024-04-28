$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {}, //当前登录账号对象

            loading: false,
            loading_init: true
        },
        mounted: function () {
            this.getAccount();
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            //获取当前登录账号
            getAccount: function () {
                var th = this;
                th.loading_init = true;
                $api.get('Account/Current').then(function (req) {
                    if (req.data.success) {
                        th.account = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading_init = false);
            },
            btnEnter: function (formName) {
                var th = this;
                th.loading = true;
                var obj = th.remove_redundance(th.account);
                var apipath = 'Account/ModifyJson';
                $api.post(apipath, { 'acc': obj }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.getAccount();
                        th.$message({
                            type: 'success',
                            message: '操作成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //清理冗余的属性，仅保持当前form表单的属性，未在表单中的不提交到服务器
            remove_redundance: function (obj) {
                //表单中的字段
                var props = ['Ac_ID', 'Ac_Intro'];
                //obj的属性字段,如果表单上没有，则删除               
                for (let att in obj) {
                    var exist = false;
                    for (var i = 0; i < props.length; i++) {
                        if (att == props[i]) {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist) delete obj[att];
                }
                return obj;
            }
        }
    });

});
