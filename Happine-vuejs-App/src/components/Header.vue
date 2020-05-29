<template>
  <v-card class="hedder" color="grey lighten-4" flat tile>
    <v-app-bar color="indigo" dark dense>
      <v-spacer></v-spacer>
      <v-toolbar-title>Happiness Point</v-toolbar-title>
      <div>
        <template>
          <div class="sum-picter">
            <v-badge>
              <template v-slot:badge>{{ displaycnt }}</template>
              <v-icon>mdi-camera</v-icon>
            </v-badge>
          </div>
        </template>
        </div>
      <v-spacer></v-spacer>
        <v-btn icon v-on:click="reload">
          <v-icon>mdi-autorenew</v-icon>
        </v-btn>
    </v-app-bar>
 </v-card>
</template>

<script>
export default {
  props: ['displaycnt'],
  methods: {
    reload () {
      location.reload()
    }
  }
}

</script>
<style>
.hedder {
  padding-bottom: 2em;
}

.sum-picter{
  padding-left: 1em;
}
</style>
