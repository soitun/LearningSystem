﻿// 学员的头像

Vue.component('avatar', {
    //account: 学员账号对象
    //path:头像图片在服务端的路径
    //circle: 是否为圆形(true)，默认是方形(false)
    //size: 宽高值
    props: ['account', 'path', 'circle', 'size'],
    data: function () {
        return {
            //默认图片
            def: {
                'man': '/Utilities/Images/head1.jpg',
                'woman': '/Utilities/Images/head2.jpg'
            },
            loading: false
        }
    },
    watch: {
        account: function (nv, ov) {
            //if (nv) this.init = true;
        }
    },
    computed: {
        //是否为圆形
        'circle_val': function () {
            let type = $api.getType(this.circle);
            if (type == 'Boolean') return this.circle;
            if (type == 'String')
                return this.circle == 'true' ? true : false;
            return this.circle;
        },
        //学员对象是否存在
        'exist': t => !$api.isnull(t.account)

    },
    created: function () {
        $dom.load.css(['/Utilities/Components/Styles/avatar.css']);
    },
    methods: {
        //头像的url路径
        photourl: function (photo) {
            if (photo == null || photo == '') return '';
            if (this.path != null && this.path != '') return this.path + photo;
            return photo;
        }
    },
    template: `<div :class="{'ws_avatar':true,'ws_circle':circle_val}" :style="{width:size+'px',height:size+'px'}">
        <div v-if="loading">loading...</div>
        <template v-if='exist'>
            <div v-if="account.Ac_Photo!=''" :style="'background:url('+photourl(account.Ac_Photo)+') no-repeat center'" class="ws_avatar_photo" ></div>
            <img v-else :src="account.Ac_Sex==2 ? def.woman : def.man"/>    
        </template>         
    </div>`
});
