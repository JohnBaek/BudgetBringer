
import { registerPlugins } from './plugins'
import { createApp } from 'vue'
import router from "./router";
import App from "./App.vue";
import {createPinia} from "pinia";
const app = createApp(App)
registerPlugins(app)
const pinia = createPinia();

app
  // 라우터 사용
  .use(router)
  // Pinia 상태관리 사용
  .use(pinia)
  .mount('#app')
