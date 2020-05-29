<template>
    <div>
      <span class="subheading mr-2">{{this.getCount}}</span>
      <v-btn icon v-on:click="addPlus">
        <v-icon v-show="display" color="pink accent-2grey">mdi-heart</v-icon>
        <v-icon v-show="!display" color="grey lighten-2">mdi-heart</v-icon>
      </v-btn>
    </div>
</template>

<script>
import axios from 'axios'

export default {
  name: 'GoodCounter',
  props: ['getCount', 'picture_id'],
  data () {
    return {
      id: '',
      display: false
    }
  },
  created () {
    this.id = this.picture_id
  },
  methods: {
    addPlus: function () {
      this.display = !this.display
      const url = process.env.VUE_APP_HOST + '/v1/good'
      axios.post(url, {id: this.id})
        .then(res => {
          console.log(res)
        })
        .catch(res => {
          console.log(res)
        })
    }
  },
  watch: {
    display: function () {
      setTimeout(() => { this.display = false }, 200)
    }
  }
}
</script>
