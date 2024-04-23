import {registerPlugins} from './plugins'
import {createApp} from 'vue'
import "@fontsource/roboto"
import App from "./App.vue";
import "ag-grid-community/styles/ag-grid.css";
import "ag-grid-community/styles/ag-theme-alpine.min.css";
import "vue3-loading-skeleton/dist/style.css";
import {SkeletonLoader} from "vue3-loading-skeleton";


/**
 * Root
 */
const app = createApp(App).component("SkeletonLoader", SkeletonLoader);

// Add Date Plugin
registerPlugins(app);
app.mount('#app')

