//试题收藏
$dom.load.css([$dom.pagepath() + 'Components/Styles/ques_collect.css']);
Vue.component('ques_collect', {
    //ques:试题对象
    //accid:管理员的账户ID    
    props: ['ques', 'accid'],
    data: function () {
        return {
            collected: false,   //是否已收藏
            loading: false
        }
    },
    watch: {
        //监听试题对象
        'ques': {
            handler(nv, ov) {
                if ($api.isnull(nv)) return;
                this.getstate(nv);
            }, immediate: true,
        }
    },
    computed: {},
    mounted: function () {

    },
    methods: {
        //获取收藏状态   
        getstate: function () {
            var th = this;
            th.loading = true;
            $api.get("ExamQues/Collected", { "accid": th.accid, "qusid": th.ques.Qus_ID })
                .then(req => {
                    if (req.data.success) {
                        th.collected = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        //设置收藏或取消收藏
        setcollect: function () {
            var th = this;
            th.loading = true;
            th.collected = !th.collected;
            $api.post("ExamQues/CollectUpdate", { "accid": th.accid, "qusid": th.ques.Qus_ID, "state": th.collected })
                .then(req => {
                    if (req.data.success) {
                        let result = req.data.result;
                        if (th.collected) {
                            this.$notify({
                                title: '成功',
                                message: '收藏该试题',
                                type: 'success', duration: 1000
                            });
                        } else {
                            this.$notify({
                                title: '取消',
                                message: '取消收藏该试题',
                                type: 'warning', duration: 1000
                            });
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        }
    },
    template: `<div class="ques_collect">
        <loading v-if="loading" asterisk></loading>
        <el-link v-else-if="!collected" type="warning" plain @click="setcollect"> <icon>&#xe747</icon> 收藏</el-link>
        <el-link v-else type="danger" plain @click="setcollect"> <icon>&#xe679</icon>  取消收藏</el-link>
    </div> `
});