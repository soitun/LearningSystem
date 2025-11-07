//试题编辑中的基本信息
$dom.load.css([$dom.pagepath() + 'Components/Styles/general.css']);
Vue.component('general', {
    props: ["question", "org"],
    data: function () {
        //config的字段是否为空
        let isrequired = function (rule, value, callback) {
            var field = rule.field;
            if (!!vapp.config[field] && vapp.config[field] != '')
                return callback();
            callback(new Error("不得为空"));
        };
        return {
            //标签输入框
            taginput: '',
            tagShowInput: false,

            tagmax: 6,       //最多可输入的标签数量

            loading: false
        }
    },
    watch: {
        'question': {
            handler: function (nv, ov) {
                //获取试题的关键字
                if (nv != null) this.gettags();

                //如果是新增试题，总之难度小于零，默认为3
                if (!nv.Qus_Diff || nv.Qus_Diff <= 0) nv['Qus_Diff'] = 3;
                if (nv.Qus_Diff >= 5) nv.Qus_Diff = 5;
                //默认为启用状态
                if (!nv.Qus_IsUse) nv['Qus_IsUse'] = true;
                if (!nv.Qus_Order) nv['Qus_Order'] = 0;

            }, immediate: true,

        },
        'org': {
            handler: function (nv, ov) {
                //if (nv) this.getCourses();
            }, immediate: true
        },
    },
    computed: {
        //试题关键字列表
        taglist: function () {
            return this.question.Tags;
        }
    },
    mounted: function () {

    },
    methods: {
        //试题关键字
        gettags: function () {
            var th = this;
            if (th.loading || th.taglist != null) return;
            th.loading = true;
            $api.get("ExamQues/TagForQues", { "quesid": th.question.Qus_ID })
                .then(req => {
                    if (req.data.success) {
                        th.taglist = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        //显示新增标签输入框
        showTagInput() {
            this.tagShowInput = true;
            this.$nextTick(_ => {
                this.$refs.taginput.$refs.input.focus();
            });
        },
        //编辑标签
        tagedit: function () {
            if (this.taginput == null || this.taginput.length < 1) return;
            let str = this.taginput.replace(/，/g, ",");
            str = str.replace(/\s/g, ",");
            str = str.replace(/,+/g, ",");
            if (str == null || str.length < 1) {
                this.tagShowInput = false;
                return;
            }
            let arr = str.split(",");
            if (this.taglist == null) this.taglist = [];
            for (let i = 0; i < arr.length; i++) {
                arr[i] = $api.trim(arr[i]);
                if (arr[i] == null || arr[i].length < 1) continue;
                if (!this.taglist.some(tag => tag.Qtag_Name == arr[i]))
                    this.taglist.push({ Qtag_Name: arr[i] });
            }
            if (this.taglist.length > this.tagmax) {
                this.$message({
                    message: '最多可以添加' + this.tagmax + '个标签',
                    type: 'warning'
                });
                this.taglist.splice(this.tagmax)
            }
            this.tagShowInput = false;
            this.taginput = '';

        },
        //设置试题的关键字
        setquestag: function () {
            var th = this;
            if (th.taginput == null || th.taginput.length < 1) return;
            $api.get("ExamQues/TagConnectionQues", { "tags": th.taginput, "quesid": th.question.Qus_ID, "couid": "" })
                .then(req => {
                    if (req.data.success) {
                        let result = req.data.result;
                        th.gettags();
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => { });
        },
        //查询关键字
        tagquery: function (search, cb) {
            var th = this;
            $api.get("ExamQues/TagPager", { "orgid": th.question.Org_ID, "couid": 0, "isdeleted": false, "name": search, "size": 10, "index": 1 })
                .then(req => {
                    if (req.data.success) {
                        let result = req.data.result;
                        cb(result);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => { });
        },
        tagselect: function (item) {
            if (!this.taglist.some(tag => tag.Qtag_Name == item.Qtag_Name))
                this.taglist.push(item);
            this.taginput = item.Qtag_Name;
            this.tagedit();
        }
    },
    template: `<div class="general">
        <el-form ref="question" :model="question" @submit.native.prevent label-width="80px">    
            <el-form-item label="难度" prop="Qus_Diff">
                <el-rate title="点击难度值" v-model="question.Qus_Diff" :max="5" show-score></el-rate>
            </el-form-item>
            <el-form-item label="排序号" prop="Qus_Order" v-if="false">
                <el-input-number v-model="question.Qus_Order"></el-input-number>
                <help>数值越小越靠前，可以为负值</help>
            </el-form-item>
            <el-form-item label="" prop="Qus_IsUse">
                <el-switch  v-model="question.Qus_IsUse"  active-text="启用"  inactive-text="禁用"></el-switch>
            </el-form-item>         
            <el-form-item label="关键字" prop="taginput" class="taglist">               
                <el-tag size="medium" v-for="(tag,idx) in taglist" closable @close="taglist.splice(idx, 1)"> {{tag.Qtag_Name}}</el-tag>
                <el-button v-if="!tagShowInput" class="button-new-tag" size="small" @click="showTagInput">+ 新增关键字</el-button>
                <el-autocomplete class="input-new-tag" clearable v-else v-model="taginput" ref="taginput"
                  :fetch-suggestions="tagquery"  placeholder="请输入内容"  @select="tagselect"
                  @keyup.enter.native="tagedit" @blur="tagedit">
                    <template slot-scope="{ item }">
                        <div class="name" v-html="showsearch(item.Qtag_Name,taginput)"></div>                       
                    </template>
                </el-autocomplete>               
            </el-form-item>   
        </el-form>
    </div> `
});