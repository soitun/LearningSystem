$ready([
    '/Utilities/Components/question/function.js',
    '../Question/Components/ques_type.js',      //题型
    '../ExamQues/Components/ques_diff.js',      //难度
    '../ExamQues/Components/ques_collect.js',   //收藏
    '../ExamQues/Components/tagselect.js',  //标签
    'Components/quesrow.js',        //试题行
    'Components/diff.js',      //难度选择
], function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            org: window.org,      //当前机构对象     
            type: $api.querystring('type'),    //当前题型
            types: $api.querystring('types').split(','),       //所有题型
            admin: {},          //当前登录用户

            tabs: [
                { name: '按分类选题', tab: 'parts', icon: 'a015' },
                { name: '按关键字选题', tab: 'tags', icon: 'e841' },
                { name: '按知识点选题', tab: 'knls', icon: 'e6fd' },
                { name: '我的收藏', tab: 'collect', icon: 'e747' },
            ],
            activeName: 'parts',
            //查询条件
            partsform: {
                "orgid": -1, "search": "", "isdeleted": false, "qpid": "", "tagid": "", "knlid": "", "type": $api.querystring('type'),
                "diff": "", "use": true, "error": false, 'wrong': false, "size": 10, "index": 1
            },
            partstotal: 1, //总记录数
            partspages: 1, //总页数
            partsques: [],
            parts: [],

            tags: [],
            knls: [],
            collect: [],


            loadstate: {
                init: false,        //初始化
                parts: false,         //试题分类
                get: false,         //加载数据
                update: false,      //更新数据
                del: false          //删除数据
            }
        },
        mounted: function () {
            var th = this;
            this.partsform.orgid = window.org.Org_ID;
            //当前登录的管理员
            $api.login.current('admin', d => th.admin = d);
            this.getparts().then(t => t.getpartques());
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
            //所取试题分类的数据，为树形数据
            getparts: function () {
                var th = this;
                return new Promise(function (resolve, reject) {
                    th.loadstate.parts = true;
                    $api.get('ExamQues/PartTree', { orgid: window.org.Org_ID, search: '', isuse: true })
                        .then(function (req) {
                            if (req.data.success) {
                                th.parts = req.data.result;
                                resolve(th);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(err => console.error(err))
                        .finally(() => th.loadstate.parts = false);
                });
            },
            //试题分类的试题
            getpartques: function (index) {
                var th = this;
                th.loadstate.parts = true;
                if (index != null) this.partsform.index = index;
                $api.get("ExamQues/QuesPager", th.partsform).then(function (d) {
                    if (d.data.success) {
                        var result = d.data.result;
                        for (let i = 0; i < result.length; i++) {
                            result[i] = window.ques.parseAnswer(result[i]);
                            result[i]["checked"] = false;
                            result[i]["showdetail"] = true;
                            //result[i].Qus_Title = result[i].Qus_Title.replace(/(<([^>]+)>)/ig, "");
                        }

                        th.partsques = result;
                        th.partspages = Number(d.data.totalpages);
                        th.partstotal = d.data.total;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loadstate.parts = false);
            }
        },
        filters: {

        },
        components: {

        }
    });
});