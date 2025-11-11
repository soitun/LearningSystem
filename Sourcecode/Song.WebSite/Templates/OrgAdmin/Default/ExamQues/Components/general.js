//试题编辑中的基本信息
$dom.load.css([$dom.pagepath() + 'Components/Styles/general.css']);
Vue.component('general', {
    props: ["question", "org"],
    data: function () {
        return {
            //标签输入框
            taginput: '',
            tagShowInput: false,
            tagmax: 6,       //最多可输入的标签数量

            partall: [],       //试题分类     
            partsearch:'',      //分类搜索关键字    
            partloading: false,   //分类加载中

            loading: false
        }
    },
    watch: {
        'question': {
            handler: function (nv, ov) {
                //如果是新增试题，总之难度小于零，默认为3
                if (!nv.Qus_Diff || nv.Qus_Diff <= 0) nv['Qus_Diff'] = 3;
                if (nv.Qus_Diff >= 5) nv.Qus_Diff = 5;
                //默认为启用状态
                if (!nv.Qus_IsUse) nv['Qus_IsUse'] = true;
                if (!nv.Qus_Order) nv['Qus_Order'] = 0;

            }, immediate: true,
        },
        'question.Qus_ID': {
            handler: function (nv, ov) {
                //获取试题的关键字
                if (nv != null && nv != 0) {
                    this.gettags();
                    this.getallparts();
                }
            }, immediate: true,
        },
         //过滤树形数据
         partsearch: function (val) {
            this.$refs.parttree.filter(val);           
        },
    },
    computed: {
        //试题关键字列表
        taglist: t => t.question.Tags,
        //试题的分类
        parts: t => t.question.Parts,
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
        //选择下拉的关键字时
        tagselect: function (item) {
            if (!this.taglist.some(tag => tag.Qtag_Name == item.Qtag_Name))
                this.taglist.push(item);
            this.taginput = item.Qtag_Name;
            this.tagedit();
        },
        /** 试题分类 */
        //获取所有试题分类
        getallparts: function () {
            var th = this;
            th.partloading = true;
            $api.get("ExamQues/PartTreeFront", { "orgid": th.question.Org_ID })
                .then(req => {
                    if (req.data.success) {
                        th.partall = req.data.result;
                        th.getparts();
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
            //.finally(() => th.partloading = false);
        },
        //获取当前试题的分类
        getparts: function () {
            var th = this;
            th.partloading = true;
            $api.get("ExamQues/PartForQues", { "qusid": th.question.Qus_ID })
                .then(req => {
                    if (req.data.success) {
                        th.parts = req.data.result;
                        th.setCheckedKeys();
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => th.partloading = false);
        },
        //分类的节点点击事件
        //data:当前节点对象，即QuesPart实体
        //stat:树形节点状态，checkedNodes为选中节点，halfCheckedNodes为半选中节点
        partcheck: function (value, stat) {
            console.error(stat);
            //添加到当前试题的分类
            let checkedparts = stat.checkedNodes ?? [];
            for (let i = 0; i < checkedparts.length; i++) {
                let part = checkedparts[i];
                if (!this.parts.some(p => p.Qp_ID == part.Qp_ID)) {
                    this.parts.push(part);
                }
            }
            //
            let checkedNodes = this.$refs.parttree.getCheckedNodes();
            this.parts.splice(0, this.parts.length);
            this.parts = [];
            for (let i = 0; i < checkedNodes.length; i++) {
                let part = checkedNodes[i];
                if (!this.parts.some(p => p.Qp_ID == part.Qp_ID)) {
                    if (part.children == null)
                        this.parts.push(part);
                }
            }
            //this.parts = checkedNodes;
        },
        //设置当前试题的分类
        setCheckedKeys: function () {
            var arr = [];
            for (var i = 0; i < this.parts.length; i++)
                arr.push(this.parts[i].Qp_ID);
            this.$refs.parttree.setCheckedKeys(arr);

        },
        //移除已经选择的分类
        removepart: function (idx) {
            this.parts.splice(idx, 1);
            this.setCheckedKeys();
        },
         //过滤树形
         filterNode: function (value, data) {
            if (!value) return true;
            var txt = $api.trim(value.toLowerCase());
            if (txt == '') return true;
            return data.Qp_Name.toLowerCase().indexOf(txt) !== -1;
        },
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
                <el-tag type="warning" size="medium" v-for="(tag,idx) in taglist" closable @close="taglist.splice(idx, 1)"> {{tag.Qtag_Name}}</el-tag>
                <el-button v-if="!tagShowInput" class="button-new-tag" size="small" @click="showTagInput">+ 添加关键字</el-button>
                <el-autocomplete class="input-new-tag" clearable v-else v-model="taginput" ref="taginput"
                  :fetch-suggestions="tagquery"  placeholder="请输入内容"  @select="tagselect"
                  @keyup.enter.native="tagedit" @blur="tagedit">
                    <template slot-scope="{ item }">
                        <div class="name" v-html="showsearch(item.Qtag_Name,taginput)"></div>                       
                    </template>
                </el-autocomplete>               
            </el-form-item>   
            <el-form-item label="试题分类" prop="parts" class="parts-area">
                <div class="parts-tree">
                    <div class="title">
                        <el-input placeholder="检索" v-model="partsearch" clearable></el-input>
                    </div>
                    <div class="parttree">
                        <el-tree ref="parttree" node-key="Qp_ID" :props="{label: 'Qp_Name',id:'Qp_ID',children: 'children'}" 
                        :data="partall" show-checkbox @check="partcheck" :filter-node-method="filterNode" empty-text="没有满足条件的数据">
                            <div class="custom-node" slot-scope="{ node, data }">
                                <span class="large" v-html="showsearch(data.Qp_Name,partsearch)"></span>
                            </div> 
                        </el-tree>
                    </div>
                </div>
                <div class="selected_parts">
                    <div class="title">已选 {{parts.length}} 个分类</div>
                    <div class="part_list" v-if="parts?.length>0">
                        <el-tag size="medium" v-for="(p,idx) in parts" closable @close="removepart(idx)">
                        {{p.Qp_Name}}</el-tag>
                    </div>
                    <div class="part_list_null" v-else>暂无</div>
                </div>
            </el-form-item>    
        </el-form>
    </div> `
});