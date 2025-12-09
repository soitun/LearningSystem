$ready(['../Question/Components/ques_type.js',],
    function () {
        window.vapp = new Vue({
            el: '#vapp',
            data: {
                types: [],       //题型
                //试卷对象  
                entity: {},
                //试题的类型数据，例如题型，该题型的题量，分数，分数占比，
                qtypeitems: [],
                scoreitems: [],      //用于分数计算，是qtypeitems的拷贝
                //录入校验的规划
                rules: {
                    Etp_Total: [
                        { required: true, message: '分数不得为空', trigger: 'blur' },
                        {
                            validator: function (rule, value, callback) {
                                if (!(/^[1-9]\d*$/.test(value))) return callback(new Error('请输入大于零的整数'));;
                                if (Number(value) < Number(vapp.entity.Etp_PassScore))
                                    return callback(new Error('试卷总分不得小于及格分'));;
                                callback();
                            }, trigger: 'blur'
                        }
                    ],
                    Etp_PassScore: [
                        { required: true, message: '分数不得为空', trigger: 'blur' },
                        {
                            validator: function (rule, value, callback) {
                                if (!(/^[1-9]\d*$/.test(value))) return callback(new Error('请输入大于零的整数'));
                                if (Number(value) > vapp.entity.Etp_Total) callback(new Error('及格分不得大于满分'));
                                else callback();

                            }, trigger: 'blur'
                        }
                    ],
                    total: [
                        {
                            validator: function (rule, value, callback) {
                                let items = vapp.qtypeitems;
                                let percent = items.reduce((a, b) => a + b.percent, 0);
                                if (percent > 100) return callback(new Error('各题型分数占比之和不能大于100'));
                                if (percent != 100) return callback(new Error('各题型分数占比之和必须等于100'));

                                for (let i = 0; i < items.length; i++) {
                                    const el = items[i];
                                    if (el.count > el.total) {
                                        return callback(new Error('' + el.name + '题的数量不能大于' + el.total + '道'));
                                    }
                                    if (el.count <= 0 && el.percent > 0) {
                                        return callback(new Error('' + el.name + '题的数量不能小于1道'));
                                    }
                                    if (el.percent <= 0 && el.count > 0) {
                                        return callback(new Error('' + el.name + '题的分数不可小于1分'));
                                    }
                                }
                                callback();
                            }, trigger: 'blur'
                        }
                    ]
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
                this.receive();
                this.$nextTick(function () {
                    //初始化
                    vapp.rowdrop();
                });
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
                transmit: function (entity, qtypeitems) {
                    //像主窗体传值，传三个值：选中的分类，选中的试题数，调用函数名
                    var pagebox = window.top.$pagebox;
                    if (pagebox && pagebox.source.top) {
                        //试卷对象，题型分数分配的数据
                        pagebox.source.box(window.name, 'vapp.scorereceive', false, [entity, qtypeitems]);
                    }
                },
                //接收“更多分数设置”的主窗体数据
                receive: function () {
                    //像主窗体传值，传三个值：选中的分类，选中的试题数，调用函数名
                    var pagebox = window.top.$pagebox;
                    if (pagebox && pagebox.source.top) {
                        [this.entity, this.types, this.qtypeitems] = pagebox.source.box(window.name, 'vapp.scoretransmit', false);
                        this.scoreitems = this.qtypeitems;
                    }
                },
                //当试卷总分更改时
                chanageTotal: function () {
                    let tptotal = this.entity.Etp_Total;
                    this.qtypeitems.forEach(el => {
                        el.score = Math.floor(el.percent * tptotal / 100);
                    });
                },
                //行的拖动
                rowdrop: function () {
                    // 首先获取需要拖拽的dom节点            
                    const el1 = document.querySelectorAll('div.qtype_card .el-card__body')[0];
                    if (el1 == null) return;
                    Sortable.create(el1, {
                        disabled: false, // 是否开启拖拽
                        ghostClass: 'sortable-ghost', //拖拽样式
                        handle: '.draghandle',     //拖拽的操作元素
                        animation: 150, // 拖拽延时，效果更好看
                        group: { pull: false, put: false },
                        onEnd: (e) => {
                            let typerow = $dom(".qtypeitems");
                            var th = this;
                            var arr = [];
                            typerow.each(function () {
                                let type = Number($dom(this).attr('type'));
                                let item = th.qtypeitems.find(el => Number(el.type) == type);
                                arr.push(item);
                            });
                            th.$nextTick(function () {
                                th.rowdrop();
                                th.transmit(th.entity, arr);

                            });
                            th.scoreitems = [];
                            th.scoreitems = arr;
                        }
                    });
                },
            },
            filters: {

            },
            components: {
                //试题题型的分数
                'scores': {
                    props: ['count', 'score'],
                    data: function () {
                        return {
                            list: []
                        }
                    },
                    computed: {
                        'data': t => [t.count, t.score]
                    },
                    watch: {
                        'data': {
                            handler: function (val) {
                                let [count, score] = val;
                                this.list = [];
                                if (count == 0) this.list = [];
                                else {
                                    let num = Math.floor(score / count * 10) / 10;
                                    let tmtotal = 0;  //题型总分，计算所得
                                    for (let i = 0; i < count; i++) {
                                        tmtotal += num;
                                        this.list.push(num);
                                    }
                                    if (tmtotal != score) {
                                        let last = this.list[this.list.length - 1];
                                        this.list[this.list.length - 1] = Math.floor((last + score - tmtotal) * 10) / 10;
                                    }
                                }
                            }, immediate: true,
                        }
                    },
                    methods: {},
                    template: `<div class="scores">
                <div v-for="(item,idx) in list" :zero="item<=0">
                    <span>{{idx+1}}</span>
                    <span :zero="item<=0">{{item}} 分</span>
                </div>
            </div>`
                }
            }
        });
    });