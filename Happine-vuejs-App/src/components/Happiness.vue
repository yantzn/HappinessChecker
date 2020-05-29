<template>
  <v-content>
    <strong v-if="errored" class="text-danger">An error has occurred. Please reload.</strong>

    <div v-else>
      <div v-show="isLoading" class="loader">
        <div class="text-center">
          <strong class="text-info">Loading...</strong>
        </div>
        <div class="text-xs-center">
          <v-progress-circular indeterminate color="primary"></v-progress-circular>
        </div>
      </div>

      <v-container pa-1>
        <v-row no-gutters v-show="!isLoading">
          <v-col v-for="item in items" v-bind:key="item.rank" md="6" lg="4">
            <v-card max-width="95%" class=" ma-2 pa-1">
              <v-list-item>
                <v-list-item-avatar>
                  <v-img :src="item.user_icon"></v-img>
                </v-list-item-avatar>

                <v-list-item-content>
                  <v-list-item-title class="headline">{{item.score}} pt</v-list-item-title>
                </v-list-item-content>
                <v-chip color="pink" label text-color="white">{{item.rank}}</v-chip>
              </v-list-item>

              <v-img :src="item.picture_Url"></v-img>

              <v-list-item-content>
                <v-list-item-title>Phot by {{item.user}}</v-list-item-title>
              </v-list-item-content>

              <v-card-actions>
                <ImageViewDialog v-bind:getImage="item.picture_Url"/>
                <v-spacer></v-spacer>
                <GoodCounter v-bind:getCount="item.good_cnt" :picture_id="item.image_id"/>
                <DownloadImage v-bind:getImage="item.picture_Url" @alert="downloadImageError" />
              </v-card-actions>
            </v-card>
          </v-col>
        </v-row>
      </v-container>
    </div>
    <ErrorAlert v-bind:snackbar="display"/>
  </v-content>
</template>

<script>
import GoodCounter from '@/components/GoodCounter.vue'
import DownloadImage from '@/components/DownloadImage.vue'
import ImageViewDialog from '@/components/ImageViewDialog.vue'
import ErrorAlert from '@/components/ErrorAlert.vue'
import axios from 'axios'
import * as signalr from '@aspnet/signalr'

export default {
  name: 'Happiness',
  components: {
    GoodCounter,
    DownloadImage,
    ImageViewDialog,
    ErrorAlert
  },
  data () {
    return {
      items: [],
      isLoading: true,
      sumcnt: 0,
      errored: false,
      display: false
    }
  },
  created () {
    console.log(process.env.VUE_APP_HOST)
    axios
      .get(process.env.VUE_APP_HOST + '/v1/rank')
      .then(response => {
        this.items = response.data.images
        this.isLoading = false
        this.sumcnt = response.data.images.length
        this.$emit('sumpicter', this.sumcnt)
        console.log(response.data.images)
      })
      .catch(error => {
        this.errored = true
        this.$appInsights.trackException('error.message')
        console.log(error)
      })
  },
  mounted () {
    // SignalRとコネクションを作成
    this.connection = new signalr.HubConnectionBuilder()
      .withUrl(process.env.VUE_APP_HOST)
      .configureLogging(signalr.LogLevel.Information)
      .build()
    console.log('connecting...')

    // SignalR Serviceへの接続
    this.connection
      .start()
      .then(() => console.log('connected!'))
      .catch(console.error)

    // SignalR Serviceへの接続
    this.connection.on('newMessage', (data) => {
      this.items = JSON.parse(data).images
      this.sumcnt = JSON.parse(data).images.length
      this.isLoading = false
      this.errored = false
      this.$emit('sumpicter', this.sumcnt)
      console.log(this.sumcnt)
      console.log(this.items)
    })
    // 切断
    this.connection.onclose(() => console.log('disconnected'))
  },
  methods: {
    downloadImageError: function (val) {
      this.display = val
    }
  },
  watch: {
    display: function () {
      setTimeout(() => { this.display = false }, 5000)
    }
  }
}

</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
.v-progress-circular{
  margin: 1rem
}
</style>
