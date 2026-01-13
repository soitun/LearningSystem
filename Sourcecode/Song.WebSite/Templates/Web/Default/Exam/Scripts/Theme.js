$ready([], function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项      
            examid: $api.querystring("id"),      //考试ID

            theme: {},       //考试主题
            accounttotal:0,     //需要参考学员总数
            loadstate: {               
                get: false,         //加载数据
                exams: false,      //加载考试场次
                
            }
        },
        mounted: function () {
            this.gettheme();
        },
        created: function () {

        },
        computed: {
            loading: function () {
                if (!this.loadstate) return false;
                for (let key in this.loadstate) {
                    if (this.loadstate.hasOwnProperty(key)
                        && this.loadstate[key])
                        return true;
                }
                return false;
            }
        },
        watch: {

        },
        methods: {
            //获取考试主题
            gettheme: function () {
                var th = this;
                th.loadstate.get = true;
                $api.get("Exam/ForID", { "id": th.examid })
                    .then(req => {
                        if (req.data.success) {
                            th.theme = req.data.result;
                            th.getaccounttotal();
                            th.getexmas();
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                    .finally(() => th.loadstate.get = false);
            },
             //获取参考学员的总数
             getaccounttotal: function () {
                var th = this;
                $api.cache('Exam/ScopeTotal', { 'uid': th.theme.Exam_UID })
                    .then(function (req) {
                        if (req.data.success) {
                            th.accounttotal = req.data.result;
                        } else throw req.data.message;
                    }).catch(err => console.error(err));
            },
            //获取“考试场次”
            getexmas: function () {
                var th = this;
                th.loadstate.exams = true;
                $api.get('Exam/Exams', { 'uid': th.theme.Exam_UID }).then(function (req) {
                    if (req.data.success) {
                        th.exams = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadstate.exams = false);
            },
        },
        filters: {

        },
        components: {

        }
    });
});