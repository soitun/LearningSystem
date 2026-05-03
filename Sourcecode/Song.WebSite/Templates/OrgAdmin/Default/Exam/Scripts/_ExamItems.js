$ready([
    "../ExamTestPaper/Components/papertype.js"  //试卷类型
], function () {
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
            currpaper: {},      //当前试卷

            sbjTree: [],        //专业树
            sbjids: [],      //选择中的专业

            courses: [],
            course: {},
            couid: '',

            //paper: {},      //课程试卷
            courpapers: [],


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
                paper: [
                    {
                        validator: function (rule, value, callback) {
                            if (!vapp.ishavepaper) return callback(new Error('必须选择试卷'));
                            return callback();
                        }, trigger: 'blur'
                    }
                ],
                Exam_Span: [
                    { required: true, message: '不得为空', trigger: 'change' },
                    {
                        validator: function (rule, value, callback) {
                            if (Number(value) % 1 > 0) return callback(new Error('必须为整数'));
                            if (Number(value) <= 0) return callback(new Error('限时不能小于1'));
                            return callback();
                        }, trigger: 'blur'
                    }
                ],
                Exam_Date: [
                    { required: true, message: '不得为空', trigger: 'change' }
                ]
            },
            loadstate: {
                init: false,        //初始化
                exampaper: false,         //考试试卷加载
                subject: false,         //加载专业
                update: false,      //更新数据
                del: false          //删除数据
            }
        },
        mounted: function () {
            this.org = window.org;
            var th = this;
            this.receive().then((d) => {
                //课程试卷相关
                if (d.Tp_Id != '' && d.Tp_Id != '0') {
                    th.getSubjects().then(sbjs => {
                        th.getcourpaper(d.Tp_Id).then(paper => {
                            th.sbjids = [];
                            th.clac_sbjids(sbjs, paper.Sbj_ID);
                        });
                    });
                }
            });

            //考试专用试卷相关
            this.getexampapers();
            //课程试卷相关
            //this.getSubjects();
            if (this.exam.Tp_Id != '' && this.exam.Tp_Id != '0') {

            }
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
            },
            //是否是考试专用试卷
            isexampaper: function () {
                return this.exam.Exam_Purpose == 1 || (this.exam.Etp_Id != '' && this.exam.Etp_Id != '0');
            },
            //是否是课程试卷
            iscoursepaper: function () {
                return this.exam.Exam_Purpose == 0 || (this.exam.Tp_Id != '' && this.exam.Tp_Id != '0');
            },
        },
        watch: {
            //选择考试专用试卷时
            'exam.Etp_Id': function (val) {
                let paper = this.exampapers.find(x => x.Etp_Id == val);
                if (paper == null) return;
                this.currpaper = paper;
                this.exam.Exam_Total = paper.Etp_Total;
                this.exam.Exam_PassScore = paper.Etp_PassScore;
                this.exam.Exam_Span = paper.Etp_Span;
                this.Tp_Id = '';
            },
            //选择课程试卷时
            'exam.Tp_Id': function (val) {
                let paper = this.courpapers.find(x => x.Tp_Id == val);
                if (paper == null) return;
                this.currpaper = paper;
                this.exam.Exam_Total = paper.Tp_Total;
                this.exam.Exam_PassScore = paper.Tp_PassScore;
                this.exam.Exam_Span = paper.Tp_Span;
                this.exam.Sbj_ID = paper.Sbj_ID;
                this.exam.Sbj_Name = paper.Sbj_Name;
                this.Etp_Id = '';
            },
            //当选择试卷是课程试卷时，加载专业等信息
            'papertype': {
                handler: function (val) {
                    if (val == 0 && (this.sbjTree == null || this.sbjTree.length == 0)) {
                        this.getSubjects();
                    }
                }
            }, immediate: true,
        },
        methods: {
            //接收的主窗体数据
            receive: async function () {
                //return new Promise((resolve, reject) => {
                    //像主窗体传值，传三个值：选中的分类，选中的试题数，调用函数名
                    var pagebox = window.top.$pagebox;
                    if (pagebox && pagebox.source.top) {
                        let [exam, theme] = pagebox.source.box(window.name, 'vapp.transmit', false, this.examid);
                        this.theme = theme;     //考试主题
                        if (exam == null) {
                            //新增时，创建场次
                            this.exam = await this.createexam();
                        } else this.exam = $api.clone(exam);
                        if (this.exam.Etp_Id != '' && this.exam.Etp_Id != '0') this.papertype = 1;
                        else if (this.exam.Tp_Id != '' && this.exam.Tp_Id != '0') this.papertype = 0;
                        this.exam.Exam_Purpose = this.papertype;
                        //resolve(this.exam);
                        return this.exam;
                    }
                    return null;
                //});
            },
            //创建考试对象
            createexam: async function () {
                var exam = {
                    Exam_ID: '',
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
                try {
                    const req = await $api.get("Snowflake/Generate");
                    if (req.data.success) {
                        let snowid = req.data.result;
                        exam.Exam_ID = snowid;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                } catch (err) {
                    console.error(err);
                    throw err;
                }
                return exam;
            },
            //加载考试专用试卷
            getexampapers: function (search) {
                //试卷的查询条件
                let form = {
                    "orgid": this.org.Org_ID, "accid": "", "search": search,
                    "isdeleted": false, "diff": "", "use": true, "size": 999, "index": 1
                };
                var th = this;
                th.loadstate.exampaper = true;
                $api.get("ExamTestPaper/Pager", form).then(function (d) {
                    if (d.data.success) {
                        var result = d.data.result;
                        th.exampapers = result;
                        //th.totalpages = Number(d.data.totalpages);
                        //th.total = d.data.total;
                    } else throw d.data.message;
                }).catch(err => alert(err))
                    .finally(() => th.loadstate.exampaper = false);
            },

            /**
             * 选择课程试卷相关
             */
            //获取专业
            getSubjects: function () {
                var th = this;
                var form = { orgid: th.org.Org_ID, search: '', isuse: null, delete: false };
                var buildtree = function (data, pid) {
                    var list = data.filter(item => item.Sbj_PID == pid);
                    list.forEach(item => {
                        item['children'] = buildtree(data, item.Sbj_ID);
                    });
                    return list.length ? list : null;
                };
                th.loadstate.subject = true;
                return new Promise(function (resolve, reject) {
                    $api.get('Subject/list', form).then(function (req) {
                        if (req.data.success) {
                            let result = req.data.result;
                            th.sbjTree = buildtree(result, 0);
                            resolve(result);
                        } else {
                            throw req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => th.loadstate.subject = false);
                });
            },

            //专业选择变更时
            sbjChange: function (val) {
                //关闭级联菜单的浮动层
                if (this.$refs["subjects"]) this.$refs["subjects"].dropDownVisible = false;
                var currid = -1;
                if (val.length > 0) currid = val[val.length - 1];
                var th = this;
                var orgid = th.org.Org_ID;
                $api.cache('Course/Pager', { 'orgid': orgid, 'sbjids': currid, 'thid': '', 'use': '','del':false, 'live': '', 'free': '', 'search': '', 'order': '', 'size': -1, 'index': 1 })
                    .then(function (req) {
                        if (req.data.success) {
                            th.courses = req.data.result;
                            if (th.courses.length > 0) {
                                let course = th.courses.find(item => item.Cou_ID == th.currpaper.Cou_ID);
                                if (course) th.couid = course.Cou_ID;
                                else th.couid = th.courses[0].Cou_ID;
                                th.courChange(th.couid);
                            }
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(err => console.error(err));
            },
            //课程选择变更时
            courChange: function (val) {
                var th = this;
                $api.get('TestPaper/ShowPager', { 'couid': val, 'search': '', 'diff': '', 'size': 99999, 'index': 1 }).then(function (req) {
                    if (req.data.success) {
                        th.courpapers = req.data.result;
                        if (th.exam.Tp_Id <= 0 || th.exam.Tp_Id == '' || th.exam.Tp_Id == '0') {
                            if (th.courpapers.length > 0) {
                                th.exam.Tp_Id = th.courpapers[0].Tp_Id;
                            }
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err));
            },
            //计算当前选中的专业
            clac_sbjids: function (list, sbjid) {
                var subject = list.find(function (item) {
                    return item.Sbj_ID == sbjid;
                });
                if (subject != null) {
                    this.sbjids.splice(0, 0, sbjid);
                    this.clac_sbjids(list, subject.Sbj_PID);
                }
            },
            //获取课程试卷
            getcourpaper: function (tpid) {
                var th = this;
                return new Promise((resolve, reject) => {
                    $api.get("TestPaper/ForID", { "id": tpid })
                        .then(req => {
                            if (req.data.success) {
                                th.currpaper = req.data.result;
                                if (th.currpaper == null) return;
                                resolve(th.currpaper);
                                //
                                th.couid = th.currpaper.Cou_ID;
                                th.sbjChange([th.currpaper.Sbj_ID]);
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(err => console.error(err)).finally(() => { });
                });
            },

            //确定           
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid, fields) => {
                    if (valid) {
                        //试卷类型，0为课程试卷，1为考试试卷
                        th.exam.Exam_Purpose = th.papertype;
                        //题量
                        th.exam.Exam_QuesCount = th.iscoursepaper ?
                            th.currpaper.Tp_Count : th.currpaper.Etp_Count;
                        //像主窗体传值，当前实体，图片对象
                        var pagebox = window.top.$pagebox;
                        if (pagebox && pagebox.source.box) {
                            pagebox.source.box(window.name, 'vapp.receive', false, [this.exam]);
                            let curbox = pagebox.source.self(window.name);
                            curbox.shut();
                        }
                    } else {
                        console.error('error submit!!');
                        return false;
                    }
                });
            },
        },
        filters: {

        },
        components: {

        }
    });
});