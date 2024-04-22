import {registerPlugins} from './plugins'
import {createApp} from 'vue'
import "@fontsource/roboto"
import App from "./App.vue";
import "ag-grid-community/styles/ag-grid.css";
import "ag-grid-community/styles/ag-theme-alpine.min.css";


/**
 * Root
 */
const app = createApp(App);

// Add Date Plugin
registerPlugins(app);
app.mount('#app')

