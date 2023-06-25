﻿
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            organ: {},
            activeName: 'general',      //选项卡
            subjects: [],     //所有专业数据
            //当前要操作的对象
            entity: {
                Sbj_IsUse: true,
                Sbj_IsRec: false,
                Sbj_PID: '',
                Sbj_Logo: '',
                Sbj_LogoSmall: ''
            },
            //专业树形下拉选择器的配置项
            defaultProps: {
                children: 'children',
                label: 'Sbj_Name',
                value: 'Sbj_ID',
                disabled: 'Sbj_IsUse',
                expandTrigger: 'hover',
                checkStrictly: true
            },
            sbjSelects: [],      //选择中的专业项
            //图片文件
            upfile: null, //本地上传文件的对象  

            rules: {
                Sbj_Name: [{ required: true, message: '专业不得为空', trigger: 'blur' }]
            },
            loading: false,
            loading_init: true,      //初始化预载
            num: 0
        },
        computed: {
            //有没有图片
            'islogo': function () {
                var et = this.entity;
                var etlogo = JSON.stringify(et) != '{}' && et != null && et['Sbj_Logo'];
                return etlogo || this.upfile != null
            }
        },
        watch: {
        },
        created: function () {
            var th = this;
            $api.get('Organization/Current').then(function (req) {
                vapp.loading_init = false;
                if (req.data.success) {
                    th.organ = req.data.result;
                    th.getTreeData(th.organ.Org_ID);
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            });
        },
        mounted: function () {

        },
        methods: {
            //获取课程专业的数据
            getTreeData: function (orgid) {
                var th = this;
                this.loading = true;
                $api.get('Subject/Tree', { orgid: orgid, search: '', isuse: null })
                    .then(function (req) {
                        th.loading = false;
                        if (req.data.success) {
                            th.subjects = req.data.result;
                        } else {
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        console.error(err);
                    }).finally(function () {
                        th.getEntity();
                    });
            },
            //获取当前实体
            getEntity: function () {
                var th = this;
                th.loading = true;
                if (th.id == '' || th.id == null) {
                    $api.get('Snowflake/Generate').then(function (req) {
                        th.loading = false;
                        if (req.data.success) {
                            th.entity.Sbj_ID = req.data.result;
                            th.traversalUse(th.subjects);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        th.loading = false;
                        Vue.prototype.$alert(err);
                        console.error(err);
                    });
                    return;
                }
                $api.get('Subject/ForID', { 'id': th.id }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.entity = result;
                        //设置当前节点禁用，防止选择自身
                        var sbj = th.traversalQuery(result.Sbj_ID, th.subjects);
                        if (sbj != null) sbj.Sbj_IsUse = false;
                        th.traversalUse(th.subjects);
                        //将当前专业的上级专业，在控件中显示
                        var arr = [];
                        arr = th.getParentPath(th.entity, th.subjects, arr);
                        th.sbjSelects = arr;
                    } else {
                        throw '未查询到数据';
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    th.loading = false;
                });
            },
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid,fields) => {
                    if (valid) {
                        var sbj = th.clone(th.entity);
                        //return;
                        th.loading = true;
                        //接口路径
                        var apipath = 'Subject/' + (th.id == '' ? 'add' : 'Modify');
                        //接口参数，如果有上传文件，则增加file
                        var para = {};
                        if (th.upfile == null) para = { 'entity': sbj };
                        else
                            para = { 'file': th.upfile, 'entity': sbj };
                        $api.post(apipath, para).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '修改成功!',
                                    center: true
                                });
                                window.setTimeout(function () {
                                    th.operateSuccess();
                                }, 600);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err, '错误');
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
            //为上传数据作处理
            clone: function (entity) {
                var obj = $api.clone(entity);
                if (this.upfile == null) {
                    obj.Sbj_Logo = '';
                    obj.Sbj_LogoSmall = '';
                }
                //上级专业
                if (this.sbjSelects.length > 0)
                    obj.Sbj_PID = this.sbjSelects[this.sbjSelects.length - 1];
                else
                    obj.Sbj_PID = 0;
                if (obj.Sbj_PID == '') obj.Sbj_PID = 0;

                return obj;
            },
            //操作成功
            operateSuccess: function () {
                $api.cache('Subject/ForID:clear', { 'id': this.id });
                window.top.$pagebox.source.tab(window.name, 'vapp.fresh_operateSuccess', true);
            },
            //获取当前专业的上级路径
            getParentPath: function (entity, datas, arr) {

                var obj = this.traversalQuery(entity.Sbj_PID, datas);
                if (obj == null) return arr;
                arr.splice(0, 0, obj.Sbj_ID);
                arr = this.getParentPath(obj, datas, arr);
                return arr;
            },
            //从树中遍历对象
            traversalQuery: function (sbjid, datas) {
                var obj = null;
                for (let i = 0; i < datas.length; i++) {
                    const d = datas[i];
                    if (d.Sbj_ID == sbjid) {
                        obj = d;
                        break;
                    }
                    if (d.children && d.children.length > 0) {
                        obj = this.traversalQuery(sbjid, d.children);
                        if (obj != null) break;
                    }
                }
                return obj;
            },
            //所有Sbj_IsUse取反，主要是Cascader控件的disabled取了Sbj_IsUse作为值，它俩的意义相反
            traversalUse: function (datas, use) {
                for (let i = 0; i < datas.length; i++) {
                    const d = datas[i];
                    d.Sbj_IsUse = use == null ? !d.Sbj_IsUse : use;

                    if (d.children && d.children.length > 0)
                        obj = this.traversalUse(d.children, d.Sbj_IsUse ? true : null);
                }
            }
        },
    });

});
