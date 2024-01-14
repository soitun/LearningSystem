﻿// 页面顶部的按钮条
//参数：
//show:显示哪些按钮，中文加逗号
//selects: 选中的数据，用于编辑或删除时
//idkey:数据对象中的ID的键名，用于取selects中的id值
//path:要打开的窗体的页面路路，width和height即窗口宽高
//ico: 弹窗的图标值，不用带&#x
//modal:弹窗是否为模态窗（即窗口在最前面，不可切换，不可最小化）
//disabled:是否按钮全禁用
Vue.component('btngroup', {
    props: ['show', 'selects', 'idkey', 'path', 'ico', 'width', 'height', 'modal', 'disabled'],
    data: function () {
        // data 选项是一个函数，组件不相互影响
        return {
            buttonArray: [{
                text: '新增', tips: '新增',
                id: 'add', type: 'success',
                class: 'el-icon-plus'
            }, {
                text: '修改', tips: '修改数据，请选择数据行',
                id: 'modify', type: 'primary',
                class: 'el-icon-edit'
            }, {
                text: '删除', tips: '删除选中的数据项',
                id: 'delete', type: 'danger',
                class: 'el-icon-delete'
            }, {
                text: '导入', tips: '批量导入数据',
                id: 'input', type: 'info',
                class: 'el-icon-folder-add',
                icon: 'e69f'
            }, {
                text: '导出', tips: '批量导出数据',
                id: 'output', type: 'info',
                class: 'el-icon-document-copy',
                icon: 'e73e'
            }, {
                text: '说明', tips: '相关说明',
                id: 'help', type: 'warning',
                class: 'el-icon-document-copy',
                icon: 'a026'
            }],
            iconfont: 'webdesk_icon'
        }
    },
    watch: {
        'selects': function (val, old) {
            //console.log(val);
        }
    },
    computed: {
        buttons: function () {
            if (this.show == '') return [];
            let btnarr = [];
            //按参数show的设置顺序来显示按钮
            let showarr = this.show.split(',');
            for (let i = 0; i < showarr.length; i++) {
                const show = showarr[i];
                for (let j = 0; j < this.buttonArray.length; j++) {
                    const btn = this.buttonArray[j];
                    if (show == btn.text) {
                        btnarr.push(btn);
                        break;
                    }
                }
            }
            //如果按钮属性为enable，不用在show中设置也可以显示
            for (let i = 0; i < this.buttonArray.length; i++) {
                const btn = this.buttonArray[i];
                let isexist = false;
                for (let j = 0; j < btnarr.length; j++) {
                    if (btnarr[j].text == btn.text) {
                        isexist = true;
                        break;
                    }
                }
                if (btn.enable && !isexist)
                    btnarr.push(btn);
            }
            return btnarr;
        }
    },
    methods: {
        //添加按钮
        addbtn: function (btn) {
            this.buttonArray.push(btn);
        },
        eventClick: function (btn) {
            let btnid = btn.id;
            //当前点击的按钮
            let curr = this.getCurrbtn(btnid);
            //新增与修改因为有默认事件方法，所有这里做个判断         
            if (btnid == 'add' || btnid == 'modify') {
                //如果设置了事件，则直接执行，否则执行默认事件
                let existEvent = this.$listeners[btnid];
                if (existEvent) return this.$emit(btnid, curr, this);
                if (!top.$pagebox) {
                    return this.$message({
                        message: '未找到pagebox.js对象',
                        type: 'error'
                    });
                }
            }
            if (btnid == 'add') return this.add();     //添加            
            if (btnid == 'modify') return this.modify(this.getid());       //修改            
            if (btnid == 'delete') return this.delete(this.getids(), curr);    //删除   
            //其它按钮事件
            let existEvent = this.$listeners[btnid];
            if (existEvent) return this.$emit(btnid, curr, this.getids());
        },
        //添加按钮事件
        add: function (url, param) {
            if (url == null) url = this.path;
            if (!(top.$pagebox && url)) return;
            url = this.setParameter(url, '');
            this.pagebox(url, '新增', window.name + '[add]', this.width, this.height, param);
        },
        //修改事件
        modify: function (id, title, param) {
            //如果传进来的是对象
            if (id && typeof (id) == 'object') {
                id = id[this.idkey];
                if (id == null) id = '';
            }
            if (id == '') {
                this.$message({
                    message: '请选中要编辑的数据行',
                    type: 'error'
                });
                return;
            }
            if (!this.path) {
                this.$message({
                    message: '未设置编辑页的路径',
                    type: 'error'
                });
                return;
            }
            if (!(top.$pagebox && this.path)) return;
            let url = this.setParameter(this.path, id);
            this.pagebox(url, title ? title : '修改/查看', window.name + '_' + id + '[modify]', this.width, this.height, param);
        },
        modifyrow: function (row, title, param) {
            if (!this.idkey) return '';
            let id = !!row[this.idkey] ? row[this.idkey] : '';
            this.modify(id, title, param);
        },
        //删除事件
        delete: function (ids, btn) {
            let arr = String(ids).split(',');
            if (ids == '' || arr.length < 1) {
                this.$message({
                    message: '请选中要操作的数据行',
                    type: 'error'
                });
                return false;
            }
            if (btn == null) {
                this.$emit('delete', ids, btn);
                return true;
            }
            var th = this;
            this.$confirm('是否确认删除这 ' + arr.length + ' 项数据? ', '谨慎操作', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(function () {
                th.$emit('delete', ids, btn);
            }).catch(function () { });
        },
        //获取当前点击的按钮
        getCurrbtn: function (btnid) {
            let curr = {};
            let btn = this.buttons;
            for (let t in btn) {
                if (btn[t].id == btnid) {
                    curr = btn[t];
                    break;
                }
            }
            return curr;
        },
        //获取id
        getid: function () {
            if (!this.idkey || !this.selects || this.selects.length < 1) return '';
            let id = !!this.selects[0][this.idkey] ? this.selects[0][this.idkey] : '';
            return id;
        },
        //获取树菜单节点数据
        getnode: function () {
            let tree = top.tree;
            if (tree) return tree.getData(window.name);
            return null;
        },
        //获取多个id，用逗号分隔
        getids: function () {
            if (!this.idkey || !this.selects || this.selects.length < 1) return '';
            let str = '';
            for (let i = 0; i < this.selects.length; i++) {
                let id = !!this.selects[i][this.idkey] ? this.selects[i][this.idkey] : '';
                if (id == '') continue;
                str += i < this.selects.length - 1 ? id + ',' : id;
            }
            return str;
        },
        //获取完整的页面路径
        getfullpath: function (path) {
            if (path.substring(0, 1) == "/") return path;
            let root = String(window.document.location.href);
            let file = root.substring(0, root.lastIndexOf("/") + 1);
            return file + path;
        },
        //设置参数
        setParameter: function (url, id) {
            let params = $api.url.params();
            for (let i = 0; i < params.length; i++)
                url = $api.url.set(url, params[i].key, params[i].val);
            return $api.url.set(url, 'id', id);
        },
        //打开窗体
        //url:弹窗内页的链接
        //title:标题
        //boxid:弹窗的标识，不是单纯的id
        //param:弹窗参数，
        pagebox: function (url, title, boxid, width, height, param) {
            if (!top.$pagebox) {
                return this.$message({
                    message: '未找到pagebox.js对象',
                    type: 'error'
                });
            }
            url = this.getfullpath(url);
            let node = this.getnode();
            let tit = node ? node.title : $dom('title').text();
            if ($dom('title').text() != '' && (!title || title != '')) tit += ' - ';
            let ico = this.ico ? this.ico : (node && node.ico != '' ? node.ico : 'a021');
            let iconstyle = node && !!node.icon ? node.icon : {};
            let titstyle = node && !!node.font ? node.font : {};
            let attrs = {
                width: width ? width : 400,
                height: height ? height : 300,
                url: url,
                ico: ico, iconstyle: iconstyle,
                pid: window.name,
                id: boxid,
                title: tit + title, titstyle: titstyle
            };
            if (param != null) {
                for (let t in param) attrs[t] = param[t];
            }
            //如果是模态窗口，则显示遮罩且不允许最小化
            if (this.modal) {
                attrs['showmask'] = true;
                attrs['min'] = false;
            }
            //console.log(attrs);
            if (!top.$pagebox) return;
            let pbox = top.$pagebox.create(attrs);
            pbox.open();
        },
        //按钮图标
        btnicon: function (btn) {
            return "&#x" + btn.icon;
        }
    },
    template: `<div class="btngroup">
        <el-button-group>
            <template v-for="(btn,index) in buttons">
                <el-tooltip class="item" :content="btn.tips" placement="bottom" effect="light">
                    <el-button :key="index" :type="btn.type"  size="small" plain 
                    v-on:click="eventClick(btn)" :disabled="disabled"
                    :class="!!btn.class && !btn.icon ? btn.class : false">
                        <icon v-if="btn.icon" v-html="btnicon(btn)" style="margin-right:0px"></icon>&nbsp; {{btn.text}}
                    </el-button>
                </el-tooltip>
            </template>
            <slot></slot>
        </el-button-group>
    </div>`
});
