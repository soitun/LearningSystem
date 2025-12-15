$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            tabs: [
                { name: '按分类选题', tab: 'parts', icon: 'a015' },
                { name: '按关键字选题', tab: 'e841', icon: 'e841' },
                { name: '按知识点选题', tab: 'knls', icon: 'e6fd' },
                { name: '我的收藏', tab: 'collect', icon: 'e747' },
            ],
            activeName: 'parts',
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
        
        },
        filters: {
        
        },
        components: {
        
        }
    });
});