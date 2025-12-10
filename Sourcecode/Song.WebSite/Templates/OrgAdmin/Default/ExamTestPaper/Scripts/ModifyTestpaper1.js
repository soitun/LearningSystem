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
            }
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