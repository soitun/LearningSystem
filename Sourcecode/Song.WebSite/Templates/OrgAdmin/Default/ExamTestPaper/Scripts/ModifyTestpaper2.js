$ready(['../Question/Components/ques_type.js',],
    function () {
        window.vapp = new Vue({
            el: '#vapp',
            data: {
                id: $api.querystring('id'), //主键ID
                org: {},          //当前机构对象
                config: {},      //当前机构配置项  
                types: [],       //题型
                tabs: [
                    { name: '基本信息', tab: 'general', icon: 'e72f' },
                    { name: '出题范围', tab: 'range', icon: 'e731' },
                    { name: '分数', tab: 'score', icon: 'e731' },
                    { name: '注意事项', tab: 'remind', icon: 'e697' },
                    { name: '其它', tab: 'other', icon: 'e67e' }
                ],
                activeName: 'range',
                //试卷对象  
                entity: {
                    Etp_Id: 0,        //主键
                    Etp_IsUse: true,
                    Etp_Span: 120,    //默认限时 120分钟
                    Etp_Type: 2,
                    Etp_Total: 100, Etp_PassScore: 60,
                    Etp_Diff: 1,
                    Etp_Diff2: 5,
                    Etp_FromConfig: '',
                    Etp_FromType: 0,
                    Sbj_ID: 0,
                    Cou_ID: 0           //所属课程的id
                },
                //试题的类型数据，例如题型，该题型的题量，分数，分数占比，
                qtypeitems: [],
                //图片文件
                upfile: null, //本地上传文件的对象         
                Etp_Diff: [1, 5],     //难度范围

                //选中的试题分类
                parts: [],

                //录入校验的规划
                rules: {
                    Etp_Name: [
                        { required: true, message: '标题不得为空', trigger: 'blur' },
                        { min: 1, max: 255, message: '最长输入255个字符', trigger: 'change' },
                        { validator: validate.name.proh, trigger: 'change' },   //禁止使用特殊字符
                        { validator: validate.name.danger, trigger: 'change' },
                        {
                            validator: function (rule, value, callback) {
                                let v = $api.trim(value);
                                if (v == '' || v.length < 1) return callback(new Error('不能全部是空格'));
                                return callback();
                            }, trigger: 'blur'
                        }
                    ],
                    Etp_Span: [
                        { required: true, message: '限时不得为空', trigger: 'blur' },
                        {
                            validator: function (rule, value, callback) {
                                if (Number(value) <= 0) {
                                    callback(new Error('请输入大于零的整数'));
                                } else {
                                    callback();
                                }
                            }, trigger: 'blur'
                        }
                    ],
                    Etp_Total: [
                        { required: true, message: '分数不得为空', trigger: 'blur' },
                        {
                            validator: function (rule, value, callback) {
                                if (Number(value) <= 0) {
                                    callback(new Error('请输入大于零的整数'));
                                } else {
                                    callback();
                                }
                            }, trigger: 'blur'
                        }
                    ],
                    Etp_PassScore: [
                        { required: true, message: '分数不得为空', trigger: 'blur' },
                        {
                            validator: function (rule, value, callback) {
                                if (Number(value) <= 0) {
                                    callback(new Error('请输入大于零的整数'));
                                } else if (Number(value) > vapp.entity.Etp_Total) {
                                    callback(new Error('及格分不得大于满分'));
                                } else {
                                    callback();
                                }
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
                var th = this;
                th.org = window.org;
                th.config = window.config;
                //获取题型
                th.loadstate.init = true;
                $api.bat(
                    $api.cache('Question/Types:99999')
                ).then(([types]) => {
                    th.types = types.data.result;
                    th.qtypeitems = th.types.map((t, i) => {
                        return {
                            type: i + 1,  //题型，数字表示
                            name: t, //题型名称
                            total: 0,       //可供选择的题量
                            count: 0,        //题量
                            score: 0,   //分数
                            rate: 0,   //分数占比
                        }
                    });
                    //console.error(th.qtypeitems);
                }).catch(err => console.error(err))
                    .finally(() => th.loadstate.init = false);
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
                },
                //是否为新增
                isadd: t => t.id == null || t.id == '' || this.id == 0,
                //是否存在试卷对象
                exist: t => !$api.isnull(t.entity) && t.entity.Etp_Id != '',
            },
            watch: {

            },
            methods: {
                //难度范围变更时
                diffChange: function (val) {
                    this.entity['Etp_Diff'] = val[0];
                    this.entity['Etp_Diff2'] = val[1];
                },
                //打开子窗口
                //page:页面名称，place:子窗口相对于当前窗口位置，left,right,top,bottom
                opensubwin: function (page, place) {
                    if (!window.top.$pagebox) return;
                    //子窗口页面路径
                    var suburl = $dom.routepath() + page;
                    if (page.toUpperCase() === 'SelectParts'.toUpperCase())
                        suburl = $api.url.set(suburl, { 'id': this.parts.map(p => p.Qp_ID).join(',') });
                    //当前窗口
                    var curbox = window.top.$pagebox.get(window.name);
                    //创建新窗口中
                    var subbox = window.top.$pagebox.create({
                        width: 500, height: 300,
                        id: page, ico: 'a015', title: '试题分类选择',
                        url: suburl
                    });
                    curbox.opensub(subbox, place);
                },
                //接收子窗口数据
                //data:子窗口返回的数据
                //func:要调用的函数名称
                receive: function ([data, func]) {
                    console.error(data);
                    if (func == 'selectpart') {
                        this.parts = data;
                    }
                },
                //
                selectpart: function (data) {
                },
                //操作成功
                operateSuccess: function (isclose) {
                    //如果处于课程编辑页，则刷新
                    var pagebox = window.top.$pagebox;
                    if (pagebox && pagebox.source.top)
                        pagebox.source.top(window.name, 'vapp.fresh_frame', isclose);
                }
            },
            filters: {

            },
            components: {

            }
        });
    });