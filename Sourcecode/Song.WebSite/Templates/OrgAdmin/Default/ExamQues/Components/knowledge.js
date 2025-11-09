//试题编辑中的知识点选择
$dom.load.css([$dom.pagepath() + 'Components/Styles/knowledge.css']);
Vue.component('knowledge', {
    props: ["question", "org"],
    data: function () {
        return {
            
            loading: false,
          
        }
    },
    watch: {
        'question': {
            handler: function (nv, ov) {
               
            }, immediate: true
        },        
    },
    computed: {},
    mounted: function () {
       
    },
    methods: {
       
    },
    template: `<div class="knowledge">
        
       
    </div> `
});