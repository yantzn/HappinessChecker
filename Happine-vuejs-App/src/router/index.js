import Vue from 'vue'
import Router from 'vue-router'
import Happiness from '@/components/Happiness'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Happiness',
      component: Happiness
    }
  ]
})
