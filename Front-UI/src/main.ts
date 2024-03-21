/**
 * main.ts
 *
 * Bootstraps Vuetify and other plugins then mounts the App`
 */

// Plugins
import { registerPlugins } from './plugins'
// Components
// Composables
import { createApp } from 'vue'
import router from "./router";
import App from "./App.vue";
const app = createApp(App)
registerPlugins(app)

app
  // 라우터 사용
  .use(router)
  .mount('#app')
