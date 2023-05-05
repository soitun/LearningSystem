
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            organ: {},       //当前机构
            config: {},
            //当前数据实体
            entity: {
                Ths_IsUse: true,
                Ths_IsDefault: false,
                Org_ID: 0
            },
            rules: {
                Ths_Name: [{ required: true, message: '名称不得为空', trigger: 'blur' }]
            },

            loading_init: false,
            loading: false
        },
        created: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Organization/Current')
            ).then(axios.spread(function (organ) {
                th.loading_init = false;
                //获取结果             
                th.organ = organ.data.result;
                if (th.id == "") th.entity.Org_ID = th.organ.Org_ID;
                //机构配置信息
                th.config = $api.organ(vapp.organ).config;
                th.getEntity();
            })).catch(function (err) {
                console.error(err);
            });            

        },
        methods: {
            //获取实体
            getEntity: function () {
                if (this.id == '') return;
                var th = this;
                th.loading = true;
                $api.get('Teacher/TitleForID', { 'id': th.id }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.entity = result;
                    } else {
                        throw '未查底到数据';
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                });
            },
            //保存信息
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var apipath = 'Teacher/Title' + (th.id == '' ? 'add' : 'Modify');
                        $api.post(apipath, { 'entity': th.entity }).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                vapp.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                window.setTimeout(function () {
                                    vapp.operateSuccess();
                                }, 600);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            vapp.$alert(err, '错误');
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vapp.getdatalist', true);
            }
        },
    });

});
