import { createApp } from 'vue'
import App from './App.vue'
import Vant, {
  Toast,
  Notify,
  Dialog
} from 'vant';
import 'vant/lib/index.css';
import store from './store'
import router from './router'
import IMBSProxy from './services/IMBSProxy.js';


// ���� rem ����
function FixWindowRem() {
  // ���ֻ�׼��С����λpx��
  const BaseFontSize = 32
  // Ӧ����ƻ�׼���
  const Width = 750
  const Height = 1334
  var scale;
  // ��Ļ���
  var width = document.documentElement.clientWidth || document.body.clientWidth;
  var height = document.documentElement.clientHeight || document.body.clientHeight;
  if ((width / height) < (Width / Height)) {
    // ����
    scale = width / Width
  } else {
    // ����
    scale = height / Height
  }
  // ����ҳ����ڵ������С
  document.getElementsByTagName('html').item(0).style.fontSize = BaseFontSize * scale + 'px'
}
// ��һ�γ�ʼ������
FixWindowRem()
// �ı䴰�ڴ�Сʱ��������
window.onresize = FixWindowRem

var vue = createApp(App)
vue.config.globalProperties.$Toast = Toast;
vue.config.globalProperties.$Notify = Notify;
vue.config.globalProperties.$Dialog = Dialog;
vue.config.globalProperties.$IMBSProxy = IMBSProxy;
vue
  // ui���
  .use(Vant)
  // Vuex
  .use(store)
  // router
  .use(router)
  .mount('#app')