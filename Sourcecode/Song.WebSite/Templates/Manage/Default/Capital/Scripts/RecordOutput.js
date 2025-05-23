﻿
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            loading: false,  //
            id: $api.querystring('id'),
            files: [],          //已经生成的excel文件列表
            //path: 'MoneyOutputToExcel',     //导出的文件的存储路径
            form: {
                path: 'MoneyOutputToExcel',     //导出的文件的存储路径
                orgid: -1,
                from: -1,     //来源
                type: -1,     //类型，支出或充值               
                start: '',       //时间区间的开始时间
                end: ''         //结束时间               
            },           
        },
        watch: {
            
        },
        created: function () {
            //日期范围的初始时间值
            const start = new Date();
            start.setDate(1);
            var yy = start.getFullYear();
            var mm = start.getMonth() + 1;
            if (mm > 12) {
                mm = 1;
                yy = yy + 1;
            }
            var end = new Date(yy, mm, 0);
            this.selectDate = [];
            this.selectDate[0] = start;
            this.selectDate[1] = end;

            this.getFiles();
        },
        methods: {
             //选择时间区间
             selectDate: function (start, end) {
                this.form.start = start;
                this.form.end = end;
                this.handleCurrentChange(1);
            },
            btnOutput: function () {
                //创建生成Excel
                this.loading = true;
                var th = this;
                $api.get('Money/ExcelOutput', this.form).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            message: '成功生成Excel文件！',
                            type: 'success',
                            position: 'bottom-left',
                            duration: 2000
                        });
                        th.getFiles();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //获取文件列表
            getFiles: function () {
                var th = this;
                $api.get('Money/ExcelFiles', { 'path': this.form.path, 'orgid': -1 }).then(function (req) {
                    if (req.data.success) {
                        th.files = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //删除文件
            deleteFile: function (file) {
                var th = this;
                th.loading = true;
                $api.get('Money/ExcelDelete', { 'path': th.form.path, 'orgid': -1, 'filename': file }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.getFiles();
                        th.$notify({
                            message: '文件删除成功！',
                            type: 'success',
                            position: 'bottom-left',
                            duration: 2000
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading = false);
            }
        }
    });
});
