$ready(['../Question/Components/ques_type.js',
    'Components/papertype.js'],
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

                //关联的试题分类
                parts: [],
                tags: [],    //关联的关键字
                knls: [],   //关联的知识点
                //试题数量
                quescount: [],
                questotal: 0,

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
                    total: false          //获取试题数量
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
                    th.getquestotal();
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
                //试题的选择范围，这里是临时变量，用时监听选择范围是否变化
                range: function () {
                    return { a: this.parts, b: this.tags, c: this.knls };
                },
            },
            watch: {
                //当范围发生变化时
                range: {
                    handler: function (v) {
                        this.getquestotal();
                    },
                },
            },
            methods: {
                //难度范围变更时
                diffChange: function (val) {
                    this.entity['Etp_Diff'] = val[0];
                    this.entity['Etp_Diff2'] = val[1];
                },
                //打开子窗口
                //page:页面名称，
                opensubwin: function (page, title, icon) {
                    if (!window.top.$pagebox) return;
                    //子窗口页面路径
                    var suburl = $dom.routepath() + page;
                    if (page.toLowerCase().indexOf('part') > -1)
                        suburl = $api.url.set(suburl, { 'id': this.parts.map(p => p.Qp_ID).join(',') });
                    if (page.toLowerCase().indexOf('knl') > -1)
                        suburl = $api.url.set(suburl, { 'id': this.knls.map(p => p.Qk_ID).join(',') });
                    if (page.toLowerCase().indexOf('tag') > -1)
                        suburl = $api.url.set(suburl, { 'id': this.tags.map(p => p.Qtag_ID).join(',') });
                    //当前窗口
                    var curbox = window.top.$pagebox.get(window.name);
                    //创建新窗口中
                    var subbox = window.top.$pagebox.create({
                        width: 500, height: 300,
                        id: page, ico: icon, title: title,
                        url: suburl
                    });
                    //打开子窗口,窗口位置:left,right,top,bottom
                    curbox.opensub(subbox, 'left');
                },
                //接收子窗口数据
                //data:子窗口返回的数据
                //func:要调用的函数名称
                receive: function ([data, func]) {
                    if (func == 'selectpart') this.parts = data;
                    if (func == 'selectknl') this.knls = data;
                    if (func == 'selecttag') this.tags = data;
                    this.getquestotal();
                },
                //获取可供选择的试题数量
                getquestotal: function () {
                    var th = this;
                    th.loadstate.total = true;
                    console.error(3);
                    let form = { "orgid": "", "qpid": "", "tagid": "", "knlid": "", "isdeleted": false, "diff": "", "use": true, "error": false, "wrong": false };
                    form.orgid = th.org.Org_ID;
                    form.qpid = th.parts.map(p => p.Qp_ID).join(',');
                    form.tagid = th.tags.map(p => p.Qtag_ID).join(',');
                    form.knlid = th.knls.map(p => p.Qk_ID).join(',');
                    $api.get("ExamQues/QuesTotal", form)
                        .then(req => {
                            if (req.data.success) {
                                th.quescount = req.data.result;
                                th.questotal = Object.values(this.quescount).reduce((sum, price) => sum + price, 0);
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(err => console.error(err))
                        .finally(() => th.loadstate.total = false);
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