$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},
            platinfo: {},
            org: {},
            config: {},

            uid: $api.dot(),        //新闻栏目uid
            articles: [],        //新闻列表
            column: {},
            query: {
                'uid': '', 'search': '',
                'order': 'top', 'size': 15, 'index': $api.querystring("index", 1)
            },
            total: 1, //总记录数
            totalpages: 1, //总页数

            courses: [],     //课程列表
            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading = true;
            $api.get('News/ColumnsForUID', { 'uid': this.uid }).then(function (req) {
                if (req.data.success) {
                    th.column = req.data.result;
                    document.title = th.column.Col_Name;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                //alert(err);
                console.error(err);
            });
            this.getcourses();
            this.getArticles();
        },
        created: function () { },
        computed: {
            //栏目是否为空，即不存在
            isempty: function () {
                return !(JSON.stringify(this.column) != '{}' && this.column != null);
            }
        },
        watch: {},
        methods: {
            //获取课程
            getcourses: function () {
                var th = this;
                $api.get('Course/ShowCount', { 'sbjid': -1, 'orgid': th.org.Org_ID, 'search': '', 'order': 'rec', 'count': 6 })
                    .then(function (req) {
                        if (req.data.success) {
                            th.courses = req.data.result;
                            console.log(th.courses);
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {

                        console.error(err);
                    });
            },
            //获取新闻文章
            getArticles: function (index) {
                var th = this;
                if (index != null) th.query.index = index;
                th.query.uid = this.uid;
                var query = this.query;
                $api.get('News/ArticlePagerShow', query).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.total = req.data.total;
                        th.articles = req.data.result;
                        th.totalpages = req.data.totalpages;
                        //更改地址栏，添加分页索引
                        var url = $api.setpara('index', th.query.index);
                        history.pushState({}, "", url);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }

                }).catch(function (err) {
                    th.loading = false;
                    th.error = err;
                    console.error(err);
                });
            }
        }
    });

});
