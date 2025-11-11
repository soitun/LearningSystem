//试题编辑中的知识点选择
$dom.load.css([$dom.pagepath() + 'Components/Styles/knowledge.css']);
Vue.component('knowledge', {
    props: ["question", "org"],
    data: function () {
        return {
            datas: [],
            knlsearch: '',      //分类搜索关键字    

            //knls: [],   //试题关联的知识点
            loading: false,

        }
    },
    watch: {
        'question': {
            handler: function (nv, ov) {
                if (nv != null && nv.Org_ID > 0)
                    this.getknlall();
            }, immediate: true
        },
        //过滤树形数据
        knlsearch: function (val) {
            this.$refs.knltree.filter(val);
        },
    },
    computed: {
        //试题的知识点
        knls: t => t.question.Knls,
        //知识点数量
        knlslength: function () {
            let len = this.knls?.length ?? 0;
            //console.error(len);
            return len;
        },
    },
    mounted: function () {

    },
    methods: {
        //获取所有知识点列表
        getknlall: function () {
            var th = this;
            th.loading = true;
            $api.get("ExamQues/KnlTree", { "orgid": th.question.Org_ID, "search": "", "isuse": true })
                .then(req => {
                    if (req.data.success) {
                        let results = req.data.result;
                        th.datas = th.calcknls(results, th.knls);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        //计算已经选中的知识点
        calcknls: function (treedata, selectarr) {
            let treedatalen = treedata?.length ?? 0;
            if (treedata == null || treedatalen < 1) return treedata;
            for (let i = 0; i < treedatalen; i++) {
                let idx = selectarr.findIndex(x => x.Qk_ID == treedata[i].Qk_ID);
                treedata[i]["selected"] = idx >= 0;
                if (treedata[i]["children"] != null) {
                    treedata[i].children = this.calcknls(treedata[i].children, selectarr);
                }
            }
            return treedata;
        },
        //过滤树形
        filterNode: function (value, data) {
            if (!value) return true;
            var txt = $api.trim(value.toLowerCase());
            if (txt == '') return true;
            return data.Qk_Name.toLowerCase().indexOf(txt) !== -1;
        },
        //分类的节点点击事件
        //data:当前节点对象，即QuesKnowledge实体      
        knlcheck: function (value, ctrl) {
            this.$set(value, "selected", !value.selected);
            let idx = this.knls.findIndex(x => x.Qk_ID == value.Qk_ID);
            if (value.selected && idx < 0) this.knls.push(value);
            if (!value.selected && idx >= 0) this.knls.splice(idx, 1);
        },
        //获取已经选择的知识点
        gettreeselected: function (knls, selectarr) {
            if (selectarr == null) selectarr = [];
            if (knls == null || this.knlslength < 1) return selectarr;
            for (let i = 0; i < this.knlslength; i++) {
                if (knls[i].selected) selectarr.push(knls[i]);
                if (knls[i]["children"] != null)
                    selectarr = selectarr.concat(this.gettreeselected(knls[i].children));
            }
            return selectarr;
        },
        //移除已经选择的知识点
        removeknl: function (idx) {
            this.knls.splice(idx, 1);
            this.calcknls(this.datas, this.knls);
        },
    },
    template: `<div class="knowledge">
        <el-card shadow="never" class="knl-tree">
            <template slot="header">
                <span><icon medium>&#xe84d</icon>知识点</span>
                <el-input placeholder="检索" v-model="knlsearch" clearable></el-input>
            </template>
            <div class="knltree">
                <el-tree ref="knltree" node-key="Qk_ID" :props="{label: 'Qk_Name',id:'Qk_ID',children: 'children'}" 
                :data="datas" @node-click="knlcheck" :filter-node-method="filterNode" empty-text="没有满足条件的数据">
                    <div :class="{'selected':data.selected,'knlnode':true}" slot-scope="{ node, data }">
                        <span v-if="data.selected"><icon>&#xa048</icon></span>
                        <span class="large" v-html="showsearch(data.Qk_Name,knlsearch)"></span>
                    </div> 
                </el-tree>
            </div>
        </el-card>
        <el-card shadow="never" class="selected_knls">
            <div slot="header" class="title">
                <el-badge :value="knlslength" :hidden="knlslength<1">
                    <icon>&#xa029</icon>关联的知识点</span>
                </el-badge>               
            </div>
            <div class="knls_list" v-if="knlslength>0">
                <div v-for="(k,idx) in knls" >
                    {{idx+1}} .
                    <el-tag size="medium"closable @close="removeknl(idx)">
                    {{k.Qk_Name}}</el-tag>
                </div>
            </div>
            <div class="knls_list_null" v-else>暂无</div>
        </el-card>    
    </div> `
});