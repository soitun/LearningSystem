
$ready(["Components/group_select.js",
    "Components/accountselect.js",
    "../ExamTestPaper/Components/papertype.js"  //试卷类型
], function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id', 0),
            org: {},
            tabs: [
                { title: '基本信息', name: 'general', icon: 'e6b0' },
                { title: '参考人员', name: 'range', icon: 'e67d' }],
            activeName: 'range',     //选项卡

            //当前数据实体
            entity: {
                Exam_ID: 0,
                Exam_IsTheme: true,
                Exam_DateType: 1,
                Exam_IsRightClick: true,
                Exam_IsUse: true,
                Exam_GroupType: 1,
                Exam_Date: new Date(),
                Exam_DateOver: new Date(new Date().getTime() + 30 * 24 * 60 * 60 * 1000),
                Exam_UID: new Date().getTime()
            },
            exams: [],          //考试场次
            examgroups: [],     //考试分组
            //考试表单验证
            rules: {
                Exam_Title: [
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
                ]
            },
            //参考的学员数量
            studenttotal: 0,

            loadstate: {
                init: false,        //初始化            
                get: false,         //加载数据
                update: false,      //更新数据              
            },
        },
        watch: {
            //当考试时间方式更改时
            'entity.Exam_DateType': function (nv, ov) {
                if (nv == 2) {
                    var date = this.entity.Exam_Date;
                    var over = this.entity.Exam_DateOver;
                    if (date.getFullYear() - 100 > new Date().getFullYear() || date.getFullYear() + 100 < new Date().getFullYear())
                        date = new Date();
                    if (over.getFullYear() - 100 > new Date().getFullYear() || over.getFullYear() + 100 < new Date().getFullYear())
                        over = date.setMonth(date.getMonth() + 1);
                    this.entity.Exam_Date = date;
                    this.entity.Exam_DateOver = over;
                }
                //console.log(nv);
            },
            //当学员范围变化时
            'entity.Exam_GroupType': function (nv, ov) {
                this.getaccounttotal();
            }
        },
        created: function () {
            var th = this;
            th.org = window.org;
            th.getTheme();


        },
        mounted: function () {

        },
        computed: {
            //是否新增账号
            isadd: t => t.id == null || t.id == '' || this.id == 0,
            //是否加载中
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
        methods: {
            //获取考试主题
            getTheme: function () {
                var th = this;
                if (th.id == 0) return;
                th.loadstate.get = true;
                $api.get('Exam/ForID', { 'id': th.id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.entity = result;
                        $api.get('Exam/exams', { 'uid': th.entity.Exam_UID }).then(function (req) {
                            if (req.data.success) {
                                th.exams = req.data.result;
                                //场次拖拽
                                th.$nextTick(() => th.rowdrop());
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(err => console.error(err));
                        th.getaccounttotal();
                    } else {
                        throw '未查询到数据';
                    }
                }).catch(err => alert(err)).finally(() => th.loadstate.get = false);
            },
            /** 
             * 考试场次的管理
             * */
            //场次的拖动
            rowdrop: function () {
                // 首先获取需要拖拽的dom节点            
                const el1 = document.querySelectorAll('div.examscard .el-card__body')[0];
                if (el1 == null) return;
                Sortable.create(el1, {
                    disabled: false, // 是否开启拖拽
                    ghostClass: 'sortable-ghost', //拖拽样式
                    handle: '.draghandle',     //拖拽的操作元素
                    animation: 150, // 拖拽延时，效果更好看
                    group: { pull: false, put: false },
                    onEnd: (e) => {
                        let examdom = $dom("section.exams");
                        var th = this;
                        examdom.each(function (index, element) {
                            let examid = $dom(this).attr('examid');
                            let exam = th.exams.find(el => String(el.Exam_ID) == examid);
                            th.$set(exam, 'Exam_Order', index + 1);
                        });
                    }
                });
            },
            //打开选择试题的子窗体
            openitems: function (examid) {
                if (!window.top.$pagebox) return;
                //let item = this.qtypeitems.find(el => el.type == type);
                let page = '_ExamItems';
                let suburl = $dom.routepath() + page;    //子窗口页面路径      
                suburl = $api.url.set(suburl, { 'examid': examid });
                var curbox = window.top.$pagebox.get(window.name);   //当前窗口
                //创建新窗口中
                var subbox = window.top.$pagebox.create({
                    width: 500, id: page, ico: 'e834', title: '考试场次',
                    url: suburl
                });
                curbox.opensub(subbox, 'right');
            },
            //向“考试场次”的窗体传递数据
            transmit: function (examid) {
                if (examid == null) return [null, this.entity];
                let exam = this.exams.find(el => Number(el.Exam_ID) == Number(examid));
                //考试场次
                return [exam, this.entity];
            },
            //接收子窗体（考试场次）的数据
            receive: function ([exam]) {
                let index = this.exams.findIndex(el => el.Exam_ID == exam.Exam_ID);
                if (index < 0) this.exams.push(exam);
                else this.$set(this.exams, index, exam);
                for (var i = 0; i < this.exams.length; i++) {
                    this.exams[i].Exam_Order = i + 1;
                }
            },
            /** 
            * 参考人员的管理
            * */
            //参考人员的学员组变更时
            //参数说明
            //stsid: 学员组ID的数组
            //sorts: 学员组对象数组
            //relationships: 学员组与考试的关系对象数组，ExamGroup对象
            groupselected: function (stsid, sorts, relationships) {
                if (stsid == null) stsid = [];
                this.examgroups = relationships;
                this.getaccounttotal();
            },
            //获取参考学员的总数
            getaccounttotal: function () {
                var api = null;
                var stsid = [];
                let groups = this.examgroups;
                for (var i = 0; groups != null && i < groups.length; i++) {
                    stsid.push(groups[i].Sts_ID);
                }
                if (this.entity.Exam_GroupType == 1) api = $api.get('Account/Total', { "orgid": this.org.Org_ID });
                else if (this.entity.Exam_GroupType == 2) api = $api.get('Account/TotalOfSort', { "sts": stsid.join(',') });
                else if (this.entity.Exam_GroupType == 3) api = $api.get('Account/TotalOfSort', { "sts": stsid.join(',') });
                var th = this;
                api.then(req => {
                    if (req.data.success) {
                        th.studenttotal = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err)).finally(() => { });
            },
            //保存
            btnEnter: function (formName, isclose) {
                var th = this;
                if (th.loadstate.update) return;
                //考试场次
                var exams = th.exams;
                for (var i = 0; i < exams.length; i++)
                    if (exams[i].Exam_Order <= 0) exams[i].Exam_Order = i + 1;
                //
                this.$refs[formName].validate((valid, fields) => {
                    if (valid) {
                        th.loadstate.update = true;
                        var apipath = th.id == 0 ? 'Exam/add' : 'Exam/Modify';
                        $api.post(apipath, { 'theme': th.entity, 'items': exams, 'groups': th.examgroups }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$notify({
                                    message: '考试管理操作成功！',
                                    type: 'success',
                                    position: 'bottom-left'
                                });
                                th.operateSuccess(isclose);
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        }).finally(() => th.loadstate.update = false);
                    } else {
                        th.$nextTick(() => {
                            console.error('录入验证失败');
                            let err = $dom('.el-form-item.is-error').first();
                            if (err && err.length > 0) {
                                while (err.attr('tab') == null) err = err.parent();
                                this.activeName = err.attr('tab');
                            }
                        });
                        return false;
                    }
                });
            },
            //操作成功
            operateSuccess: function (isclose) {
                //当处理教师管理状态时
                var pagebox = window.top.$pagebox;
                var box = pagebox?.get(window.name);
                if (!box) return;
                var pid = box.pid;
                if (window.top.vapp && window.top.vapp.fresh) {
                    window.top.vapp.fresh(pid, 'vapp.handleCurrentChange');
                    window.setTimeout(function () {
                        pagebox.shut(window.name);
                    }, 1000);
                }
                else {
                    //当处于机构管理界面时
                    window.top.$pagebox.source.tab(window.name, 'vapp.handleCurrentChange', isclose);
                }
            }
        },
    });

});
