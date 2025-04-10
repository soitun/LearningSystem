﻿/*!
 * 主 题：树形菜单
 * 说 明：
 * 1、支持无限级菜单分类;
 * 2、可自定义节点样式，例如：粗体、斜体、颜色;
 * 3、节点事件可定义
 *
 * 作 者：微厦科技_宋雷鸣_10522779@qq.com
 * 开发时间: 2020年1月1日
 * 最后修订：2023年10月8日
 * Git开源地址:https://gitee.com/weishakeji/WebdeskUI
 */
(function (win) {
	var treemenu = function (param) {
		if (param == null || typeof (param) != 'object') param = {};
		this.attrs = {
			target: '', //所在Html区域			
			width: '100%',
			height: '100%',
			id: '',
			complete: false,	//是否显示完成度
			bind: true, //是否实时数据绑定
			fold: false, //是否折叠
			query: false,		//是否启用查询功能
			querypanel: false,		//是否显示查询面板
			taghide: false		//是否隐藏左侧选项卡的竖栏
		};
		for (let t in param) this.attrs[t] = param[t];
		eval($ctrl.attr_generate(this.attrs));
		/* 自定义事件 */
		//fold,折叠或展开;data，数据源变化时; change，切换根菜单,click点击菜单项
		eval($ctrl.event_generate(['fold', 'data', 'change', 'resize', 'click']));

		this.datas = new Array(); //子级	
		this._datas = ''; //数据源的序列化字符串	
		this.dom = null; //控件的html对象
		this.domtit = null; //控件标签栏部分的html对象
		this.dombody = null; //控件内容区
		this.domquery = null;	//查询菜单的面板
		//面板是否显示，当鼠标滑过时如果为false，3秒后隐藏面板
		this.leavetime = 3;
		this.leaveshow = false;
		//默认数据
		this.def_data = {
			title: '数据加载...', id: 'loading',
			tit: 'load', type: 'loading', ico: 'e621'
		};
		this.datas.push(this.def_data);
		//查询按钮
		this.query_data = {
			title: '菜单项检索', id: 'query',
			tit: '检索', type: 'query', ico: 'f004c'
		};
		//初始化并生成控件
		this._initialization();
		this.bind = this._bind;
		this.fold = this._fold;
		//
		$ctrls.add({
			id: this.id, obj: this, dom: this.dom,
			type: 'treemenu'
		});
	};
	var fn = treemenu.prototype;
	fn._initialization = function () {
		if (!this._id) this._id = 'treemenu_' + new Date().getTime();
	};
	//添加数据源
	fn.add = function (item) {
		if (item instanceof Array) {
			for (let i = 0; i < item.length; i++)
				this.add(item[i]);
		} else {
			this.datas.push(item);
		}
		//一旦加载数据，则去除最初的预载信息
		for (let i = 0; i < this.datas.length; i++) {
			if (this.datas[i].type && this.datas[i].type == 'loading')
				this.datas.splice(i, 1);
		}
	};
	//当属性更改时触发相应动作
	fn._watch = {
		'width': function (obj, val, old) {
			if (obj.dom) {
				obj.dom.width(val);
				if (obj.taghide) obj.dombody.width(val)
				else
					obj.dombody.width(val - 40);
				//触发尺寸变化事件
				obj.trigger('resize', { width: val, height: obj._height, action: 'width' });
			}
		},
		'height': function (obj, val, old) {
			if (obj.dom) {
				obj.dom.height(val);
				obj.domtit.height(val);
				//触发尺寸变化事件
				obj.trigger('resize', { width: obj._width, height: val, action: 'height' });
			}
		},
		//是否显示查询按钮
		'query': function (obj, val, old) {
			if (obj.domtit) {
				let el = obj.domtit.find('tree_tag[type=query]');
				val ? el.show() : el.hide();
			}
		},
		//是否显示查询面板
		'querypanel': function (obj, val, old) {
			if (obj.domquery) {
				if (!val) return obj.domquery.hide();
				obj.domquery.find('section:first-child').css('min-width', obj.datas.length * 180 + 'px');
				//将树形菜单的html直接放入查询面板，不再直接生成
				let sect = obj.domquery.find('section>section');
				sect.html(obj.dombody.html());
				sect.find('tree_area').show();
				sect.find('tree_area tree_box').show();
				sect.find('tree_area tree-node.folderclose').attr('class', 'folder');
				sect.find('tree_box').removeClass('current');
				//计算菜单项数量
				let total = obj.getNodeCount(true);
				let count = obj.getNodeCount(false);
				obj.domquery.find('header>div[title]').html('全部菜单项 (' + count + '/' + total + '个)');

				obj.domquery.show('flex');
				obj.domquery.find('tree-node').click(function (e) {
					let node = e.target ? e.target : e.srcElement;
					while (node.nodeName.toLowerCase() != 'tree_box') node = node.parentNode;
					let treeid = node.getAttribute('treeid');
					let ctrobj = treemenu._getObj(e);	//控件对象
					let datanode = ctrobj.getData(treeid); //数据源节点					
					//触发节点点击事件
					if (datanode.type != 'link' && datanode.childs.length < 1) {
						ctrobj.trigger('click', { treeid: treeid, data: datanode });
						ctrobj.querypanel = false;
					}
				});
				//重新设置查询结果的状态
				let input = obj.domquery.find('input');
				obj.queryEffect(input.val());
				input.focus();		//设置焦点
			}
		},
		//是否显示完成度的进度条
		'complete': function (obj, val, old) {
			if (obj.dom) {
				let el = obj.dom.find('complete');
				val ? el.show() : el.hide();
			}
		},
		//折叠与展开
		'fold': function (obj, val, old) {
			if (obj.dom == null || obj.dom.length < 1) return;
			obj.domtit.find('tree-foldbtn').attr('class', obj.fold ? 'fold' : '');
			if (val) {
				//折叠
				obj.dom.width(40);
				obj.dombody.css('position', 'absolute');
				obj.dombody.left(obj.domtit.width());
				obj.dombody.height(obj.dom.height()).width(0);
				obj.dom.attr('fold', obj.fold);		//在控件html元素上标识折叠状态
				obj.dom.removeAttr('expand');
			} else {
				obj.dom.width(obj.width);
				obj.dombody.width(obj.width - 40);
				obj.dom.removeAttr('fold');
				window.setTimeout(function () {
					obj.dombody.css('position', 'relative');
					obj.dombody.left(0);
				}, 300);
			}
			//折叠事件
			obj.trigger('fold', { action: val ? 'fold' : 'open' });
		},
		//是否启动实时数据绑定
		'bind': function (obj, val, old) {
			if (val) {
				obj._setinterval = window.setInterval(function () {
					let str = JSON.stringify(obj.datas);
					if (str != obj._datas) {
						//计算数据源的层深等信息	
						obj.datas = obj._calcLevel($dom.clone(obj.datas), 1);
						obj._restructure();
						obj._datas = JSON.stringify(obj.datas);
						//触发数据变更事件
						obj.trigger('data', { data: obj.datas });
					}
				}, 10);
			} else {
				window.clearInterval(obj._setinterval);
			}
		}
	};
	//重构
	fn._restructure = function () {
		let area = $dom(this.target);
		if (area.length < 1) {
			console.log('treemenu所在区域不存在');
		} else {
			area.html(''); //清空原html节点
			//生成Html结构和事件
			for (let t in this._builder) this._builder[t](this);
			for (let t in this._baseEvents) this._baseEvents[t](this);
			this.width = this._width;
			this.height = this._height;
			this.complete = this._complete;
		}
	};

	//生成结构
	fn._builder = {
		shell: function (obj) {
			let area = $dom(obj.target);
			if (area.length < 1) {
				console.log('treemenu所在区域不存在');
				return;
			}
			area.addClass('treemenu').attr('ctrid', obj.id);
			obj.dom = area;
		},
		//左侧标签区
		title: function (obj) {
			//if (obj.taghide) return;
			obj.domtit = obj.dom.add('tree_tags');
			obj.domtit.add('tree-tagspace').height('10px');
			//创建左侧选项卡
			for (let i = 0; i < obj.datas.length; i++) {
				const item = obj.datas[i];
				let tabtag = createTag(obj.domtit.add('tree_tag'), item);
			}
			//查询按钮
			let tabtag = createTag(obj.domtit.add('tree_tag'), obj.query_data);
			obj.query ? tabtag.show() : tabtag.hide();
			tabtag.bind('click', function (event) {
				let crtobj = treemenu._getObj(event);
				if (crtobj == null) return;
				crtobj.querypanel = true;
			});
			//创建选项卡的html
			function createTag(tag, data) {
				tag.attr('title', data.title).attr('treeid', data.id);
				tag.attr('type', data.type);
				//图标
				if (data.ico && data.ico != '') {
					let ico = tag.add('ico').html('&#x' + data.ico);
					if (data.icon) {
						if (data.icon.color) ico.css('color', data.icon.color,);
						if (data.icon.x != 0) ico.css('margin-left', data.icon.x + 'px');
						if (data.icon.y != 0) ico.css('margin-top', data.icon.y + 'px');
						if (data.icon.size != 0) ico.css('transform', 'scale(' + (1 + data.icon.size / 100) + ')');
					}
				}
				//标题
				let tit = tag.add('itemtxt').html(data.tit);
				if (data.font) {
					if (data.font.color) tit.css('color', data.font.color, true);
					if (data.font.bold) tit.css('font-weight', data.font.bold ? 'bold' : 'normal', true);
					if (data.font.italic) tit.css('font-style', data.font.italic ? 'italic' : 'normal', true);
				}
				return tag;
			}
			//隐藏左侧标签栏
			if (obj.taghide) obj.domtit.hide();
			obj.domtit.add('tree-foldbtn');
		},
		//右侧内容区
		body: function (obj) {
			obj.dombody = obj.dom.add('tree_body');
			for (let i = 0; i < obj.datas.length; i++) {
				const item = obj.datas[i];
				//右侧树形菜单区
				let area = obj.dombody.add('tree_area');
				area.attr('treeid', item.id).hide();
				//右侧菜单的大标题
				let tit = area.add('tree_tit');
				if (item.ico != '') {
					let ico = tit.add("i").html("&#x" + item.ico);				
					if (item.icon) {
						if (item.icon.y != 0) ico.css('margin-top', item.icon.y + 'px');
						if (item.icon.size != 0) ico.css('transform', 'scale(' + (1 + item.icon.size / 100) + ')');
					}
				}
				tit.add("span").html(item.title);
				if (item.type == 'loading') {
					area.add('tree-loading').html('&#x' + item.ico);
					continue;
				}
				let div = area.add('div');
				if (item.childs) {
					for (let j = 0; j < item.childs.length; j++)
						treemenu._add_nodechild(div, item.childs[j], obj);
				}
			}
		},
		//查询面板
		querypanel: function (obj) {
			if (obj.domquery != null) return;
			obj.domquery = $dom('body').add('div').hide();
			let panel = obj.domquery;
			panel.attr('ctrid', obj.id).addClass('querypanel').addClass('treemenu');
			panel.click(function (event) {
				let node = event.target ? event.target : event.srcElement;
				if (!$dom(node).hasClass('querypanel')) return;
				let ctrl = $ctrls.get(node.getAttribute('ctrid'));
				if (ctrl != null) ctrl.obj.querypanel = false;
			});
			//创建查询内容区
			let sect = panel.add('section');
			//头部
			let head = sect.add('header');
			head.add('div').attr('title', '全部菜单项').add('span');
			head.add('div').attr('query', '检索菜单项').append('input').add('clear');
			//关闭按钮
			sect.add('div').addClass('query_close').bind('click', function (event) {
				let node = event.target ? event.target : event.srcElement;
				while (!$dom(node).hasClass('querypanel')) node = node.parentNode;
				let ctrl = $ctrls.get(node.getAttribute('ctrid'));
				if (ctrl != null) ctrl.obj.querypanel = false;
			});
			//查询事件
			head.find('input').bind('change,input', function (event) {
				let node = event.target ? event.target : event.srcElement;
				let crtobj = treemenu._getObj(event);
				if (crtobj != null) crtobj.queryEffect(node.value);
			});
			head.find('clear').click(function (event) {
				let crtobj = treemenu._getObj(event);
				if (crtobj == null) return;
				crtobj.domquery.find('input').val('');
				crtobj.queryEffect('');
			});
			//菜单列表区
			let menuarea = sect.add('section');
		}
	};
	//基础事件，初始化时即执行
	fn._baseEvents = {
		//鼠标滑过事件，整个组件都响应
		mouseover: function (obj) {
			obj.dom.bind('mouseover,mousemove,mousedown', function (event) {
				let crtobj = treemenu._getObj(event);
				if (crtobj == null) return;
				crtobj.leavetime = 3;
				crtobj.leaveshow = true;
			});
			obj.dom.bind('mouseout', function (event) {
				let crtobj = treemenu._getObj(event);
				if (crtobj == null) return;
				crtobj.leavetime = 3;
				crtobj.leaveshow = false;
			});
		},
		//树形菜单的收缩与展开
		fold: function (obj) {
			//左下角折叠按钮的事件
			obj.domtit.find('tree-foldbtn').click(function (event) {
				let crtobj = treemenu._getObj(event);
				if (crtobj == null) return;
				crtobj.fold = !crtobj.fold;
			});
			//当折叠时，鼠标滑过左侧标签后显示主体菜单，过几秒后自动消失
			obj.leavetime = 3;
			obj.dombody.bind('mouseover', function (event) {
				let crtobj = treemenu._getObj(event);
				if (crtobj == null) return;
				crtobj.leavetime = 3;
			});
			obj.leaveInterval = window.setInterval(function () {
				if (obj.fold && !obj.leaveshow && --obj.leavetime <= 0) {
					obj.dombody.width(0);
					obj.dom.removeAttr('expand');
				}
			}, 200);
		},
		//左侧标签点击事件
		rootclick: function (obj) {
			obj.domtit.find('tree_tag[type=item]').click(function (event) {
				let node = event.target ? event.target : event.srcElement;
				//选项卡html元素对象
				let tag = null;
				while (node.tagName.toLowerCase() != 'tree_tag') node = node.parentNode;
				tag = $dom(node);
				//树形菜单控件的对象
				let crtobj = treemenu._getObj(event);
				if (crtobj == null) return;
				//切换选项卡
				crtobj.switch(obj, tag);
			});
			obj.domtit.find('tree_tag[type=item]').bind('mouseover', function (event) {
				let node = event.target ? event.target : event.srcElement;
				//选项卡html元素对象
				let tag = null;
				while (node.tagName.toLowerCase() != 'tree_tag') node = node.parentNode;
				tag = $dom(node);
				//树形菜单控件的对象
				let crtobj = treemenu._getObj(event);
				if (crtobj == null) return;
				//如果菜单不是折叠状态，则滑过事件不响应
				if (!crtobj.fold) return;
				//保持悬念状态
				crtobj.dombody.show().css('z-index', 100).width(crtobj.width - 40);
				crtobj.dom.attr('expand', true);
				crtobj.leavetime = 3;
				crtobj.leaveshow = true;
				crtobj.switch(obj, tag);
			});
			obj.switch(obj, obj.domtit.find('tree_tag[type=item]').first());
		}
	};
	//创建树形节点
	fn._createNode = function (item, box) {
		let node = box.add('tree-node');
		node.css('padding-left', ((item.level - 1) * 15) + 'px');
		if (item.intro) node.attr('title', item.intro);
		//节点类型
		node.attr('type', item.type ? item.type : 'node');
		//图标，图标样式
		let ico = node.add('ico').html('&#x' + (item.ico ? item.ico : 'a022'));
		if (item.icon) {
			if (item.icon.color) ico.css('color', item.icon.color,);
			if (item.icon.x != 0) ico.css('margin-left', item.icon.x + 'px');
			if (item.icon.y != 0) ico.css('margin-top', item.icon.y + 'px');
			if (item.icon.size != 0) ico.css('transform', 'scale(' + (1 + item.icon.size / 100) + ')');
		}
		if (item.type == 'link') {
			let link = node.add('a');
			link.attr('href', item.url).attr('target', item.target ? item.target : '_blank');
		} else {
			node.add('span').html(item.title);
		}
		//完成度	
		if (item.complete < 100 && this.complete) {
			let surplus = 100 - item.complete;
			let color = "#00ca08";
			if (surplus >= 90) color = "#ff0000";
			else if (surplus >= 70) color = "#ff8a49";
			else if (surplus >= 50) color = "#e6a23c";
			else if (surplus >= 30) color = "#127ba0";
			node.add('complete').html(surplus).css('background-color', color);
			surplus = surplus < 20 ? 20 : surplus;
			node.css('border-image', 'linear-gradient( to right,' + color + ' ' + surplus + '%, rgba(255, 255, 255,0) ' + (surplus + 5) + '%, rgba(255, 255, 255,0))');
			node.css('border-bottom', 'solid 2px');
		}
		//字体样式
		if (item.font) {
			let fonts = node;
			if (item.font.color) fonts.css('color', item.font.color, true);
			if (item.font.bold) fonts.css('font-weight', item.font.bold ? 'bold' : 'normal', true);
			if (item.font.italic) fonts.css('font-style', item.font.italic ? 'italic' : 'normal', true);
		}
		//如果有下级节点
		if (item.type != 'node' && item.childs && item.childs.length > 0) {
			node.addClass('folder').click(function (event) {
				let n = event.target ? event.target : event.srcElement;
				while (n.tagName.toLowerCase() != 'tree-node') n = n.parentNode;
				let tnode = $dom(n);
				if (tnode.hasClass('folder')) {
					tnode.attr('class', 'folderclose');
					tnode.siblings('tree_box').hide();
				} else {
					tnode.attr('class', 'folder');
					tnode.siblings('tree_box').show();
				}
			});
		} else {
			if (item.type != 'link') {
				//节点点击事件
				node.click(function (event) {
					let n = event.target ? event.target : event.srcElement;
					while (n.tagName.toLowerCase() != 'tree_box') n = n.parentNode;
					//节点id
					let treeid = $dom(n).attr('treeid');
					//对象
					let ctrobj = treemenu._getObj(event);
					if (ctrobj == null) return;
					let datanode = ctrobj.getData(treeid); //数据源节点
					//触发节点点击事件
					ctrobj.trigger('click', { treeid: treeid, data: datanode });
				});
			}
		}
		return node;
	};
	//计算层深，并补全一些信息
	fn._calcLevel = function (items, level) {
		for (let i = 0; i < items.length; i++) {
			const item = items[i];
			//补全一些信息
			if (!item.id || item.id < 0) item.id = 'node_' + (i + Math.floor(Math.random() * 100000));
			if (!item.pid || item.pid < 0) item.pid = 0;
			if (!item.level || item.level <= 0) item.level = level;
			if (!item.path) item.path = item.title;
			if (!item.ico || item.ico == '') item.ico = 'a009';
			if (!item.tit || item.tit == '') item.tit = item.title;
			if (!item.index) item.index = i;
			if (item.childs && item.childs.length > 0) {
				for (let j = 0; j < item.childs.length; j++) {
					item.childs[j].pid = item.id;
					item.childs[j].path = item.path + ',' + item.childs[j].title;
					item.childs = this._calcLevel(item.childs, level + 1);
				}
			}
		}
		return items;
	};
	//通过节点id,获取数据源的节点项
	fn.getData = function (treeid) {
		if (this.datas.length < 1) return null;
		return $dom.clone(getdata(treeid, this.datas));
		//根据节点id，获取节点对象
		function getdata(treeid, datas) {
			let d = null;
			for (let i = 0; i < datas.length; i++) {
				if (datas[i].id == treeid) return datas[i];
				if (datas[i].childs && datas[i].childs.length > 0)
					d = getdata(treeid, datas[i].childs);
				if (d != null) return d;
			}
			return d;
		}
	};
	//计算节点数量，如果isall为flase,则只取最底一级，节点不计入
	fn.getNodeCount = function (isall, childs) {
		let list = childs == null ? this.datas : childs;
		let count = 0;
		for (let i = 0; i < list.length; i++) {
			if (isall || list[i].childs.length < 1) count++;
			count += this.getNodeCount(isall, list[i].childs);
		}
		return count;
	}
	//查询节点，返回满足条件的节点数组
	//search:查询值
	//isall:sall为flase,则只取最底一级，节点不计入
	fn.queryData = function (search, isall, childs) {
		let list = childs == null ? this.datas : childs;
		let arr = [];
		if (search == null || search == '') return arr;
		search = search.toLowerCase();
		for (let i = 0; i < list.length; i++) {
			const item = list[i];
			if (isall || item.childs.length < 1) {
				if (item.title.toLowerCase().indexOf(search) > -1 ||
					item.tit.toLowerCase().indexOf(search) > -1 ||
					item.intro.toLowerCase().indexOf(search) > -1
				) arr.push(item);
			}
			if (item.childs.length > 0)
				arr = arr.concat(this.queryData(search, isall, item.childs));
		}
		return arr;
	};
	//查询节点后的显示效果
	fn.queryEffect = function (search) {
		//去除之前的高亮显示效果
		this.domquery.find('*[queried]').removeAttr('queried');
		let div = this.domquery.find('section header>div').first();
		div.removeAttr('total');
		this.domquery.find('tree_area tree_tit').removeAttr('count');
		if (search != null) search = search.replace(/^\s*|\s*$/g, '').replace(/^\n+|\n+$/g, "");
		if (search == null || search == '') return this.domquery.find('section header clear').hide();
		//***计算出查询结果
		let list = this.queryData(search, true);
		for (let i = 0; i < list.length; i++) {
			//菜单项（包括菜单节点）
			let treebox = this.domquery.find('tree_box[treeid=' + list[i].id + ']');
			let treenode = treebox.find('tree-node');
			if (treenode.length > 0) treenode.first().attr('queried', true);
			//顶级菜单项（即某一个菜单面板）
			let treearea = this.domquery.find('tree_area[treeid=' + list[i].id + ']');
			let treetit = treearea.find('tree_tit');
			if (treetit.length > 0) treetit.first().attr('queried', true);
		}
		//查询出的结果总数
		let total = list.length;
		div.attr('total', '查询到 ' + total + ' 个菜单项');
		//各个根节点下的查询到的总数
		for (let i = 0; i < this.datas.length; i++) {
			const item = this.datas[i];
			let count = this.queryData(search, true, [item]).length;
			let tit = this.domquery.find('tree_area[treeid=' + item.id + '] tree_tit');
			count > 0 ? tit.attr('count', count) : tit.removeAttr('count');
		}
		//显示清除查询字符的按钮
		this.domquery.find('section header clear').show();
	};
	//切换选项卡
	fn.switch = function (obj, tag) {
		this.domtit.find('tree_tag').removeClass('selected');
		if (tag == null) return;
		tag.addClass('selected');
		this.dombody.childs().hide();
		this.dombody.find('tree_area[treeid=\'' + tag.attr('treeid') + '\']').show();
		//触发左侧选项卡切换事件,参数：数据源节点
		obj.trigger('change', { data: obj.getData(tag.attr('treeid')) });
	};
	//设置当前菜单项的样式，并打开关联的上级菜单
	fn.currentnode = function (treeid) {
		$dom('tree_box').removeClass('current');
		if (treeid == null) return;
		let data = this.getData(treeid);	//获取数据源节点对象
		if (data == null) return;
		//console.log(data);
		//设置当前节点的样式
		let menu = $dom('tree_box[treeid=\'' + treeid + '\']');
		$dom('tree_box').removeClass('current');
		menu.addClass('current');
		//
		data = this.getData(data.pid);
		while (data != null) {
			//上级折叠菜单
			let n = $dom('tree_box[treeid=\'' + data.id + '\']>tree-node.folderclose');
			if (n.length > 0) n.click();
			//根菜单，即最左侧选项卡
			let r = $dom('tree_tag[treeid=\'' + data.id + '\']:not(.selected)');
			if (r.length > 0) r.click();
			data = this.getData(data.pid);
		}
	};
	/*
	treemenu的静态方法
	*/
	treemenu.create = function (param) {
		if (param == null) param = {};
		return new treemenu(param);
	};
	//用于事件中，取点击的组件对象
	treemenu._getObj = function (e) {
		let node = e.target ? e.target : e.srcElement;
		while (!$dom(node).hasClass('treemenu')) node = node.parentNode;
		if (node == null || $dom(node).length < 1) return null;
		let ctrl = $ctrls.get(node.getAttribute('ctrid'));
		if (ctrl == null) return null;
		return ctrl.obj;
	};
	//添加树形的子级节点
	treemenu._add_nodechild = function (area, item, obj) {
		let box = area.add('tree_box');
		box.attr('treeid', item.id);
		obj._createNode(item, box);
		if (item.type != 'node' && item.childs && item.childs.length > 0) {
			for (let i = 0; i < item.childs.length; i++) {
				treemenu._add_nodechild(box, item.childs[i], obj);
			}
		}
	};
	treemenu._initEvent = function () {
		window.addEventListener("resize", function () {
			let treebody = $dom('.treemenu tree_body');
			treebody.height(treebody.parent().height());
		}, false);
	}
	win.$treemenu = treemenu;
	win.$treemenu._initEvent();
})(window);