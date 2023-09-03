
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            organ: {},
            config: {},

            quantity: {},        //数量

            test: { number: [56], content: '{nt}个' }

        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (platinfo, organ) {
                th.loading_init = false;
                th.platinfo = platinfo.data.result;
                document.title += ' - ' + th.platinfo.title;
                th.organ = organ.data.result;
                th.config = $api.organ(th.organ).config;

                th.getquantity();

            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {


        },
        methods: {
            //获取一些统计数据
            getquantity: function () {
                var th = this;
                $api.get('Platform/Datas', { 'orgid': '' }).then(function (req) {
                    if (req.data.success) {
                        th.quantity = req.data.result;
                        console.group("相关数据统计")
                        console.log(th.quantity);

                        th.test.number = [th.quantity.course];
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    //Vue.prototype.$alert(err);
                    console.error(err);
                });
            }
        }
    });

}, ["/Utilities/Viewport/datav.min.vue.js",
    "Components/number.js",
    "Components/studylog.js",
    "Components/map_henan.js"]);
