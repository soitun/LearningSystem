//固定试卷在选择试题时的试题行
$dom.load.css([$dom.pagepath() + 'Components/Styles/quesrow.css']);
Vue.component('quesrow', {
    // 属性
    //serial:序号
    props: ['ques', 'types', 'admin', 'serial', 'search'],
    data: function () {
        return {
            loading: false
        }
    },
    watch: {

    },
    computed: {},
    mounted: function () {

    },
    methods: {
        //解析试题答案
        answer: function (q) {
            let func = 'type' + q.Qus_Type;
            return this[func] != null ? this[func](q) : '';
        },
        type1: q => String.fromCharCode(65 + q.Qus_Items.findIndex(m => m.Ans_IsCorrect)),
        type2: q => q.Qus_Items.map((m, i) => m.Ans_IsCorrect ? String.fromCharCode(65 + i) : null)
            .filter(m => m !== null).join('、'),
        type3: q => q.Qus_IsCorrect ? "正确" : "错误",
        type4: q => q.Qus_Answer,
        type5: q => q.Qus_Items.length === 1 ?
            q.Qus_Items[0].Ans_Context : q.Qus_Items.map((m, i) => `${i + 1}、${m.Ans_Context}`).join('；'),
    },
    template: ` <div class="quesrow" :qid="ques.Qus_ID">
    <div class="quesrow-title">
        <div class="index">{{serial}}.</div>
        <ques_type :type="ques.Qus_Type" :types="types" :showicon="true" :showname="true">
        </ques_type>
        <ques_diff :ques="ques"></ques_diff>
        <ques_collect :ques="ques" :accid="admin.Acc_Id"></ques_collect>
        <div>
            <el-checkbox v-model="ques.checked">选中</el-checkbox>
        </div>
    </div>
    <div class="quesrow-content" @click="$set(ques,'showdetail',!ques.showdetail)"
        :showdetail="ques.showdetail">
        <span v-html="showsearch(ques.Qus_Title,search)"></span>
        <div v-if="ques.Qus_Type==1 || ques.Qus_Type==2" class="qus_items" remark="单选、多选的选项">
            <div v-for="(item,index) in ques.Qus_Items" :correct="item.Ans_IsCorrect"
                v-html="String.fromCharCode(65 + index) +'.'+item.Ans_Context">
            </div>
        </div>
        <div class="answer">
            正确答案：<span v-html="answer(ques)"></span>
        </div>
    </div>
</div>`
});

