<script src="https://unpkg.com/vue@2.5.17"></script>
<template>
  <div id="app">
    <v-app id="inspire">
      <Header :displaycnt="this.sum_pict"/>
      <Happiness @sumpicter="getSum"/>
      <Footer/>
    </v-app>
  </div>
</template>

<script>
import Header from '@/components/Header.vue'
import Happiness from '@/components/Happiness'
import Footer from '@/components/Footer.vue'

export default {
  name: 'App',
  components : {
    Header,
    Happiness,
    Footer
  },
  data() {
    return {
      sum_pict: 0
    }
  },
  methods: {
    getSum(val) {
      this.sum_pict = val
    }
  }
}
</script>

<style>
body{
  background-color:#EEEEEE;
}
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
</style>
