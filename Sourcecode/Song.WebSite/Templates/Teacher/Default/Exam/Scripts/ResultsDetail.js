﻿
$ready(function () {

    window.vapp = new Vue({
        el: '#app',
        data: {
            form: {
                examid: $api.querystring('id'),
                name: '', idcard: '', stsid: 0,
                min: -1, max: -1, manual: null,
                size: 20, index: 1
            },
            entity: {}, //当前考试对象
            sorts: {},       //当前场次的学员组（根据参考学员判断）
            results: [],     //成绩
            account: {},      //当前学员信息
            current: {},     //当前行对象
            accountVisible: false,   //是否显示当前学员
            scorerange: '成绩',     //成绩范围选择的提示信息
            //
            exportVisible: false,    //成绩导出信息是否显示
            files: [],               //导出的文件列表
            fileloading: false,      //导出时的加载状态
            loading: false,
            loadingid: 0,
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [] //数据表中选中的行
        },
        computed: {
        },
        watch: {

        },
        mounted: function () {
            this.$refs['btngroup'].addbtn({
                text: '清空', tips: '清空当前考试下的所有成绩',
                id: 'clear', type: 'warning',
                icon: 'e839'
            });
        },
        created: function () {
            //获取考试信息
            var th = this;
            $api.get('Exam/ForID', { 'id': this.form.examid }).then(function (req) {
                if (req.data.success) {
                    th.entity = req.data.result;
                    th.form.max = th.entity.Exam_Total;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                //alert(err);
                console.error(err);
            });
            this.getsorts();
            this.handleCurrentChange();
            this.getFiles();
        },
        methods: {
            //获取学员分组
            getsorts: function () {
                var th = this;
                $api.get('Exam/Sort4Exam', { 'examid': this.form.examid }).then(function (req) {
                    if (req.data.success) {
                        th.sorts = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => { });
            },
            //下拉菜单事件
            dorphandle: function (command) {
                switch (Number(command)) {
                    //全部
                    case -1:
                        this.form.min = -1;
                        this.form.max = -1;
                        this.scorerange = '成绩';
                        break;
                    //优秀
                    case 1:
                        this.form.min = Math.floor(this.entity.Exam_Total * 0.8);
                        this.form.max = this.entity.Exam_Total;
                        this.scorerange = '优秀（' + this.form.min + '分以上)';
                        break;
                    //及格
                    case 2:
                        this.form.min = this.entity.Exam_PassScore;
                        this.form.max = this.entity.Exam_Total;
                        this.scorerange = '及格（' + this.form.min + '分以上)';
                        break;
                    //不及格
                    case 3:
                        this.form.min = 0;
                        this.form.max = this.entity.Exam_PassScore - 0.01;
                        this.scorerange = '不及格（' + this.entity.Exam_PassScore + '分以下)';
                        break;
                    //零分
                    case 4:
                        this.form.min = 0;
                        this.form.max = 0;
                        this.scorerange = '零分';
                        break;
                }
                if (Number(command) <= 4)
                    this.handleCurrentChange(1);
            },
            //加载数据集
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                console.log('最小值' + th.form.min);
                console.log('最大值' + th.form.max);
                this.loading = true;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 105;
                th.form.size = Math.floor(area / 42);
                $api.get("Exam/Result4Exam", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        th.results = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            },
            //计算考试用时
            calcSpan: function (d1, d2) {
                if (d1 == null || d2 == null) return '';
                var total = (d2.getTime() - d1.getTime()) / 1000;
                var span = Math.floor(total / 60);
                return span <= 0 ? "<1" : span;
            },
            //计算考试成绩
            clacScore: function (exrid) {
                var th = this;
                th.loadingid = exrid;
                $api.get('Exam/ClacScore', { 'exrid': exrid }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.loadingid = 0;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loadingid = 0;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                $api.delete('Exam/ResultDelete', { 'exrid': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            },
            //清空
            clear: function () {
                this.$confirm('删除所有成绩，是否继续？', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    this.$confirm('此操作将永久删除所有成绩，且无法恢复, 是否继续？', '再次提示', {
                        confirmButtonText: '确定',
                        cancelButtonText: '取消',
                        type: 'error'
                    }).then(() => {
                        var th = this;
                        var loading = this.$fulloading();
                        $api.delete('Exam/ResultClear', { 'examid': this.form.examid }).then(function (req) {
                            if (req.data.success) {
                                th.$message({
                                    type: 'success',
                                    message: '删除成功!'
                                });
                                loading.close();
                                th.handleCurrentChange();
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        });

                    }).catch(() => { });
                }).catch(() => { });
            },
            //当查看学员信息时，获取当前学员信息
            getaccount: function (row) {
                this.accountVisible = true;
                this.current = row;
                $api.get('Account/ForID', { 'id': row.Ac_ID }).then(function (req) {
                    if (req.data.success) {
                        vapp.account = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            },

            //已经导出的文件列表
            getFiles: function () {
                var th = this;
                $api.get('Exam/ExcelFiles', { 'examid': this.form.examid }).then(function (req) {
                    th.fileloading = false;
                    if (req.data.success) {
                        th.files = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            },
            //生成导出文件
            toexcel: function () {
                var th = this;
                th.fileloading = true;
                $api.post('Exam/ExcelOutput', { 'examid': this.form.examid }).then(function (req) {
                    if (req.data.success) {
                        th.getFiles();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => alert(err))
                    .finally(() => th.fileloading = false);
            },
            //删除文件
            deleteFile: function (file) {
                var th = this;
                this.fileloading = true;
                $api.get('Exam/ExcelDelete', { 'examid': this.form.examid, 'filename': file }).then(function (req) {
                    th.fileloading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.getFiles();
                        th.$notify({
                            message: '文件删除成功！',
                            type: 'success',
                            position: 'bottom-right',
                            duration: 2000
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => alert(err))
                    .finally(() => th.fileloading = false);
            },
            //打开考试回顾的窗口
            review: function (row) {
                var boxid = "ResultsReview_" + row.Exr_ID + "_" + row.Exam_ID;
                console.log(row);
                var url = $api.url.set("/student/exam/review", {
                    "examid": row.Exam_ID,
                    "exrid": row.Exr_ID
                });
                //创建
                if (!window.top.$pagebox) {
                    alert('弹窗对象不存在');
                    return;
                }
                var box = window.top.$pagebox.create({
                    width: '80%', height: '80%',
                    resize: true, id: boxid,
                    pid: window.name,
                    url: url,
                    id: boxid,
                    'showmask': true, 'min': false, 'ico': 'e696'
                });
                box.title = row.Ac_Name + '在“' + this.entity.Exam_Name + "”中的成绩回顾";
                box.open();
            }
        },
        components: {
            //得分的输出，为了小数点对齐
            'score': {
                props: ['number'],
                data: function () {
                    return {
                        prev: '',
                        dot: '.',
                        after: ''
                    }
                },
                created: function () {
                    var num = String(Math.round(this.number * 100) / 100);
                    if (num.indexOf('.') > -1) {
                        this.prev = num.substring(0, num.indexOf('.'));
                        this.after = num.substring(num.indexOf('.') + 1);
                    } else {
                        this.prev = num;
                        this.dot = '&nbsp;';
                    }
                },
                template: `<div class="score">
                <span class="prev">{{prev}}</span>
                <span class="dot" v-html="dot"></span>
                <span class="after">{{after}}</span>
                </div>`
            }
        }
    });

});
