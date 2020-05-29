<template>
  <div>
  <v-btn icon v-on:click="download">
    <v-icon>mdi-arrow-collapse-down</v-icon>
  </v-btn>
  </div>

</template>

<script>
import ErrorAlert from '@/components/ErrorAlert.vue'
import axios from 'axios'
export default {
  name: 'DownloadImage',
  props: ['getImage'],
  components: {
    ErrorAlert
  },
  data () {
    return {
      fileName: ''
    }
  },
  methods: {
    download: function () {
      axios.get(this.getImage, {
        dataType: 'binary',
        responseType: 'blob'
      })
        .then(response => {
          if (window.navigator.msSaveOrOpenBlob) {
            window.navigator.msSaveBlob(new Blob([response.data]), this.fileName)
          } else {
            const resurl = window.URL.createObjectURL(new Blob([response.data], {
              type:
                'image/jpeg'
            }))
            const link = document.createElement('a')
            link.href = resurl
            link.setAttribute('download', this.fileName)
            document.body.appendChild(link)
            link.click()
            document.body.removeChild(link)
          }
          console.log(response)
        })
        .catch(error => {
          this.$emit('alert', true)
          console.log(error)
        })
    }
  }
}
</script>
