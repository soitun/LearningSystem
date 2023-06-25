$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id', 0),
            organ: {},
            config: {},      //当前机构配置项      
            titles: [],          //教师职称  
            //当前对象的实体
            entity: {
                Th_IsUse: true,
                Th_Photo: ''
            },
            activeName: 'general',      //选项卡
            accPingyin: [],  //账号名称的拼音
            rules: {
                Th_Name: [
                    { required: true, message: '姓名不得为空', trigger: 'blur' }
                ],
                Th_AccName: [
                    { required: true, message: '账号不得为空', trigger: 'blur' },
                    { min: 6, max: 20, message: '长度在 6 到 20 个字符', trigger: 'blur' }
                ]
            },
            //图片文件
            upfile: null, //本地上传文件的对象   

            loading: false,
            loading_init: true
        },
        mounted: function () {
            this.getEntity();
        },
        created: function () {

        },
        computed: {
            //是否存在账号
            isexist: function () {
                return JSON.stringify(this.entity) != '{}' && this.entity != null && this.id != 0;
            },
        },
        watch: {
            'organ': function (n, o) {
                var th = this;
                $api.get('Teacher/Titles', { 'orgid': th.organ.Org_ID, 'name': '', 'use': true }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.titles = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                });
            }
        },
        methods: {
            //获取当前对象
            getEntity: function () {
                var th = this;
                th.loading = true;
                if (th.id != "") {
                    $api.get('Teacher/ForID', { 'id': th.id }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.entity = result;
                            if (th.entity.Ths_ID <= 0) th.entity.Ths_ID = '';
                            $api.get('Organization/ForID', { 'id': th.entity.Org_ID }).then(function (req) {
                                th.loading = false;
                                if (req.data.success) {
                                    vapp.organ = req.data.result;
                                    vapp.config = $api.organ(vapp.organ).config;
                                } else {
                                    console.error(req.data.exception);
                                    throw req.data.message;
                                }
                            }).catch(function (err) {
                                Vue.prototype.$alert(err);
                                console.error(err);
                            });
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        Vue.prototype.$alert(err);
                        console.error(err);
                    });
                } else {
                    $api.bat(
                        $api.put('Snowflake/Generate'),
                        $api.get('Organization/Current')
                    ).then(axios.spread(function (snowid, org) {
                        //获取结果
                        //th.entity.Th_ID = snowid.data.result;
                        th.organ = org.data.result;
                        th.config = $api.organ(th.organ).config;
                    })).catch(function (err) {
                        Vue.prototype.$alert(err);
                        console.error(err);
                    });
                }
            },
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid, fields) => {
                    if (valid) {
                        th.loading = true;
                        var apipath = th.id == '' ? api = 'Teacher/add' : 'Teacher/Modify';
                        if (th.id == '') th.entity.Org_ID = th.organ.Org_ID;
                        //接口参数，如果有上传文件，则增加file
                        var para = {};
                        if (th.upfile == null || JSON.stringify(th.upfile) == '{}') para = { 'entity': th.entity };
                        else
                            para = { 'file': th.upfile, 'entity': th.entity };
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
                                    th.operateSuccess();
                                }, 600);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            //window.top.ELEMENT.MessageBox(err, '错误');
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
                this.accPingyin = makePy(this.entity.Th_Name);
                if (this.accPingyin.length > 0 && this.entity.Th_Pinyin == '')
                    this.entity.Th_Pinyin = this.accPingyin[0];
                //console.log(this.accPingyin);
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vue.handleCurrentChange', true);
            }
        }
    });

}, ["../Scripts/hanzi2pinyin.js",
    "/Utilities/Components/education.js"]);
