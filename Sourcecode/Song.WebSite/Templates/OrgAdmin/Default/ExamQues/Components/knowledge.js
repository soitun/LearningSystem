//试题编辑中的知识点选择
$dom.load.css([$dom.pagepath() + 'Components/Styles/knowledge.css']);
Vue.component('knowledge', {
    props: ["question", "org"],
    data: function () {
        return {
            datas: [],
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
    },
    computed: {},
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
                        th.datas = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
    },
    template: `<div class="knowledge">
        
       
    </div> `
});