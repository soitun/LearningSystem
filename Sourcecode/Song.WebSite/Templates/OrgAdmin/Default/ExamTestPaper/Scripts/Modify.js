$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'), //主键ID
            loadstate: {
                init: false,        //初始化
                def: false,         //默认
                get: false,         //加载数据
                update: false,      //更新数据
                del: false          //删除数据
            }
        },
        mounted: function () {

        },
        created: function () {

        },
        computed: {
            //加载状态
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
            //跳转到指定类型页面, type:1静态试卷，2动态随机试卷
            gototype: function (type) {
                //console.error(type);
                let winname = window.name;
                let $pagebox = window.top.$pagebox;
                let box = $pagebox.get(winname);
                //box.toSize(1200,900);
                //console.error(box);
                box.ondrag(function (s, e) {
                    //console.error(box.width, box.height);
                    //console.error(e);
                });
                box.onmove(function (s, e) {
                    //console.error(box.left, box.top);
                    console.error(e);
                });
                box.toMove(100, 100);
            },
        },
        filters: {

        },
        components: {

        }
    });
});