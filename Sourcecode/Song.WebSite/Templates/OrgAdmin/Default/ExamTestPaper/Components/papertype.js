//试卷类型
$dom.load.css([$dom.pagepath() + 'Components/Styles/papertype.css']);
Vue.component('papertype', {
    props: ['type'],
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

    },
    template: `<div class="papertype" :type="type">
        <span v-if="type==2">
            <icon large>&#xe6cc</icon>  随机出题
        </span>
        <span v-if="type==1">
            <icon large>&#xa055</icon> 固定试题
        </span>
    </div> `
});