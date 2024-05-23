import { createApp } from 'vue'
import { createPinia } from 'pinia'
import PrimeVue from 'primevue/config';

import App from './App.vue'
import router from './router'
import 'primevue/resources/themes/aura-light-blue/theme.css'
import '@mdi/font/css/materialdesignicons.css'
import 'primeflex/primeflex.min.css'
import 'primeicons/primeicons.css'
import Button from 'primevue/button'
import Row from 'primevue/row'
import Column from 'primevue/column'
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'
import InputGroup from 'primevue/inputgroup';
import InputGroupAddon from 'primevue/inputgroupaddon';


const app = createApp(App)

app.use(createPinia())
// eslint-disable-next-line vue/multi-word-component-names,vue/no-reserved-component-names
app.component('Button', Button);
// eslint-disable-next-line vue/multi-word-component-names
app.component('Password', Password);
app.component('p-row', Row);
app.component('p-column', Column);
app.component('InputText', InputText);
app.component('InputGroup', InputGroup);
app.component('InputGroupAddon', InputGroupAddon);

app.use(PrimeVue);
app.use(router)

app.mount('#app')
