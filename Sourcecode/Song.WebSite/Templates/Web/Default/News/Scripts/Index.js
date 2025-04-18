$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},
            platinfo: {},
            org: {},
            config: {},

            columns: [],        //新闻栏目++

            articles: [],         //新闻文章
            loading: false
        },
        mounted: function () {
        },
        created: function () { },
        computed: {},
        watch: {
            'org': {
                handler: function (nv, ov) {
                    this.loadinit(nv.Org_ID);
                }, immediate: true
            }
        },
        methods: {
            //加载一些初始数据
            loadinit: function (orgid) {
                if (orgid == undefined) return;
                var th = this;
                th.loading = true;
                $api.bat(
                    $api.get('News/ArticlesShow', { 'orgid': orgid, 'uid': '', 'count': 15, 'order': 'hot' }),
                    $api.cache('News/ColumnsShow:60', { 'orgid': orgid, 'pid': '', 'count': 0 })
                ).then(([articles, columns]) => {
                    //获取结果
                    th.articles = articles.data.result;
                    th.columns = columns.data.result;
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
        }
    });
}, ['Components/Articles.js',
    "../Components/subject_rec.js",
    'Components/SearchInput.js']);
