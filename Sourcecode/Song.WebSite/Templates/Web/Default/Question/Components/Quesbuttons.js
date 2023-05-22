﻿//试题右侧的按钮组
$dom.load.css([$dom.pagepath() + 'Components/Styles/quesbuttons.css']);
Vue.component('quesbuttons', {
    //current:当前显示的试题，即滑动到这个试题
    props: ['question', 'account', 'couid', 'current'],
    data: function () {
        return {
            //试题中的按钮，当used为true时，启用icon2图标
            buttons: [
                { id: 'error', name: '报错', icon1: '&#xe70e', icon2: '&#xe72c', used: false, evt: this.errorshow },
                { id: 'notes', name: '笔记', icon1: '&#xa02e', icon2: '&#xa02e', used: false, evt: this.noteeditshow },
                { id: 'collect', name: '收藏', icon1: '&#xe747', icon2: '&#xe679', used: false, evt: this.addcollect }],
            //供选择的错误项
            errorItems: ['试题没有答案', '试题图片不显示', '试题或答案有错别字',
                '试题不属于本学科', '解题思路与题干不符', '解题思路与答案矛盾',
                '与其他试题有重复'],
            errorSelect: [],     //选中的错误项
            errorInfo: '',       //填写的错误内容
            //笔记内容
            note: null,
            //是否显示笔记编辑的面板
            isShowNote: false,
            //是否显示报错界面
            isShowError: false,
            //初始化
            init: false
        }
    },
    watch: {
        'question': {
            handler(nv, ov) {
                if (nv.Qus_IsWrong) {
                    var btn = this.getbtn('error');
                    if (btn != null) btn.used = true;
                }
                if (this.current) {
                    this.collectState();
                    this.noteState();
                }
            },
            immediate: true
        },
        'errorSelect': function (nv, ov) {
            console.log(nv);
        },
        //是否是当前显示的试题
        'current': {
            handler(nv, ov) {
                if (!ov && nv && !this.init) {
                    this.init = true;
                    this.collectState();
                    this.noteState();
                }
            },
            immediate: true
        }
    },
    mounted: function () {

    },
    methods: {
        //获取按钮
        getbtn: function (id) {
            for (let i = 0; i < this.buttons.length; i++) {
                if (this.buttons[i].id === id)
                    return this.buttons[i];
            }
            return null;
        },
        //收藏的状态
        collectState: function () {
            var th = this;
            //试题是否被收藏
            var query = { 'acid': this.account.Ac_ID, 'qid': this.question.Qus_ID };
            $api.get('Question/CollectExist', query).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    var btn = th.getbtn('collect');
                    if (btn != null) btn.used = result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        //笔记的状态，及内容
        noteState: function () {
            var th = this;
            var query = { 'acid': this.account.Ac_ID, 'qid': this.question.Qus_ID };
            $api.get('Question/NotesSingle', query).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.note = result ? result.Stn_Context : '';
                    if (th.note != '') {
                        var btn = th.getbtn('notes');
                        if (btn != null) btn.used = true;
                    }
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                //console.error(err);
            });
        },
        //设置收藏
        addcollect: function (btn) {
            var th = this;
            if (!btn.used) {
                var query = { 'acid': this.account.Ac_ID, 'qid': this.question.Qus_ID, 'couid': this.couid };
                $api.post('Question/CollectAdd', query).then(function (req) {
                    if (req.data.success) {
                        btn.used = true;
                        th.$message({
                            message: '试题收藏成功',
                            type: 'success'
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            } else {
                //删除收藏
                var query = { 'acid': this.account.Ac_ID, 'qid': this.question.Qus_ID };
                $api.get('Question/CollectDelete', query).then(function (req) {
                    if (req.data.success) {
                        btn.used = false;
                        th.$message({
                            message: '删除收藏成功',
                            type: 'success'
                        });                     
                        vapp.deleteQues();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            }
            console.log(btn);
        },
        //笔记编辑面板的显示
        noteeditshow: function (th) {
            this.isShowNote = true;
        },
        //更改笔记内容
        noteUpdate: function (note) {
            var th = this;
            th.isShowNote = false;
            var query = { 'acid': this.account.Ac_ID, 'qid': this.question.Qus_ID, 'note': note };
            $api.post('Question/NotesModify', query).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.$notify({ type: 'success', message: '笔记编辑成功' });
                    var btn = th.getbtn('notes');
                    if (btn != null) btn.used = result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        //试题报错的编辑面板显示
        errorshow: function (th) {
            this.isShowError = true;
        },
        //提交错误信息
        errorUpdate: function () {
            var th = this;
            th.isShowError = false;
            var error = th.errorSelect.join(',') + "；" + this.errorInfo;
            $api.get('Question/WrongModify', { 'qid': this.question.Qus_ID, 'error': error }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.$notify({ type: 'success', message: '提交成功' });
                    //th.question.Qus_IsWrong=true;
                    $api.put('Question/ForID', { 'id': th.question.Qus_ID }).then(function (req) {
                        if (req.data.success) {
                            th.question = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });                 
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        }
    },
    template: `<buttons no-font-size>        
        <btn v-for="btn in buttons" @click="btn.evt(btn)" :class="{used:btn.used}">
        <i v-html="btn.icon1" v-if="!btn.used"></i>
        <i v-html="btn.icon2" v-else></i>
        {{btn.name}}
        </btn>
        <el-dialog  width="80%" :visible.sync="isShowNote" class="quesNote" title="笔记" append-to-body>
                <el-input v-model="note" :rows="5" type="textarea"  maxlength="140"
                placeholder="请输入笔记内容" show-word-limit></el-input>
                <div>
                    <el-button type="primary" @click="noteUpdate(note)"><icon>&#xe84c</icon>确 定</el-button>
                </div>
      </el-dialog>
      <el-dialog :visible.sync="isShowError"  append-to-body class="quesError" title="报错">
            <template v-if="!question.Qus_IsWrong">
                <el-checkbox-group v-model="errorSelect">
                    <el-checkbox :label="err" v-for="err in errorItems" shape="square">{{err}}</el-checkbox>                   
                </el-checkbox-group>
                <el-divider content-position="left">其它问题</el-divider>
                <el-input v-model="errorInfo" rows="2" type="textarea"  maxlength="140"
                placeholder="请输入需要报错的内容" show-word-limit></el-input>    
                <div class="btnbar">           
                    <el-button type="primary" @click="errorUpdate(note)"><icon>&#xe84c</icon>提交信息</el-button>   
                </div> 
            </template>       
            <div v-else>
                <el-divider content-position="left">已经有学员反馈：</el-divider>
                     <div>{{question.Qus_WrongInfo}}</div>
            </div>    
        </el-dialog>
    </buttons> `
});