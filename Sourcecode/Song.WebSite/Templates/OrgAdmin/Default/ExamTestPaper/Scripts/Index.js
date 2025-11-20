$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            org: window.org,    //当前机构
            types: [],        //试题类型，来自web.config中配置项
            admin: {},          //当前登录用户
            //试题的查询条件
            form: {
                "orgid": -1, "search": "", "isdeleted": false, "qpid": "", "tagid": "", "knlid": "", "type": "", "diff": "",
                "use": "", "error": "", 'wrong': "",
                "size": 10, "index": 1
            },
            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            loadstate: {
                init: false,        //初始化
                def: false,         //默认
                get: false,         //加载数据
                update: false,      //更新数据
                del: false          //删除数据
            },
            loadingid: 0,
        },
        mounted: function () {
        
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
            handleCurrentChange:function (val) {
                this.currentPage = val;
                this.getData();
            },
        },
        filters: {
        
        },
        components: {
        
        }
    });
});