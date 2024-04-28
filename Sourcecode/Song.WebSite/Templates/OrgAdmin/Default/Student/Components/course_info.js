//课程相关信息
Vue.component('course_info', {
    //config:机构的配置项，其实包括了视频完成度的容差值（VideoTolerance）
    props: ['course', 'stid', 'config'],
    data: function () {
        return {
            data: {},        //进度信息
            percent: 0,     //完成的真实百分比
            tolerance: 10,       //容差，例如容差为10，则完成90%即可认为是100%
            loading: false
        }
    },
    watch: {
        'course': {
            handler: function (nv, ov) {
                this.onload();
            }, immediate: true, deep: true
        },
        'config': {
            handler: function (nv, ov) {
                if (nv && nv.VideoTolerance) {
                    this.tolerance = Number(nv.VideoTolerance);
                    this.tolerance = isNaN(this.tolerance) ? 0 : this.tolerance;
                    this.tolerance = this.tolerance <= 0 ? 0 : this.tolerance;
                }
            }, immediate: true, deep: true
        },
    },
    computed: {
        'color': function () {
            if (this.progress == 0) return '#909399';
            if (this.progress < 30) return '#F56C6C';
            if (this.progress < 60) return '#E6A23C';
            if (this.progress < 90) return '#67C23A';
            if (this.progress < 100) return 'rgb(106 179 255)';
            return '#409EFF';
        },
        //完成度，加了容差之后的
        'progress': function () {
            return this.percent + this.tolerance >= 100 ? 100 : this.percent;
        },
    },
    mounted: function () { },
    methods: {
        onload: function () {
            var th = this;
            th.loading = true;
            $api.cache('Course/LogForVideo:5', { 'couid': this.course.Cou_ID, 'stid': this.stid })
                .then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        if (result != null && result.length > 0) {
                            th.data = result[0];
                            th.data.lastTime = new Date(th.data.lastTime);
                            th.percent = th.data.complete;
                            console.log(th.data);
                        } else {
                            th.data = null;
                            th.percent = 0;
                        }

                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        //是否完成
        finished: function (percentage) {
            return percentage >= 100;
        },
        //进度条显示的数值样式
        format(percentage) {
            return this.finished(percentage) ? '完成学习！100%' : '学习进度：' + percentage + '%';
        },
        //累计学习时间
        studyTime: function (time) {
            if (time < 60) return time + "秒";
            if (time >= 60) {
                var ss = time % 60;
                var mm = Math.floor(time / 60);
                if (mm < 60) {
                    return mm + "分" + ss + "秒";
                } else {
                    var hh = Math.floor(mm / 60);
                    mm = mm % 60;
                    return hh + '小时' + mm + "分" + ss + "秒";
                }
            }
            return time;
        }
    },
    template: `<div clalss="info_row">                
        <el-tag type="warning" v-if="!(course.Cou_IsFree || course.Cou_IsLimitFree)">
            <purchase_data :couid="course.Cou_ID" :stid="stid"></purchase_data>
            {{course.endtime|date("yyyy年M月d日")}}
        </el-tag>
        <el-tag type="success" v-if="course.Cou_IsLimitFree">
            免费至 {{course.Cou_FreeEnd|date('yyyy-MM-dd')}}
        </el-tag>                 
        <template v-if=" JSON.stringify(data) != '{}' && data != null">
            <el-tag type="success"><icon>&#xa039</icon>最后学习时间：{{data.lastTime|date('yyyy-MM-dd HH:mm')}}</el-tag>
            <el-tag type="info"><icon>&#xe6bf</icon>累计学习时长：{{studyTime(data.studyTime)}}</el-tag>
        </template>                            
    </div>`
});