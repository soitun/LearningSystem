//结课条件
$dom.load.css([$dom.pagepath() + 'Styles/final_condition.css']);
Vue.component('final_condition', {
    //参数：机构参数，购买记录，试卷id，学员,是否一直显示
    props: ['config', 'purchase', 'paperid', 'account', 'show'],
    data: function () {
        return {
            results: [],
            loading: false
        }
    },
    watch: {
        //当学员登录后
        'account': {
            handler: function (nv, ov) {
                var stid = Number(nv.Ac_ID);
                stid = isNaN(stid) ? 0 : stid;
                this.getresults(stid, this.paperid);
            },
            immediate: true
        },
        //课程购买记录
        'purchase': {
            handler: function (nv, ov) {
                nv.Stc_StudyScore = nv.Stc_StudyScore >= 100 ? 100 : nv.Stc_StudyScore;
            },
            immediate: true
        },
    },
    computed: {
        //购买记录是否存在
        ispurchase: function () {
            return JSON.stringify(this.purchase) != '{}' && this.purchase != null;
        }
    },
    mounted: function () {
    },
    methods: {
        //获取机构的配置参数
        orgconfig: function (para, def) {
            var val = this.config[para];
            if (val === 0) return 0;
            if (!val) return def ? def : '';
            return val;
        },
        //结果考试的按钮是否通过,为true时表示不通过
        final_disable: function () {
            //视频学习进度是否达成
            var condition_video = this.orgconfig('finaltest_condition_video', 100);
            if (condition_video > this.purchase.Stc_StudyScore) return true;
            //试题练习通过率是否达成
            var condition_ques = this.orgconfig('finaltest_condition_ques', 100);
            if (condition_ques > this.purchase.Stc_QuesScore) return true;
            //最多可以考几次
            var finaltest_count = this.orgconfig('finaltest_count', 1);
            if (finaltest_count <= this.results.length) return true;
            return false;
        },
        //获取历史成绩
        getresults: function (stid, tpid) {
            var th = this;
            if (stid <= 0 || tpid <= 0) return;
            th.loading = true;
            $api.get('TestPaper/ResultsAll', { 'stid': stid, 'tpid': tpid }).then(function (req) {
                if (req.data.success) {
                    th.results = req.data.result;
                    console.log(th.results);
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }

            }).catch(function (err) {
                th.results = [];
                alert(err);
                console.error(err);
            }).finally(() => th.loading = false);
        },
        //获取测试的最高得分
        score_highest: function () {
            if (this.results.length < 1) return;
            var highest = 0;
            for (let i = 0; i < this.results.length; i++) {
                const el = this.results[i];
                if (el.Tr_Score > highest) highest = el.Tr_Score;
            }
            //如果结课成绩与课程购买记录中的不一致，更新购买记录中的结课成绩
            if (this.ispurchase && this.purchase.Stc_ExamScore != highest) {
                var th = this;
                var form = { 'acid': th.account.Ac_ID, 'couid': th.purchase.Cou_ID, 'score': highest }
                $api.post('TestPaper/ResultLogRecord', form).then(function (req) {
                    if (req.data.success) {
                        th.purchase.Stc_ExamScore = highest;
                        th.$notify({ type: 'success', message: '更新结课考试成绩' });
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err));
            }

            return highest;
        },
    },
    template: `<div class="final_condition" v-if="show || final_disable()">
        <slot></slot>
        <div>
            1、视频学习完成<b>{{orgconfig('finaltest_condition_video',100)}}%</b>，当前完成<b>{{purchase.Stc_StudyScore}}%</b>，
            <el-tag type="success" v-if="orgconfig('finaltest_condition_video',100)<=purchase.Stc_StudyScore">
                <icon>&#xa048</icon>达成
            </el-tag>

            <el-tag type="danger" v-else>
                <icon>&#xe72c</icon>未达成
            </el-tag>
        </div>
        <div>
            2、试题练习通过率达到<b>{{orgconfig('finaltest_condition_ques',100)}}%</b>，当前完成<b>{{purchase.Stc_QuesScore}}%</b>，
            <el-tag type="success" v-if="orgconfig('finaltest_condition_ques',100)<=purchase.Stc_QuesScore">
                <icon>&#xa048</icon>达成
            </el-tag>
            <el-tag type="danger" v-else>
                <icon>&#xe72c</icon>未达成
            </el-tag>
        </div>
        <div>
            3、最多允许考试 <b>{{orgconfig('finaltest_count',1)}}</b> 次，
            <template v-if="results.length>0">
                已经考了<b>{{results.length}}</b>次，最高得分
                <b>{{score_highest()}}分</b></template>
            <template v-else>还未参加过结课考试</template>
        </div>
    </div>`
});

