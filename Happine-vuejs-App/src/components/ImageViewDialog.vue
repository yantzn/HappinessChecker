<template>
  <v-dialog v-model="dialog" fullscreen hide-overlay transition="dialog-bottom-transition">
    <template v-slot:activator="{ on }">
      <v-btn text color="deep-purple accent-4" v-on="on" >写真を見る</v-btn>
    </template>
    <v-card >
      <v-toolbar color="indigo" dark dense>
        <v-btn icon dark @click="dialog = false">
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </v-toolbar>
      <v-img :src="this.getImage"></v-img>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  props: ['getImage'],
  data () {
    return {
      dialog: false
    }
  }
}
</script>
