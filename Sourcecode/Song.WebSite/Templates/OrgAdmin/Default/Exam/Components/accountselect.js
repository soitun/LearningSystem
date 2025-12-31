// 账号选择
//事件：
//select:当选择学员时触发，返回选中的学员
Vue.component('accountselect', {
    //accounts:已经选择的学员
    props: ['orgid', 'size', 'accounts'],
    data: function () {
        return {
            tabs: [
                { title: '单个添加', name: 'add', icon: 'e759' },
                { title: '批量添加', name: 'bat', icon: 'a04d' }],
            activeName: 'bat',     //选项卡

            loading: false
        }
    },
    watch: {

    },
    computed: {},
    mounted: function () {

    },
    methods: {
        //添加学员
        studentadd_event: function (accounts) {
            console.error(accounts);
        },
    },
    template: `<div class="accountselect">
        <el-tabs v-model="activeName">
            <el-tab-pane v-for="tab in tabs" :name="tab.name">
                <span slot="label">
                    <icon v-html="'&#x'+tab.icon" medium></icon> {{tab.title}}
                </span>
            </el-tab-pane>
        </el-tabs>
       
        <!-- 新增学员到学员组（单个新增）-->
        <student_add  v-show="activeName=='add'" :orgid="orgid" ref="studentadd" @add="studentadd_event">
        </student_add>       
        
        <!-- 批量添加学员到学员组-->
        <student_batadd  v-show="activeName=='bat'" :orgid="orgid" ref="studentbatadd" @add="studentadd_event">
        </student_batadd>
       
    </div> `
});

