$ready(['/Utilities/Components/upload-excel.js',
    '../Question/Components/ques_type.js'],
    function () {
        window.vapp = new Vue({
            el: '#vapp',
            data: {
                org: {},
                config: {},      //当前机构配置项     
                types: [],        //试题类型，来自web.config中配置项


                form: { 'Cou_ID': '' },       //查询课程的条件
                rules: {

                },

                step: 0,         //步数
                qtype: 0,       //当前题型       

                loading_init: false,
                loading: false
            },
            mounted: function () {
                var th = this;
                this.org = window.org;
                this.config = window.config;
                $api.cache('Question/Types:99999').then(req => th.types = req.data.result);
            },
            created: function () {

            },
            computed: {
                //试题类型的名称
                'tname': function () {
                    if (this.qtype <= 0 || this.qtype > this.types.length) return '';
                    return this.types[this.qtype - 1];
                },

            },
            watch: {

            },
            methods: {
                //选择试题类型
                selectType: function (type) {
                    //没有选中课程
                    if (!this.selectedCourse) {
                        this.$refs['form'].validate(function (valid) {
                            if (valid) {
                                console.log(3);
                            } else {
                                console.log('error submit!!');
                                return false;
                            }
                        });
                        return;
                    }
                    this.qtype = type;
                },
                //完成导入的事件
                finish: function (count) {
                    console.log(count);
                },

            }
        });

    });
