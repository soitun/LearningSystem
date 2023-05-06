$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            pid: $api.querystring('pi'),    //接口id
            serial: $api.querystring('serial'), //流水号

            account: {},      //当前账户
            orgin: {},           //当前机构
            interface: {},       //支付接口
            moneyAccount: {},       //账单

            notify_url: '',      //成功后的回调地址
            pay_url: '',         //支付地址

            loading: true,
            loading_qr: false //二维码加载
        },
        mounted: function () {
            var th = this;
            th.loading = true;
            $api.bat(
                $api.get('Account/Current'),
                $api.get("Organization/Current")
            ).then(axios.spread(function (acc, org) {
                //获取结果
                th.account = acc.data.result;
                th.orgin = org.data.result;
                th.getPayinterface();
            })).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            },
            //支付接口是否存在
            ifexist: function () {
                return JSON.stringify(this.interface) != '{}' && this.interface != null && this.interface.Pai_IsEnable == true
                    && this.interface.Pai_InterfaceType != '';
            },
            //支付流水号是否存在或是否可用（例如未支付完成）
            ismaccount: function () {
                return JSON.stringify(this.moneyAccount) != '{}' && this.moneyAccount != null;
            }
        },
        watch: {

        },
        methods: {
            //获取接口对象和流水号
            getPayinterface: function () {
                var th = this;
                th.loading = true;
                $api.bat(
                    $api.get('Pay/Interface', { 'id': th.pid }),
                    $api.get('Pay/MoneyAccount', { 'serial': th.serial })
                ).then(axios.spread(function (pi, acc) {
                    //获取结果
                    th.interface = pi.data.result;
                    th.moneyAccount = acc.data.result;
                    if (th.ifexist && th.ismaccount) {
                        th.build_pay_url(th.interface, th.account, th.moneyAccount, th.orgin);
                    }

                })).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //生成回调地址
            build_notify_url: function (pi) {
                let notify_url = pi.Pai_Returl;
                if (notify_url == '') notify_url = window.location.origin;
                let t = notify_url.substring(notify_url.length - 1);
                if (notify_url.length > 0 && notify_url.substring(notify_url.length - 1) != '/') notify_url += "/";
                notify_url += "Pay/Weixin/NativePayNotify";
                return notify_url;
            },
            //支付url
            //pi: 支付接口的对象
            //acount: 学员账号的对象
            //moneyacc: 资金流水记录
            build_pay_url: function (pi, account, moneyacc, orgin) {
                let total_fee = Math.floor(moneyacc.Ma_Money * 100);
                let orgid = moneyacc.Org_ID;
                let appid = pi.Pai_ParterID;  //绑定支付的APPID（必须配置）
                let secret = pi.Pai_Key;      //公众帐号secert（仅JSAPI支付的时候需要配置）
                //回调地址
                let notify_url = this.build_notify_url(pi);
                //接口配置项
                let config = $api.xmlconfig.tojson(pi.Pai_Config);
                let mchid = config["MCHID"];    //商户id
                let paykey = config["Paykey"];  //支付密钥
                //充值的人员信息
                let buyer = account.Ac_MobiTel1 == '' ? account.Ac_MobiTel2 : account.Ac_MobiTel1;
                if (buyer == '') buyer = account.Ac_AccName;
                var th = this;
                th.loading_qr = true;
                $api.get('Pay/WxNativePayUrl', {
                    'productId': total_fee, 'body': orgin.Org_PlatformName, 'serial': moneyacc.Ma_Serial,
                    'total_fee': total_fee, 'appid': appid, 'mchid': mchid, 'paykey': paykey,
                    'notify_url': notify_url, 'buyer': buyer
                }).then(function (req) {
                    if (req.data.success) {
                        th.pay_url = req.data.result;
                        th.$nextTick(function () {
                            th.qrcode();
                            //验证是否支付成功
                            window.setTimeout(function () {
                                th.call_succeeded(moneyacc.Ma_Serial);
                            }, 1000);
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading_qr = false);
            },
            //生成二维码
            qrcode: function () {
                $("#qrcode").each(function () {
                    if ($(this).find("img").length > 0) return;
                    var url = $(this).attr('pay_url');
                    jQuery($(this)).qrcode({
                        render: "canvas", //也可以替换为table
                        width: 150, height: 150,
                        foreground: "#0db711", background: "#FFF",
                        text: url
                    });
                    //将canvas转换成img标签，否则无法打印
                    var canvas = $(this).find("canvas").hide()[0];  /// get canvas element
                    var img = $(this).append("<img/>").find("img")[0]; /// get image element
                    img.src = canvas.toDataURL();
                });
            },
            //验证是否成功
            call_succeeded: function (serial) {
                var th = this;
                $api.get('Pay/MoneyAccount', { 'serial': serial }).then(function (req) {
                    if (req.data.success) {
                        th.moneyAccount = req.data.result;
                        if (!th.moneyAccount.Ma_IsSuccess) {
                            window.setTimeout(function () {
                                th.call_succeeded(serial);
                            }, 1000);
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err));
            }
        }
    });

}, ['/Utilities/Components/avatar.js',
    '/Utilities/Scripts/jquery.qrcode.min.js']);
