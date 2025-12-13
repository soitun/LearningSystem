$ready(['../Question/Components/ques_type.js',
    'Components/papertype.js'], function () {
        window.vapp = new Vue({
            el: '#vapp',
            data: {
                id: $api.querystring('id'), //主键ID
                org: {},          //当前机构对象
                config: {},      //当前机构配置项  
                types: [],       //题型
                //试题的类型数据，例如题型，该题型的题量，分数，分数占比，
                qtypeitems: [],
                //图片文件
                upfile: null, //本地上传文件的对象         
                Etp_Diff: [1, 5],     //难度范围
                //试卷对象  
                entity: {
                    Etp_Id: 0,        //主键
                    Etp_Name: '试卷名称，随便打几个字', Etp_SubName: '',
                    Etp_IsUse: true,
                    Etp_Span: 120,    //默认限时 120分钟
                    Etp_Type: 2,
                    Etp_Total: 100, Etp_PassScore: 60,      //总分，及格分
                    Etp_Diff: 1,
                    Etp_Diff2: 5,
                    Etp_Remind: '',
                    Etp_Count: 0,       //总题量                   
                },
                //录入校验的规划
                rules: {
                    Etp_Total: [
                        { required: true, message: '分数不得为空', trigger: 'blur' },
                        {
                            validator: function (rule, value, callback) {
                                if (/^[1-9]\d*$/.test(value)) return callback();
                                callback(new Error('请输入大于零的整数'));
                            }, trigger: 'blur'
                        }
                    ],
                    Etp_PassScore: [
                        { required: true, message: '及格分不得为空', trigger: 'blur' },
                        {
                            validator: function (rule, value, callback) {
                                if (!(/^[1-9]\d*$/.test(value))) return callback(new Error('请输入大于零的整数'));
                                if (Number(value) > vapp.entity.Etp_Total) callback(new Error('及格分不得大于满分'));
                                else callback();

                            }, trigger: 'blur'
                        }
                    ],
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
                    //各题型的题量分数的配置
                    th.qtypeitems = th.types.map((t, i) => {
                        return {
                            type: i + 1,  //题型，数字表示
                            name: t,    //题型名称
                            byname: '',    //题型的别名
                            total: 0,       //可供选择的题量
                            count: 0,        //题量
                            number: 0,       //分数
                            percent: 0,   //分数占比
                        }
                    });
                    //th.getquestotal();
                    if (!th.isadd) th.getentity();
                }).catch(err => console.error(err))
                    .finally(() => th.loadstate.init = false);
            },
            created: function () {
                //更改当前窗体的标题
                var pagebox = window.top.$pagebox;
                if (pagebox && pagebox.source) {
                    let box = pagebox.source.self(window.name);
                    box.title = box.title.substring(0, box.title.lastIndexOf('-') + 1) + ' 固定试题';
                    window.setTimeout(function () {
                        //box.full = true;
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
                },
                //是否为新增
                isadd: t => t.id == null || t.id == '' || this.id == 0,
            },
            watch: {

            },
            methods: {
                //打开试卷基础信息的页面
                openpaperinfo: function (page, title, icon, height) {
                    if (!window.top.$pagebox) return;
                    //子窗口页面路径
                    var suburl = $dom.routepath() + page;
                    //当前窗口
                    var curbox = window.top.$pagebox.get(window.name);
                    //创建新窗口中
                    var subbox = window.top.$pagebox.create({
                        height: height, id: page, ico: icon, title: title,
                        url: suburl
                    });
                    curbox.opensub(subbox, 'top');
                },
                //接收子窗口数据
                //data:子窗口返回的数据
                //func:要调用的函数名称
                receive: function ([data, count, func]) {
                    if (func == 'selectpart') [this.parts, this.quescount.part] = [data, count];
                    if (func == 'selectknl') [this.knls, this.quescount.knl] = [data, count];
                    if (func == 'selecttag') [this.tags, this.quescount.tag] = [data, count];
                    //
                    this.getquestotal();
                },
                //向“更多分数设置”的窗体传递数据
                transmit: function () {
                    //试卷对象，题型
                    return [this.entity, this.types];
                },
                //接子的窗体数据
                receive: function ([entity, upfile]) {
                    this.entity.Etp_Name = entity.Etp_Name;
                    this.entity.Etp_SubName = entity.Etp_SubName;
                    this.entity.Etp_Span = entity.Etp_Span;
                    this.entity.Etp_Remind = entity.Etp_Remind;
                    this.entity.Etp_Intro = entity.Etp_Intro;
                    this.upfile = upfile;                   
                },
                btnEnter: function () { 
                },
            },
            filters: {

            },
            components: {

            }
        });
    });