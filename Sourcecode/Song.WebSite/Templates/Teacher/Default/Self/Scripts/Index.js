$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},      //当前机构配置项   
            tabs: [
                { title: '基本信息', name: 'general', icon: 'e687' },
                { title: '联系方式', name: 'contact', icon: 'e66c' },
                { title: '详细信息', name: 'detail', icon: 'e669' },
                { title: '个人介绍', name: 'intro', icon: 'e6b0' }],
            activeName: 'general',     //选项卡

            titles: [],          //教师职称  
            //当前对象的实体
            entity: {
                Th_IsUse: true,
                Th_Photo: ''
            },
            accPingyin: [],  //账号名称的拼音
            rules: {
                Th_Name: [
                    { required: true, message: '姓名不得为空', trigger: 'blur' }
                ],
                Th_AccName: [
                    { required: true, message: '账号不得为空', trigger: 'blur' },
                    { min: 4, max: 20, message: '长度在 4 到 20 个字符', trigger: 'blur' }
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
            //Vue.prototype.$alert("为中");
        },
        computed: {

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

                $api.get('Teacher/Current').then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.entity = result;
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
            },
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var apipath = 'Teacher/Modify';                  
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
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            //window.top.ELEMENT.MessageBox(err, '错误');
                            alert(err);
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //名称转拼音
            pingyin: function () {
                this.accPingyin = makePy(this.entity.Th_Name);
                if (this.accPingyin.length > 0)
                    this.entity.Th_Pinyin = this.accPingyin[0];
                //console.log(this.accPingyin);
            }
        }
    });

}, ["/Utilities/Scripts/hanzi2pinyin.js",
    "/Utilities/Components/education.js"]);
