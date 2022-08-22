<template>
  <v-app>
    <page-header></page-header>
    <router-view></router-view>
  </v-app>
</template>

<script>
import VueHeader from "./components/app-header.vue";

export default {

  components: {
    "page-header": VueHeader,
  },
};
</script>
