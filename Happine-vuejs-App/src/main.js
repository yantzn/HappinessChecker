// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import BootstrapVue from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import VueLazyload from 'vue-lazyload'
import VueAppInsights from 'vue-application-insights'

import Vuetify from 'vuetify'
import 'vuetify/dist/vuetify.min.css'
import 'material-design-icons-iconfont/dist/material-design-icons.css'
import '@mdi/font/css/materialdesignicons.css'

Vue.use(BootstrapVue)
Vue.config.productionTip = false
Vue.use(VueLazyload)
Vue.use(VueLazyload, {
  preLoad: 1.3,
  error: 'https://dummyimage.com/130x120/ccc/999.png&text=Not+Found',
  loading: './assets/logo.png',
  attempt: 1
})

Vue.use(VueAppInsights, {
  id: 'e1f52e28-554a-4909-ae7d-7a9034d1ec5c',
  router,
  IConfig: {
    enableAutoRouteTracking: true,
    disableAjaxTracking: false,
    disableTelemetry: false,
    disableExceptionTracking: false
  }
})

Vue.use(Vuetify)

/* eslint-disable no-new */
new Vue({
  el: '#app',
  vuetify: new Vuetify(),
  router,
  components: { App },
  template: '<App/>'
})
