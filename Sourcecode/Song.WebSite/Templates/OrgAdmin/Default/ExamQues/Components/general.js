//试题编辑中的基本信息
$dom.load.css([$dom.pagepath() + 'Components/Styles/general.css']);
Vue.component('general', {
    props: ["question", "org"],
    data: function () {
        return {
            
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
            }, immediate: true
        },
        'org': {
            handler: function (nv, ov) {
                //if (nv) this.getCourses();
            }, immediate: true
        },      
    },
    computed: {

    },
    mounted: function () {
      
    },
    methods: {


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
           
        </el-form>
    </div> `
});