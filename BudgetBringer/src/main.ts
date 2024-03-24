import { registerPlugins } from './plugins'
import { createApp } from 'vue'
import "@fontsource/roboto"
import App from "./App.vue";
import "ag-grid-community/styles/ag-grid.css";
import "ag-grid-community/styles/ag-theme-quartz.css";

/**
 * Root 앱
 */
const app = createApp(App);

// 플러그인을 등록한다.
registerPlugins(app);
app.mount('#app')

