$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'), //主键ID
            org: {},          //当前机构对象
            config: {},      //当前机构配置项  
            types: [],       //题型
            //试题的类型数据，例如题型，该题型的题量，分数，分数占比，
            qtypeitems: [],
            //图片文件
            upfile: null, //本地上传文件的对象         
            Etp_Diff: [1, 5],     //难度范围
            //试卷对象  
            entity: {
                Etp_Id: 0,        //主键
                Etp_Name: '考试的标题', Etp_SubName: '2025年中考',
                Etp_IsUse: true,
                Etp_Span: 120,    //默认限时 120分钟
                Etp_Type: 2,
                Etp_Total: 100, Etp_PassScore: 60,      //总分，及格分
                Etp_Diff: 1,
                Etp_Diff2: 5,
                Etp_FromConfig: '',
                Etp_Count: 0,       //总题量                   
            },
            loadstate: {
                init: false,        //初始化
                def: false,         //默认
                get: false,         //加载数据
                update: false,      //更新数据
                del: false          //删除数据
            }
        },
        mounted: function () {
            var th = this;
            th.org = window.org;
            th.config = window.config;
            //获取题型
            th.loadstate.init = true;
            $api.bat(
                $api.cache('Question/Types:99999')
            ).then(([types]) => {
                th.types = types.data.result;
                //各题型的题量分数的配置
                th.qtypeitems = th.types.map((t, i) => {
                    return {
                        type: i + 1,  //题型，数字表示
                        name: t,    //题型名称
                        byname: '',    //题型的别名
                        total: 0,       //可供选择的题量
                        count: 0,        //题量
                        score: 0,       //分数
                        percent: 0,   //分数占比
                    }
                });
                //th.getquestotal();
                if (!th.isadd) th.getentity();
            }).catch(err => console.error(err))
                .finally(() => th.loadstate.init = false);
        },
        created: function () {
            //更改当前窗体的标题
            var pagebox = window.top.$pagebox;
            if (pagebox && pagebox.source) {
                let box = pagebox.source.self(window.name);
                box.title = box.title.substring(0, box.title.lastIndexOf('-') + 1) + ' 固定试题';
                window.setTimeout(function () {
                    box.full = true;
                }, 200);
            }
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
            },
            //是否为新增
            isadd: t => t.id == null || t.id == '' || this.id == 0,
        },
        watch: {

        },
        methods: {

        },
        filters: {

        },
        components: {

        }
    });
});