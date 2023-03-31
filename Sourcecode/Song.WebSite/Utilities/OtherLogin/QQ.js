﻿// 第三方登录的配置项
Vue.component('qq', {
    props: [],
    data: function () {
        return {
            code: $api.querystring('code'),     //第三方平台登录成功后返回的code
            state: $api.querystring('state'),   //自己配置的信息，由第三方平台原样返回，此处为tag+','+type
            tag: $api.querystring('tag'),
            type: $api.querystring('type'),

            token: $api.querystring('token'),
            openid: $api.querystring('openid'),

            //第三登录的配置项
            object: {},

            //qq用户信息
            user: {}
            /** 示例
             {
                "ret": 0,
                "msg": "",
                "is_lost":0,
                "nickname": "碧水寒天",
                "gender": "男",
                "gender_type": 2,
                "province": "广东",
                "city": "深圳",
                "year": "1990",
                "constellation": "",
                "figureurl": "http:\/\/qzapp.qlogo.cn\/qzapp\/101436449\/58DCE5776EDC704DD286D7C409404CE7\/30",
                "figureurl_1": "http:\/\/qzapp.qlogo.cn\/qzapp\/101436449\/58DCE5776EDC704DD286D7C409404CE7\/50",
                "figureurl_2": "http:\/\/qzapp.qlogo.cn\/qzapp\/101436449\/58DCE5776EDC704DD286D7C409404CE7\/100",
                "figureurl_qq_1": "http://thirdqq.qlogo.cn/g?b=oidb&k=BuhTEI7icaATGmuRP737WtQ&kti=ZB7IYQAAAAI&s=40&t=1555306155",
                "figureurl_qq_2": "http://thirdqq.qlogo.cn/g?b=oidb&k=BuhTEI7icaATGmuRP737WtQ&kti=ZB7IYQAAAAI&s=100&t=1555306155",
                "figureurl_qq": "http://thirdqq.qlogo.cn/g?b=oidb&k=BuhTEI7icaATGmuRP737WtQ&kti=ZB7IYQAAAAI&s=640&t=1555306155",
                "figureurl_type": "1",
                "is_yellow_vip": "0",
                "vip": "0",
                "yellow_vip_level": "0",
                "level": "0",
                "is_yellow_year_vip": "0"
            }
            */
        }
    },
    watch: {
        //返回状态值
        'state': {
            handler: function (nv, ov) {
                if (nv == null || nv == '') return;
                nv = decodeURIComponent(nv);
                if (nv.indexOf(',') > -1) {
                    var arr = nv.split(',');
                    this.tag = arr[0];
                    this.type = arr[1];
                    this.getobject(true);
                }
            }, immediate: true,
        },
        //qq的唯一标识
        'openid': {
            handler: function (nv, ov) {
                //当openid不为空，且code为空时，说明已经跳转过
                if (nv != null && nv != '' && this.code == '') {
                    this.getobject();
                    //this.getUserInfo(this.token, nv);
                }
            }, immediate: true,
        }
    },
    computed: {
    },
    mounted: function () {

    },
    created: function () {

    },
    methods: {
        //获取登录配置项的记录
        getobject: function (gettoken) {
            var th = this;
            $api.get('OtherLogin/GetObject', { 'tag': th.tag }).then(function (req) {
                if (req.data.success) {
                    th.object = req.data.result;
                    if (gettoken)
                        th.gettoken(th.object);
                    else
                        th.getUserInfo(th.token, th.openid);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        //获取token
        gettoken: function (obj) {
            var url = "https://graph.qq.com/oauth2.0/token?client_id={0}&client_secret={1}&code={2}&grant_type=authorization_code&redirect_uri={3}";
            url = $api.url.set(url, {
                'client_id': obj.Tl_APPID,
                'client_secret': obj.Tl_Secret,
                'code': this.code,
                'grant_type': 'authorization_code',
                'redirect_uri': encodeURIComponent(obj.Tl_Returl),
            });
            var th = this;
            $api.get('OtherLogin/HttpRequest', { 'url': url }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    if (result.indexOf('access_token') > -1) {
                        //成功
                        //示例：'access_token=91DFA5A2931767F43F4810F25EDFBF60&expires_in=7776000&refresh_token=4CAD6BCF1614B244871CDFDA892C13D2'
                        var obj = {};
                        var arr = result.split('&');
                        for (let i = 0; i < arr.length; i++) {
                            const item = arr[i].split('=');
                            obj[item[0]] = item[1];
                        }
                        th.token = obj.access_token;
                        th.getOpenid(th.token);
                    } else {
                        //失败
                        //示例：callback( {"error":100020,"error_description":"code is reused error"} );
                        var reg = new RegExp("\{.*\}", "gi");
                        var r = result.match(reg); //匹配目标参数
                        var obj = $api.parseJson(r[0]);
                        throw obj;
                    }
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });

        },
        //获取Openid
        getOpenid: function (token) {
            var url = "https://graph.qq.com/oauth2.0/me?access_token=" + token;
            var th = this;
            $api.get('OtherLogin/HttpRequest', { 'url': url }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    if (result.indexOf('openid') > -1) {
                        //示例：callback( {"client_id":"101436449","openid":"58DCE5776EDC704DD286D7C409404CE7"} );
                        var reg = new RegExp("\{.*\}", "gi");
                        var r = result.match(reg); //匹配目标参数
                        var obj = $api.parseJson(r[0]);
                        th.openid = obj.openid;
                        th.getUserInfo(th.token, th.openid);
                        //跳转
                        /*
                        var url = window.location.href;
                        if (url.indexOf('?') > -1) url = url.substring(0, url.lastIndexOf('?'));
                        url = $api.url.set(url, {
                            token: th.token,
                            openid: th.openid,
                            tag: th.tag,
                            type: th.type
                        });
                        window.parent.location.href = url;*/
                    }

                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        //获取QQ用户信息
        getUserInfo: function (token, openid) {
            var url = "https://graph.qq.com/user/get_user_info?access_token={0}&oauth_consumer_key={1}&openid={2}";
            url = url.format(token, this.object.Tl_APPID, openid);
            var th = this;
            $api.get('OtherLogin/HttpRequest', { 'url': url }).then(function (req) {
                if (req.data.success) {
                    var result = JSON.parse(req.data.result);
                    result['openid'] = openid;
                    result['tag'] = th.tag;     //第三方登录的配置项标识
                    th.$emit('load', result, th.type);
                    console.log(result);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        }
    },
    template: `<div >
       
    </div>`
});
