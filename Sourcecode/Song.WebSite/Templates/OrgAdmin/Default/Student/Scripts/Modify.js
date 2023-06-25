﻿
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id', 0),
            activeName: 'general',      //选项卡
            //当前账号
            account: {
                Ac_IsUse: true,
                Ac_IsPass: true,
                Ac_Photo: '',
                Ac_Name: '',
                Ac_AccName: ''
            },
            sorts: [],   //学员组列表
            accsort: {},      //当前学员所在的分组
            sortpanel: false,   //学员组的选择面板
            sortquery: { 'orgid': '', 'use': true, 'search': '', 'index': 1, 'size': 10 },
            sort: {
                loading: true,
                total: 0,
                paper: 0
            },

            accPingyin: [],  //账号名称的拼音
            organ: {},       //当前登录账号所在的机构
            rules: {
                Ac_Name: [
                    { required: true, message: '姓名不得为空', trigger: 'blur' }
                ],
                Ac_AccName: [
                    { required: true, message: '账号不得为空', trigger: 'blur' },
                    { min: 6, max: 20, message: '长度在 6 到 20 个字符', trigger: 'blur' },
                    {
                        validator: async function (rule, value, callback) {
                            await window.vapp.duplicate_check(value).then(res => {
                                if (res) callback(new Error('当前账号已经存在!'));

                            });
                        }, trigger: 'blur'
                    }
                ]
            },
            defaultpw: '',      //默认密码

            //图片文件
            upfile: null, //本地上传文件的对象   

            loading: false
        },
        created: function () {
            var th = this;
            th.loading = true;
            if (th.id == '' || th.id == 0) {
                $api.put('Snowflake/Generate').then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        //th.account.Ac_ID = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    alert(err);
                    console.error(err);
                });
                return;
            }
            $api.get('Account/ForID', { 'id': th.id }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.account = result;
                    $api.get('Account/SortForID', { 'id': th.account.Sts_ID }).then(function (req) {
                        if (req.data.success) {
                            th.accsort = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        //alert(err);
                        //Vue.prototype.$alert(err);
                        console.error(err);
                    });
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            }).finally(function () {
                th.loading = false;
                th.getorgan();
                if (!th.isexist) {
                    $api.get('Account/DefaultPw').then(function (req) {
                        if (req.data.success) {
                            th.defaultpw = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        //alert(err);
                        Vue.prototype.$alert(err);
                        console.error(err);
                    });
                }
            });
        },
        computed: {
            //是否存在账号
            isexist: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null && this.id != 0;
            },
            //是否新增账号
            isadd: t => { return t.id == null || t.id == '' || this.id == 0; },
            //学员的组是否存在
            sortexist: function () {
                return JSON.stringify(this.accsort) != '{}' && this.accsort != null && !!this.accsort.Sts_ID;
            },
            //是否已经绑定手机号
            isbindmobi: function () {
                if (!this.isexist) return false;
                let tel1 = this.account.Ac_MobiTel1;
                let tel2 = this.account.Ac_MobiTel2;
                if (tel2 == null || tel2 == '') return false;
                console.log(tel1 == tel2)
                return tel1 == tel2;
            }
        },
        methods: {
            //获取机构
            getorgan: function (orgid) {
                var th = this;
                (th.isexist ?
                    $api.get('Organization/ForID', { 'id': th.account.Org_ID }) :
                    $api.get('Organization/Current'))
                    .then(function (req) {
                        th.loading = false;
                        if (req.data.success) {
                            th.organ = req.data.result;
                            th.sortquery.orgid = th.organ.Org_ID;
                            th.sortpaper(1);
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
            },
            //分页获取学员组
            sortpaper: function (index) {
                if (index != null) this.sortquery.index = index;
                var th = this;
                th.sort.loading = true;
                $api.get("Account/SortPager", th.sortquery).then(function (d) {
                    th.sort.loading = false;
                    if (d.data.success) {
                        th.sorts = d.data.result;
                        th.sort.paper = Number(d.data.totalpages);
                        th.sort.total = d.data.total;
                        //console.log(th.accounts);
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    th.sort.loading = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //选择学员组
            sortselect: function (item) {
                this.sortpanel = false;
                this.accsort = item;
                this.account.Sts_ID = item.Sts_ID;
                this.account.Sts_Name = item.Sts_Name;
            },
            //绑定手机号
            bindmobi: function (val) {
                if (val && (this.account.Ac_MobiTel1 != null && this.account.Ac_MobiTel1 != ''))
                    this.account.Ac_MobiTel2 = this.account.Ac_MobiTel1;
                if (!val) this.account.Ac_MobiTel2 = '';
                console.log(val);
                console.log(this.isbindmobi);
            },
            btnEnter: function (formName, isclose) {
                var th = this;
                this.$refs[formName].validate((valid, fields) => {
                    if (valid) {
                        th.loading = true;
                        var apipath = th.id == '' ? api = 'Account/add' : 'Account/Modify';
                        if (th.id == '') th.account.Org_ID = th.organ.Org_ID;
                        //接口参数，如果有上传文件，则增加file
                        var para = {};
                        if (th.upfile == null || JSON.stringify(th.upfile) == '{}') para = { 'acc': th.account };
                        else
                            para = { 'file': th.upfile, 'acc': th.account };
                        $api.post(apipath, para).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                window.setTimeout(function () {
                                    th.operateSuccess(isclose);
                                }, 600);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            th.loading = false;
                            th.$alert(err, '错误');
                        });
                    } else {
                        //未通过验证的字段
                        let field = Object.keys(fields)[0];
                        let label = $dom('label[for="' + field + '"]');
                        while (label.attr('tab') == null)
                            label = label.parent();
                        th.activeName = label.attr('tab');
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //名称转拼音
            pingyin: function () {
                this.accPingyin = makePy(this.account.Ac_Name);
                if (this.accPingyin.length > 0)
                    this.account.Ac_Pinyin = this.accPingyin[0];
            },
            //图片文件上传
            filechange: function (file) {
                if (!this.isexist) return;
                var th = this;
                th.loading = true;
                $api.post('Account/ModifyPhoto', { 'file': file, 'account': th.account }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.account.Ac_Photo = result.Ac_Photo;
                        th.$message({
                            type: 'success',
                            message: '上传头像成功!',
                            center: true
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //判断账号是否存在
            duplicate_check: function (val) {
                return new Promise(function (resolve, reject) {
                    $api.get('Account/IsExistAcc', { 'acc': val, 'id': vapp.account.Ac_ID }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            console.log(result);
                            return resolve(result);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        return reject(err);
                    });
                });
            },
            //操作成功
            operateSuccess: function (isclose) {
                window.top.$pagebox.source.tab(window.name, 'vapp.handleCurrentChange', isclose);
            }
        },
    });

}, ["../Scripts/hanzi2pinyin.js",
    "/Utilities/Components/education.js"]);
