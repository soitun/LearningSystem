$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            stid: $api.querystring('stid'),
            couid: $api.dot(),
            account: {},     //当前登录账号       

            outlines: [],
            logdatas: [],

            loading_init: true,
            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Account/ForID', { 'id': th.stid }),
                $api.cache('Outline/TreeList', { 'couid': th.couid })
            ).then(axios.spread(function (account, outlines) {
                th.loading_init = false;
                //获取结果
                th.account = account.data.result;
                th.outlines = outlines.data.result;
                console.log(th.outlines);
                if (th.islogin) th.getlogs();
            })).catch(function (err) {
                th.loading_init = false;
                alert(err);
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
        },
        methods: {
            //加载日志数据
            getlogs: function () {
                var th = this;
                th.loading = true;
                var acid = th.account.Ac_ID;
                $api.cache('Course/LogForOutlineVideo:10', { 'stid': acid, 'couid': th.couid }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.logdatas = req.data.result;
                        console.log(th.logdatas);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    alert(err);
                    console.error(err);
                });
            }
        }
    });

}, ['Components/outline_progress.js']);
