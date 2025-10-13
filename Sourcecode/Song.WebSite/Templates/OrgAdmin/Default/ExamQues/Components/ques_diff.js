//试题的难度
Vue.component('ques_diff', {
    props: ['ques'],
    data: function () {
        return {
            diffs: ['很简单', '简单', '一般', '较难', '比较难'],
            loading: false
        }
    },
    watch: {

    },
    computed: {},
    mounted: function () {

    },
    methods: {

    },
    template: `<div class="ques_diff">  
       {{diffs[ques.Qus_Diff]}}
    </div> `
});