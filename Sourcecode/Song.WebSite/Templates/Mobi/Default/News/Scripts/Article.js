$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项       
            
            arid: $api.dot(),        //新闻id
            isformat: $api.storage('article_isformat') == 'true',         //是否格式化
            article: {},        //新闻对象
            accessory: [],       //新闻附件
            column: {},          //栏目信息
            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading = true;
            $api.cache('News/Article', { 'id': this.arid }).then(function (req) {
                if (req.data.success) {
                    th.article = req.data.result;
                    document.title = th.article.Art_Title;
                    $api.bat(
                        $api.cache('News/ColumnsForUID', { 'uid': th.article.Col_UID }),
                        $api.cache('News/VisitPlusOne:60', { 'id': th.article.Art_ID }),
                        $api.cache("News/Accessory", { 'uid': th.article.Art_Uid })
                    ).then(axios.spread(function (column, visit, accessory, num) {
                        //栏目信息
                        th.column = column.data.result;
                        //访问量加一，并给当前新闻加上这个数
                        th.article.Art_Number = visit.data.result;
                        //新闻附件
                        th.accessory = accessory.data.result;
                    })).catch(function (err) {
                        console.error(err);
                    });
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        created: function () { },
        computed: {},
        watch: {
            //是否格式化
            'isformat': {
                handler: function (nv, ov) {
                    if (nv != null)
                        $api.storage('article_isformat', nv);
                }, immediate: false,
            }
        },
        methods: {}
    });

}, ['Components/SearchInput.js']);
