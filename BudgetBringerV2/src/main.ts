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
import { logger } from '@/services/Logger';
import Toast from 'primevue/toast';
import ToastService from 'primevue/toastservice';
import Menu from 'primevue/menu'
import Sidebar from 'primevue/sidebar'
import Menubar from 'primevue/menubar'
import Card from 'primevue/card'
import Panel from 'primevue/panel'
import Breadcrumb from 'primevue/breadcrumb'
import Divider from 'primevue/divider'
import ConfirmationService from 'primevue/confirmationservice';
import Dialog from 'primevue/dialog';
import DynamicDialog from 'primevue/dynamicdialog';
import DialogService from 'primevue/dialogservice'
import ConfirmDialog from 'primevue/confirmdialog'
import ConfirmPopup from 'primevue/confirmpopup'
import DataTable from 'primevue/datatable'
import ColumnGroup from 'primevue/columngroup'
import Skeleton from 'primevue/skeleton'
import MultiSelect from 'primevue/multiselect'
import Tag from 'primevue/tag'

const app = createApp(App);

// console.log overwriting
console.log = logger.log;
console.warn = logger.warn;
console.error = logger.error;

// eslint-disable-next-line vue/multi-word-component-names,vue/no-reserved-component-names
app.component('Button', Button);
// eslint-disable-next-line vue/multi-word-component-names
app.component('Password', Password);
// eslint-disable-next-line vue/multi-word-component-names,vue/no-reserved-component-names
app.component('Menu', Menu);
// eslint-disable-next-line vue/multi-word-component-names
app.component('Toast', Toast);
// eslint-disable-next-line vue/multi-word-component-names
app.component('Sidebar', Sidebar);
// eslint-disable-next-line vue/multi-word-component-names,vue/no-reserved-component-names
app.component('Menubar', Menubar);
// eslint-disable-next-line vue/multi-word-component-names,vue/no-reserved-component-names
app.component('Card', Card);
// eslint-disable-next-line vue/multi-word-component-names,vue/no-reserved-component-names
app.component('Panel', Panel);
// eslint-disable-next-line vue/multi-word-component-names,vue/no-reserved-component-names
app.component('Breadcrumb', Breadcrumb);
// eslint-disable-next-line vue/multi-word-component-names,vue/no-reserved-component-names
app.component('Divider', Divider);
app.component('p-row', Row);
app.component('p-column', Column);
app.component('InputText', InputText);
app.component('InputGroup', InputGroup);
app.component('InputGroupAddon', InputGroupAddon);
app.component('ConfirmDialog', ConfirmDialog);
// eslint-disable-next-line vue/multi-word-component-names,vue/no-reserved-component-names
app.component('Dialog', Dialog);
app.component('DynamicDialog', DynamicDialog);
app.component('ConfirmPopup', ConfirmPopup);
app.component('DataTable',DataTable);
app.component('MultiSelect',MultiSelect);
// eslint-disable-next-line vue/multi-word-component-names,vue/no-reserved-component-names
app.component('Column',Column);
app.component('ColumnGroup',ColumnGroup);
// eslint-disable-next-line vue/multi-word-component-names,vue/no-reserved-component-names
app.component('Row',Row);
// eslint-disable-next-line vue/multi-word-component-names,vue/no-reserved-component-names
app.component('Skeleton',Skeleton);
// eslint-disable-next-line vue/multi-word-component-names,vue/no-reserved-component-names
app.component('Tag',Tag);



app.use(createPinia());
app.use(PrimeVue,  { ripple: true  });
app.use(ConfirmationService);
app.use(DialogService);
app.use(router);
app.use(ToastService);

app.mount('#app');
