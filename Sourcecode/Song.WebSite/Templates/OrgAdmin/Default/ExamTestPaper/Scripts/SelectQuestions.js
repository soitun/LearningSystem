$ready([
    '/Utilities/Components/question/function.js',
    '../Question/Components/ques_type.js',      //题型
    '../ExamQues/Components/ques_diff.js',      //难度
    '../ExamQues/Components/ques_collect.js',   //收藏
    '../ExamQues/Components/tagselect.js',  //标签
    'Components/selectques.js',        //试题行
    'Components/diff.js',      //难度选择
    'Components/selectparts.js',        //试题分类的选择
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
            //选中的试题,试题id
            //selectedarr: $api.querystring('ques', '').split(',').filter(item => item !== ''),
            selectedques: [],       //选中的试题
            //按试题分类查询条件
            partsform: {
                "orgid": -1, "search": "", "isdeleted": false, "qpid": "", "tagid": "", "knlid": "", "type": $api.querystring('type'),
                "diff": "", "use": true, "error": false, 'wrong': false, "size": 10, "index": 1
            },
            partstotal: 1, //总记录数
            partspages: 1, //总页数
            partsques: [],


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

            this.receive();
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
            //试题分类的试题
            getpartques: function (index, parts) {
                var th = this;
                th.loadstate.parts = true;
                if (index != null) this.partsform.index = index;
                if (parts != null) this.partsform.qpid = parts.map(user => user.Qp_ID).join(',');
                $api.get("ExamQues/QuesPager", th.partsform).then(function (d) {
                    if (d.data.success) {
                        var result = d.data.result;
                        for (let i = 0; i < result.length; i++) {
                            result[i] = window.ques.parseAnswer(result[i]);
                            //试题是否选中
                            result[i]["checked"] = th.selectedques.some(m => m.Qus_ID == result[i].Qus_ID);
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
            },
            //接收的主窗体数据
            receive: function () {
                return new Promise((resolve, reject) => {
                    //像主窗体传值，传三个值：选中的分类，选中的试题数，调用函数名
                    var pagebox = window.top.$pagebox;
                    if (pagebox && pagebox.source.top) {
                        [this.selectedques] = pagebox.source.box(window.name, 'vapp.transmitques', false, this.type);
                        resolve(this.entity);
                    }
                });
            },
            //试题选中内容变更,并向主窗体传值
            changeselect: function (q, checked) {
                if (checked) this.selectedques.push(q);
                else this.selectedques = this.selectedques.filter(item => item.Qus_ID !== q.Qus_ID);
                //像主窗体传值，当前实体，图片对象
                var pagebox = window.top.$pagebox;
                if (pagebox && pagebox.source.box) {
                    pagebox.source.box(window.name, 'vapp.receiveques', false, [this.selectedques, this.type]);
                    //let curbox = pagebox.source.self(window.name);
                    //curbox.shut();
                }
            }

        },
        filters: {

        },
        components: {

        }
    });
});