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
            //卡片鼠标移入
            cardHover:function(el){
                //el.style.backgroundColor = '#f5f5f5';
                console.error(el);
            }
        },
        filters: {

        },
        components: {

        }
    });
});