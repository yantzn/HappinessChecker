<template>
     <v-snackbar
        v-model="snackbar"
        :bottom="y === 'bottom'"
        :color="color"
        :left="x === 'left'"
        :multi-line="mode === 'multi-line'"
        :right="x === 'right'"
        :timeout="timeout"
        :top="y === 'top'"
        :vertical="mode === 'vertical'"
      >
        {{ text }}
        <v-btn
          dark
          text
          @click="snackbar = false"
        >
          Close
        </v-btn>
      </v-snackbar>

</template>

<script>
export default {
  name: 'ErrorAlert',
  props: ['snackbar'],
  data () {
    return {
      color: '#ff6363',
      mode: 'vertical',
      text: 'ダウンロードできませんでした。時間を空けてお試しください。',
      timeout: 3000,
      x: null,
      y: 'bottom'
    }
  }
}
</script>
