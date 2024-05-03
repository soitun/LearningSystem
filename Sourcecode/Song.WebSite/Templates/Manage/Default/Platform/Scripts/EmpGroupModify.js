﻿
$ready(function () {

    window.vapp = new Vue({
        el: '#app',
        data: {
            loading: false,  //
            id: $api.querystring('id'),
            entity: {}, //当前数据对象
            organ: {},           //当前登录账号所在的机构
            rules: {
                EGrp_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    { min: 1, max: 100, message: '长度在 1 到 100 个字符', trigger: 'blur' },
                    {
                        validator: async function (rule, value, callback) {
                            await vapp.isExist(value).then(res => {
                                if (res) callback(new Error('已经存在!'));
                            });
                        }, trigger: 'blur'
                    }
                ]
            }
        },
        watch: {
            'loading': function (val, old) {
                console.log('loading:' + val);
            }
        },
        created: function () {
            var th = this;
            //如果是新增界面
            if (this.id == '') {
                this.entity.EGrp_IsUse = true;
                $api.get('Admin/Organ').then(function (req) {
                    if (req.data.success) {
                        th.organ = req.data.result;
                        th.entity.Org_ID = th.organ.Org_ID;

                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
                return;
            }
            //如果是修改界面
            $api.get('Admin/GroupForID', { 'id': this.id }).then(function (req) {
                if (req.data.success) {
                    th.entity = req.data.result;
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
            });
        },
        methods: {
            //判断是否已经存在
            isExist: function (val) {
                var th = this;
                return new Promise(function (resolve, reject) {
                    $api.get('admin/GroupExist', { 'name': val, 'id': th.id, 'orgid': th.entity.Org_ID }).then(function (req) {
                        if (req.data.success) {
                            return resolve(req.data.result);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        return reject(err);
                    });
                });
            },
            btnEnter: function (formName) {
                if (this.loading) return;
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var apiurl = this.id == '' ? "Admin/GroupAdd" : 'Admin/GroupModify';
                        $api.post(apiurl, { 'entity': th.entity }).then(function (req) {
                            if (req.data.success) {
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                th.operateSuccess();
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err, '错误');
                        }).finally(() => th.loading = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vapp.loadDatas', true);
            }
        },
    });

});
