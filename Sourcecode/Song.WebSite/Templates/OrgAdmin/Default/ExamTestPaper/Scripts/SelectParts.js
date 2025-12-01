$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
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
            //打开子窗口
            opensubwin: function (page, place) {
                if (!window.top.$pagebox) return;
                //当前窗口
                var curbox = window.top.$pagebox.get(window.name);
                //创建新窗口中
                var subbox = window.top.$pagebox.create({
                    width: 500, height: 300,
                    id: page + Math.random(), ico: 'a053',
                    url: $dom.routepath() + page
                });
                curbox.opensub(subbox, place);
            },
        },
        filters: {

        },
        components: {

        }
    });
});