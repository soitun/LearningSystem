$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //试卷对象  
            entity: {},
            //试题的类型数据，例如题型，该题型的题量，分数，分数占比，
            qtypeitems: [],

            loadstate: {
                init: false,        //初始化
                def: false,         //默认
                get: false,         //加载数据
                update: false,      //更新数据
                del: false          //删除数据
            }
        },
        mounted: function () {
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
            //向“更多分数设置”的主窗体传递数据
            transmit: function () {

                //试卷对象，题型分数分配的数据
                return [this.entity, this.qtypeitems];
            },
            //接收“更多分数设置”的主窗体数据
            receive: function () {
                //像主窗体传值，传三个值：选中的分类，选中的试题数，调用函数名
                var pagebox = window.top.$pagebox;
                if (pagebox && pagebox.source.top) {
                    [this.entity, this.qtypeitems] = pagebox.source.box(window.name, 'vapp.scoretransmit', false);
                    console.error(this.entity);
                    console.error(this.qtypeitems);
                }

            },
        },
        filters: {

        },
        components: {

        }
    });
});