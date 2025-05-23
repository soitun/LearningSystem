$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            form: { 'acid': '', 'start': '', 'end': '', 'size': 10, 'index': 1 },

            datas: [],      //数据集，此处是登录记录的列表
            total: 1, //总记录数
            totalpages: 1, //总页数

            loading_init: true,
            loading: false
        },
        mounted: function () {
            this.handleCurrentChange();
        },
        created: function () {
        },
        computed: {
            //是否登录
            islogin: (t) => { return !$api.isnull(t.account); }
        },
        watch: {

        },
        methods: {
            //选择时间区间
            selectDate: function (start, end) {
                this.form.start = start;
                this.form.end = end;
                this.handleCurrentChange(1);
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                //console.error(this.form);
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 115;
                th.form.size = Math.floor(area / 47);
                th.loading = true;
                $api.get("Account/LoginLogs", th.form).then(function (d) {
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                        //console.log(th.accounts);
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //显浏览时间
            timeclac: function (second) {
                if (second < 60) return second + ' 秒';
                let mm = Math.floor(second / 60);
                if (mm < 60) return mm + ' 分钟';
                let hh = Math.floor(mm / 60);
                if (hh < 24) return hh + ' 小时' + (mm % 60 > 0 ? mm % 60 + '分钟' : '');
            },
            //地理位置
            address: function (row) {
                if (row.Lso_Province == row.Lso_City) return row.Lso_Province + row.Lso_District;
                return row.Lso_Province + row.Lso_City + row.Lso_District;
            },
        }
    });

}, ['Components/account_header.js']);
