<template>
  <v-main>
    <v-card width="500" class="mx-auto mt-5">
      <v-card-title>Login</v-card-title>
      <v-card-text>
        <form>
          <v-text-field
            v-model="clientId"
            :error-messages="clientIdErrors"
            prepend-icon="mdi-account-circle"
            label="Client ID"
            required
            @input="$v.clientId.$touch()"
            @blur="$v.clientId.$touch()"
          ></v-text-field>
          <v-text-field
            v-model="clientSecret"
            :error-messages="clientSecretErrors"
            label="Client Secret"
            type="password"
            prepend-icon="mdi-lock"
            required
            @input="$v.clientSecret.$touch()"
            @blur="$v.clientSecret.$touch()"
          ></v-text-field>
        </form>
      </v-card-text>
      <v-divider></v-divider>
      <v-card-actions>
        <v-btn class="mr-4" @click="submit">Login</v-btn>
      </v-card-actions>
    </v-card>
  </v-main>
</template>

<script>
import { validationMixin } from "vuelidate";
import { required } from "vuelidate/lib/validators";
import * as authService from "@/services/auth.service";

export default {
  mixins: [validationMixin],
  validations: {
    clientId: { required },
    clientSecret: { required },
  },
  data: () => ({
    clientId: "",
    clientSecret: "",
    cookieId: "",
  }),
  computed: {
    selectErrors() {
      const errors = [];
      if (!this.$v.select.$dirty) return errors;
      !this.$v.select.required && errors.push("Item is required");
      return errors;
    },
    clientIdErrors() {
      const errors = [];
      if (!this.$v.clientId.$dirty) return errors;
      !this.$v.clientId.required && errors.push("Client ID is required.");
      return errors;
    },
    clientSecretErrors() {
      const errors = [];
      if (!this.$v.clientSecret.$dirty) return errors;
      !this.$v.clientSecret.required &&
        errors.push("Client Secret is required.");
      return errors;
    },
  },
  methods: {
    async loginForge(clientSecret, clientId) {
      const accessTokenResult = await authService.getTokenByCode(
        clientId,
        clientSecret
      );
      if (accessTokenResult) {
        window.location.href = accessTokenResult.data;
      }
      console.log(clientSecret);
    },
    submit() {
      this.$v.$touch();
      if (!this.$v.$invalid) {
        // do your submit logic here
        console.log("yes");
        this.loginForge(this.clientSecret, this.clientId);
      }
    },
  },
};


</script>
