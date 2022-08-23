<template>
  <v-main>
    <v-card width="500" class="mx-auto mt-5">
      <h1>Hubs</h1>
      <v-card-actions>
        <v-btn class="mr-4" @click="getHubs">Hubs</v-btn>
      </v-card-actions>
    </v-card>
  </v-main>
</template>

<script>
import * as forgeService from "@/services/forge.service";

export default {
  methods: {
    async getHubs() {
      const hubsresponse = await forgeService.getHubs();
      console.log(hubsresponse);
    },
  },
};
</script>