// 新增学员到学员组（单个新增）
Vue.component('student_add', {
    props: ['stsid', 'orgid'],
    data: function () {
        return {
            accounts: [],
            form: {
                'orgid': '', 'sortid': '', 'use': null, 'acc': '', 'name': '', 'phone': '', 'idcard': '',
                'gender': '-1', 'orderby': '', 'orderpattr': '',
                'index': 1, 'size': '10'
            },
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            loading: false
        }
    },
    watch: {
        'orgid': {
            handler: function (nv, ov) {
                if (nv != null) {
                    var th = this;
                    th.form.orgid = nv;
                    th.loading = true;
                    //初始加载
                    window._student_add_load = window.setInterval(function () {
                        var area = $dom('.student_add');
                        if (area.length > 0 && area.height() > 0) {
                            window.clearInterval(window._student_add_load);
                            var maxheight = area.height() - 34 - 32 - 10;
                            //console.error(maxheight);
                            th.form.size = Math.round(maxheight / 30);
                            th.getdatas(1);
                        }
                    }, 100);
                }
            }, immediate: true
        }

    },
    computed: {},
    mounted: function () { },
    methods: {
        //加载数据页
        getdatas: function (index) {
            if (index != null) this.form.index = index;
            var th = this;
            th.loading = true;
            $api.get("Account/Pager", th.form).then(function (d) {
                if (d.data.success) {
                    th.accounts = d.data.result;
                    th.totalpages = Number(d.data.totalpages);
                    th.total = d.data.total;
                } else {
                    console.error(d.data.exception);
                    throw d.data.message;
                }
            }).catch(function (err) {
                alert(err);
            }).finally(() => th.loading = false);
        },
        //增加学员
        add: function (item) {
           this.$emit('add', [item]);
        }
    },
    //
    template: `<div class="student_add">
            <header>
                <el-input v-model="form.name" placeholder="姓名" clearable @input="getdatas(1)"></el-input>                    
                <el-input v-model="form.idcard" placeholder="身份证" clearable @input="getdatas(1)"></el-input> 
                <el-input v-model="form.phone" placeholder="电话" clearable @input="getdatas(1)"></el-input> 
            </header>
            <loading v-if="loading">加载中...</loading>
            <dl class="list" v-else-if="accounts.length>0">
                <dd v-for="(item,i) in accounts" :index="(form.index-1) * form.size+i+1"> 
                    <icon class="name" size="large" :woman="item.Ac_Gender==2" :man="item.Ac_Gender==1" v-html='showsearch(item.Ac_Name,form.name)'></icon>                      
                    <span v-if="form.phone!=''" class="phone"  v-html='showsearch(item.Ac_MobiTel1,form.phone)'></span>   
                    <span v-else class="idcard"  v-html='showsearch(item.Ac_IDCardNumber,form.idcard)'></span>                  
                    <el-link  class="btn" type="primary" @click="add(item)" title="添加">添加</el-link>
                </dd>
            </dl>
            <icon v-else null>没有满足条件的数据</icon>
            <el-pagination v-on:current-change="getdatas" :current-page="form.index" 
                :page-size="form.size" :pager-count="4" layout="total, prev, pager, next" :total="total">
            </el-pagination>
        </div>`
});
//批量添加学员到学员组
Vue.component('student_batadd', {
    props: ['stsid', 'orgid'],
    data: function () {
        return {           

            datas: [],
            search_type: 'card',    //检索类型，账号acc,身份证card，手机mobi

            inputText: '',
            inputIsChange: false,        //是否有输入变化
            operstatus: 1,        //操作状态，默认1录入数据，2为解析数据
            loading: false
        }
    },
    watch: {
        'orgid': {
            handler: function (nv, ov) {

            }, immediate: true
        },
        'inputText': function (nv, ov) {
            this.inputIsChange = true;
        },
        //操作状态，默认1录入数据，2为解析数据
        'operstatus': {
            handler: function (nv, ov) {
                if (nv == 2)
                    this.parseInput();
            }, immediate: true
        }

    },
    computed: {},
    mounted: function () { },
    methods: {       
        //解析录入的学员账号信息
        parseInput: function () {
            if (!this.inputIsChange) return;
            var str = $api.trim(this.inputText);
            this.datas = [];
            if (str == '') return;

            //解析录入的信息
            var arr = str.split("\n");
            //校验证手机号，简单校验
            if (this.search_type == "mobi") {
                var regPos = / ^\d+$/; // 非负整数 
                for (var i = 0; i < arr.length; i++) {
                    const d = arr[i].replace(/\s*/g, "");
                    if (!regPos.test(d)) arr.splice(i, 1);
                }
            }
            //校验身份证，简单校验而已
            if (this.search_type == "card") {
                var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
                for (var i = 0; i < arr.length; i++) {
                    const d = arr[i].replace(/\s*/g, "");
                    if (!reg.test(d)) arr.splice(i, 1);
                }
            }
            for (var i = 0; i < arr.length; i++) {
                arr[i] = arr[i].replace(/\s*/g, "");
                if (arr[i] == '') continue;
                //state：状态,初始为-1，账号不存在为0，存在为1，处理完成为2
                this.datas.push({ 'text': arr[i], 'account': {}, 'state': -1 });
            }
            console.log(arr);
        },
        //查询的完成数
        'query_complete': function () {
            var c = 0;
            for (var i = 0; i < this.datas.length; i++) {
                if (this.datas[i].state != -1) {
                    c++;
                }
            }
            return c;
        },
        //有效的记录数
        'query_valid': function () {
            var c = 0;
            for (var i = 0; i < this.datas.length; i++) {
                if (this.datas[i].state == 1) {
                    c++;
                }
            }
            return c;
        },
        //增加学员
        add: function () {
            var num = this.query_valid();
            if (num <= 0) {
                this.$alert('请提供有效的学员信息', '没有数据', {
                    confirmButtonText: '确定'
                });
                return;
            }
            this.$confirm('是否将当前 ' + num + '个学员添加到当前组, 是否继续?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                var arr = [];
                for (let i = 0; i < this.datas.length; i++) {
                    if (this.datas[i].state == 1) {
                        arr.push(this.datas[i].account.Ac_ID);
                    }
                }
                if (arr.length > 0) this.add_func(arr.join(','));
            }).catch(() => { });
            return;

        },
        //添加学员到学员组的具体方法
        add_func: function (ids) {
            console.log(ids);
            var th = this;
            var loading = th.$fulloading();
            $api.post('Account/SortAddStudent', { 'stsid': th.stsid, 'id': ids }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.$emit('addfinish', th.stsid, ids);
                    th.$nextTick(function () {
                        loading.close();
                        th.datas = [];
                        th.inputText = '';
                        th.inputIsChange = false;        //是否有输入变化
                        th.operstatus = 1;        //操作状态，默认1录入数据，2为解析数据
                    });
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                loading.close();
                console.error(err);
            });
        }
    },
    //
    template: `<div class="student_batadd">  
            <header>
                请在下面输入框录入学员信息，换行分隔<br />并明确录入的是：
                <el-radio-group v-model="search_type" :disabled="operstatus==2">
                    <el-radio label="acc">账号</el-radio>
                    <el-radio label="card">身份证</el-radio>
                    <el-radio label="mobi">手机号</el-radio>
                </el-radio-group>
            </header>
            <div class="btns">
                <el-button type="primary" plain v-if="operstatus==1" @click="operstatus=2">
                    <icon>&#xe83c</icon>解析录入的信息
                </el-button>
                <template v-else>
                    <el-button type="success" plain  @click="operstatus=1">
                        <icon>&#xe63d</icon>继续编辑内容
                    </el-button>
                    <el-button type="primary" plain @click="add">
                        <icon>&#xe6ea</icon>全部添加<span>（{{query_valid()}}条）</span>
                    </el-button>         
                </template>       
            </div>
            <el-input type="textarea" class="inputText" :rows="2" placeholder="请输入内容" v-if="operstatus==1"  v-model="inputText">
            </el-input>          
            <el-table ref="datatables"  border resizable  class="table_datas" :stripe="true" :data="datas" tooltip-effect="dark" v-if="operstatus==2" 
                    style="width: 100%">
                    <el-table-column type="index" label="#" align="center">
                        <template slot-scope="scope">
                            <span>{{scope.$index + 1}}</span>
                        </template>
                    </el-table-column>
                    <el-table-column label="录入的信息">
                        <template slot="header" slot-scope="scope">
                            <span v-if="search_type=='acc'">学员账号</span>
                            <span v-if="search_type=='card'">身份证号</span>
                            <span v-if="search_type=='mobi'">手机号</span>
                            <span title="总数">：{{datas.length}} 条</span>
                        </template>
                        <template slot-scope="scope">
                            {{scope.row.text}}
                        </template>
                    </el-table-column>
                    <el-table-column label="账号查询">
                        <template slot="header" slot-scope="scope">
                            查询完成{{query_complete()}}条，有效{{query_valid()}}条
                        </template>
                        <template slot-scope="scope">
                            <account :item="scope.row" :text="scope.row.text" :type="search_type"></account>
                        </template>
                    </el-table-column>
                </el-table>
        </div>`
});
//账号信息的获取
Vue.component('account', {
    //item:录入项,
    //text:录入的内容，可能是账号或身份证号
    //type:搜索类型
    props: ["item", "text", "type"],
    data: function () {
        return {
            data: null,
            state: -1,
            loading: true
        }
    },
    watch: {
        'text': {
            handler: function (nv, ov) {
                this.state = this.item.state;
                this.getaccount();
            }, immediate: true, deep: true
        }
    },
    computed: {},
    mounted: function () { },
    methods: {
        getaccount: function () {
            var th = this;
            th.loading = true;
            //
            var apiurl = "Account/ForAcc", para = {};
            if (this.type == 'acc') {
                apiurl = "Account/ForAcc";
                para = { "acc": this.text };
            }
            if (this.type == 'card') {
                apiurl = "Account/ForIDCard";
                para = { "card": this.text };
            }
            if (this.type == 'mobi') {
                apiurl = "Account/ForMobi";
                para = { "phone": this.text };
            }
            $api.cache(apiurl, para).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.data = req.data.result;
                    th.item.account = req.data.result;
                    th.item.state = th.state = 1;
                } else {
                    th.data = null;
                    th.item.account = null;
                    th.item.state = th.state = 0;
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            });
        }
    },
    template: `<span title="学员信息">
            <span v-if="state==-1" class="el-icon-loading"></span>
            <span v-if="state==0"><el-tag type="info">不存在</el-tag></span>
            <span v-if="state==1"  :class="{'woman': data.Ac_Gender==2,'disable':!data.Ac_IsUse}">
                <icon v-if="data.Ac_Gender==2" woman>{{data.Ac_Name}}</icon>
                <icon v-if="data.Ac_Gender==1" man>{{data.Ac_Name}}</icon>  
                <span v-if="!data.Ac_IsUse">（已经禁用）</span>
            </span>
        </span> `
});