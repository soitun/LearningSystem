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
        <el-tooltip v-if="type==2" content="试卷类型：随机出题" placement="bottom">
            <span><icon large>&#xe6cc</icon>  随机出题</span>
        </el-tooltip>
        <el-tooltip v-if="type==1" content="试卷类型：固定试题" placement="bottom">
            <span><icon large>&#xe667</icon> 固定试题</span>
        </el-tooltip>        
    </div> `
});