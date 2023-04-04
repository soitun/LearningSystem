﻿
//友情链接
$dom.load.css([$dom.path() + 'Components/Styles/links.css']);
//友情链接的分类
Vue.component('linksorts', {
    //友情链接分类，
    //sort:分类id,如果为空，取所有指定个数(count)
    //count:取多少个分类  
    props: ["org", "sort", 'count'],
    data: function () {
        return {
            sorts: []           //链接分类         
        }
    },
    watch: {
        'org': {
            handler: function (nv, ov) {
                if (JSON.stringify(nv) != '{}' && nv != null)
                    this.getsort();
            }, immediate: true
        }
    },
    computed: {    },
    mounted: function () {},
    methods: {
        //获取链接分类
        getsort: function () {
            var th = this;
            if (th.sort != null) {
                $api.post('Link/SortForID', { 'id': th.sort }).then(function (req) {
                    if (req.data.success) {
                        th.$set(th.sorts, 0, req.data.result);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            } else {
                $api.post('Link/SortCount', { 'orgid': th.org.Org_ID, 'use': true, 'show': true, 'search': '', 'count': th.count })
                    .then(function (req) {
                        if (req.data.success) {
                            th.sorts = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {                   
                        console.error(err);
                    });
            }
        }
    },

    template: `<weisha class="linksorts" v-if="sorts.length>0">
        <slot name="title"></slot>  
        <div class="links">   
            <links :sort="ls" v-for="(ls,i) in sorts">
                <template slot="sortname">
                    <slot name="sortname" :sort="ls"></slot>
                </template>  
            </links>
        </div>
    </weisha>`
});
//友情链接列表
Vue.component('links', {
    //sort:友情链接分类，  
    //count:取多少个链接
    props: ["sort", 'count'],
    data: function () {
        return {         
            datas: [],
            loading:false
        }
    },
    watch: {
        'sort': {
            handler: function (nv, ov) {
                if (JSON.stringify(nv) != '{}' && nv != null)
                    this.getdata();
            }, immediate: true
        }
    },
    computed: {    },
    mounted: function () {},
    methods: {
        //获取数据
        getdata: function () {
            var th = this;
            if (!(!!this.sort.Ls_Id)) return;
            $api.cache('Link/Count',
                { 'orgid': -1, 'sortid': th.sort.Ls_Id, 'use': true, 'show': true, 'search': '', 'count': th.count })
                .then(function (req) {
                    if (req.data.success) {
                        th.datas = req.data.result;   
                                      
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
        }
    },

    template: `<weisha class="links" v-if="datas.length>0">
        <slot name="sortname"></slot>      
        <div class="link-item" v-for="d in datas">
            <template v-if="sort.Ls_IsImg">
                <a :href="d.Lk_Url" target="_blank"  v-if="d.Lk_Logo!=''">
                    <img :src="d.Lk_Logo"/>
                </a>
                <a :href="d.Lk_Url" target="_blank"  v-if="sort.Ls_IsText">
                    {{d.Lk_Name}}
                </a>
            </template>
            <a :href="d.Lk_Url" target="_blank"  v-else>
                {{d.Lk_Name}}
            </a>
        </div>           
    </weisha>`
});



