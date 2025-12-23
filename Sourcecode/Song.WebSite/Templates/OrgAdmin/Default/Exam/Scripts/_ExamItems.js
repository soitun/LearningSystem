$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            examid: $api.querystring('examid'),
            exam: {},       //当前场次
            theme: {},             //考试主题
            org: {},     //当前机构
            //专业树形下拉选择器的配置项
            defaultSubjectProps: {
                children: 'children',
                label: 'Sbj_Name',
                value: 'Sbj_ID',
                expandTrigger: 'hover',
                checkStrictly: true
            },
            papertype: 1,          //试卷类型,默认是考试专用试卷
            //考试专用试卷
            exampapers: [],

            sbjTree: [],        //专业树
            sbjids: [],      //选择中的专业

            courses: [],
            course: {},
            couid: '',

            paper: {},
            papers: [],
            paperid: '',

            rules: {
                Exam_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    { validator: validate.name.proh, trigger: 'change' },   //禁止使用特殊字符
                    { validator: validate.name.danger, trigger: 'change' },
                    { min: 3, max: 255, message: '长度在 3 到 255 个字符', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            let v = $api.trim(value);
                            if (v == '' || v.length < 1) return callback(new Error('不能全部是空格'));
                            return callback();
                        }, trigger: 'blur'
                    }
                ],
                Tp_Id: [
                    { required: true, message: '不得为空', trigger: 'change' }
                ],
                Exam_Date: [
                    { required: true, message: '不得为空', trigger: 'change' }
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
            this.org = window.org;

            this.receive().then((d) => {
                console.error(d);
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
            },
            //是否有试卷，包括课程试卷与考试试卷
            ishavepaper: function () {
                return (this.exam.Tp_Id != '' && this.exam.Tp_Id != '0')
                    || (this.exam.Etp_Id != '' && this.exam.Etp_Id != '0');
            }
        },
        watch: {

        },
        methods: {
            //接收的主窗体数据
            receive: function () {
                return new Promise((resolve, reject) => {
                    //像主窗体传值，传三个值：选中的分类，选中的试题数，调用函数名
                    var pagebox = window.top.$pagebox;
                    if (pagebox && pagebox.source.top) {
                        let [exam, theme] = pagebox.source.box(window.name, 'vapp.transmit', false, this.examid);
                        this.theme = theme;     //考试主题
                        if (exam == null) {
                            //新增时，创建场次
                            this.exam = {
                                Exam_ID: 0,
                                Exam_Name: '',
                                Tp_Id: '', Etp_Id: '',        //试卷id
                                Cou_ID: '',      //临时字段，数据实体中并不存在
                                Exam_DateType: this.theme.Exam_DateType,
                                Exam_Date: this.theme.Exam_Date,
                                Exam_DateOver: this.theme.Exam_DateOver,
                                Exam_GroupType: this.theme.Exam_GroupType,
                                Exam_UID: this.theme.Exam_UID,
                                Exam_Span: 0
                            };
                        } else this.exam = $api.clone(exam);
                        if (this.exam.Etp_Id != '' && this.exam.Etp_Id != '0') this.papertype = 1;
                        else if (this.exam.Tp_Id != '' && this.exam.Tp_Id != '0') this.papertype = 0;
                        resolve(this.exam);
                    }
                });
            },
            //加载考试专用试卷
            getexampapers: function () {
                //试卷的查询条件
                let form = {
                    "orgid": this.org.Org_ID, "accid": "", "search": "",
                    "isdeleted": false, "diff": "", "use": "", "size": 10, "index": 1
                };

                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 64);
                th.loading = true;

                $api.get("ExamTestPaper/Pager", th.form).then(function (d) {
                    if (d.data.success) {
                        var result = d.data.result;
                        th.datas = result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => {
                    th.loading = false;

                });
            },
            //专业选择变更时
            sbjChange: function (val) {
                //关闭级联菜单的浮动层
                if (this.$refs["subjects"])
                    this.$refs["subjects"].dropDownVisible = false;
                var currid = -1;
                if (val.length > 0) currid = val[val.length - 1];
                var th = this;
                var orgid = th.org.Org_ID;
                $api.cache('Course/Pager', { 'orgid': orgid, 'sbjids': currid, 'thid': '', 'use': '', 'live': '', 'free': '', 'search': '', 'order': '', 'size': -1, 'index': 1 })
                    .then(function (req) {
                        if (req.data.success) {
                            th.courses = req.data.result;
                            if (th.courses.length > 0) {
                                th.couid = th.courses[0].Cou_ID;
                                th.courChange(th.couid);
                            }
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        //alert(err);
                        Vue.prototype.$alert(err);
                        console.error(err);
                    });
            },
            //课程选择变更时
            courChange: function (val) {
                var th = this;
                $api.get('TestPaper/ShowPager', { 'couid': val, 'search': '', 'diff': '', 'size': 99999, 'index': 1 }).then(function (req) {
                    if (req.data.success) {
                        th.papers = req.data.result;
                        if (th.paperid <= 0 || th.paperid == '') {
                            if (th.papers.length > 0) {
                                th.paperid = th.papers[0].Tp_Id;
                                th.paperChange(th.paperid);
                            }
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //试卷选择变更时
            paperChange: function (val) {
                this.exam.Tp_Id = val;
                var paper = this.papers.find(function (item) {
                    return item.Tp_Id == val;
                })
                if (paper != null) {
                    this.exam.Sbj_ID = paper.Sbj_ID;
                    this.exam.Sbj_Name = paper.Sbj_Name;
                    this.exam.Exam_Total = paper.Tp_Total;
                    this.exam.Exam_PassScore = paper.Tp_PassScore;
                    this.exam.Exam_Span = paper.Tp_Span;
                }
                console.log(val);
            },
        },
        filters: {

        },
        components: {

        }
    });
});