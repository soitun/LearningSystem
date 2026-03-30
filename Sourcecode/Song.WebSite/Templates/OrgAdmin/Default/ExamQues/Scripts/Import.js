$ready(['/Utilities/Components/upload-excel.js',
    '../Question/Components/ques_type.js',
    'Components/selectpart.js',
    'Components/selectknl.js',],
    function () {
        window.vapp = new Vue({
            el: '#vapp',
            data: {
                org: {},
                config: {},      //当前机构配置项     
                types: [],        //试题类型，来自web.config中配置项

                //主要的参数
                form: { 'type': 1, 'parts': [], 'knls': [] },
                maintstep: 0,    //主步骤
                step: 0,         //步数            


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
                    if (this.form.type <= 0 || this.form.type > this.types.length) return '';
                    return this.types[this.form.type - 1];
                },

            },
            watch: {

            },
            methods: {
                updatepart: function (parts, partid) {
                    this.form.parts = parts;
                    console.error(parts);
                    console.error(partid);
                },
                updateknl: function (knl, knlid) {
                    this.form.knls = knl;
                    console.error(knl);
                    console.error(knlid);
                },
                //完成导入的事件
                finish: function (count) {
                    console.log(count);
                },
            }
        });

    });
