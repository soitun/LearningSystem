$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},
            organ: {},
            config: {},
            plate: {},


            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading = true;
            //平台信息
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(([acc, platinfo, org]) => {
                //获取结果   
                th.account = acc.data.result;
                th.platinfo = platinfo.data.result;
                th.organ = org.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: t => !$api.isnull(t.account)
        },
        watch: {
            'account': function (nv, ov) {

            },
            'config': function (nv, ov) {

            }
        },
        methods: {
            //已经登录
            successful: function (account) {
                this.account = account;
                var th = this;
                window.setTimeout(function () {
                    let referrer = $api.querystring('referrer');
                    if ($api.isnull(referrer)) referrer = $api.storage('singin_referrer');
                    if ($api.isnull(referrer) || referrer == 'undefined') referrer = '/mobi';
                    $api.storage('singin_referrer', null);      //去除本地记录的来源页信息
                    window.navigateTo(decodeURIComponent(referrer));
                }, 200);
            },
            //退出登录
            logout: function () {
                this.$dialog.confirm({
                    message: '是否确定退出登录？',
                }).then(function () {
                    $api.login.out('account', function () {
                        window.setTimeout(function () {
                            window.navigateTo('/mobi/');
                        }, 500);
                    });
                    this.account = {};
                }).catch(function () { });
            }
        }
    });

}, ['/Utilities/OtherLogin/config.js',      //第三方登录的配置项
    '/Utilities/Components/Sign/Login.js'
]);
