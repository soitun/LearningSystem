$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'), //主键ID
            org: {},          //当前机构对象
            config: {},      //当前机构配置项  
            tabs: [
                { name: '基本信息', tab: 'general', icon: 'e72f' },
                { name: '出题范围', tab: 'range', icon: 'e731' },
                { name: '分数', tab: 'score', icon: 'e731' },
                { name: '注意事项', tab: 'remind', icon: 'e697' },
                { name: '其它', tab: 'other', icon: 'e67e' }
            ],
            activeName: 'general',
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
            //图片文件
            upfile: null, //本地上传文件的对象         
            Etp_Diff: [1, 5],     //难度范围
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