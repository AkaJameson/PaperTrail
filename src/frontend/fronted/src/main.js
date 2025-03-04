import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import router from './router'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import HttpClient from './common/utils/httpclient'
import * as ElementPlusIconsVue from '@element-plus/icons-vue'
const app = createApp(App)
const httpclient = new HttpClient(import.meta.env.VITE_APP_BASE_API_ADDRESS);
app.use(createPinia())
app.use(router)
app.use(ElementPlus)
app.provide("httpclient",httpclient)
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component)
  }
app.mount('#app')
