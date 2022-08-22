import Vue from "vue";
import App from "./App.vue";
import vuetify from "./plugins/vuetify";
import VueRouter from "vue-router";
import VueHome from "./components/app-home.vue";
import VueLogin from "./components/app-login.vue";
import VueFileViewer from "./components/app-fileview.vue";

Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    component: VueHome,
  },
  {
    path: "/login",
    component: VueLogin,
  },
  {
    path: "/fileview",
    component: VueFileViewer,
  },
];

const router = new VueRouter({
  routes,
  mode: "history",
});

Vue.config.productionTip = false;

new Vue({
  vuetify,
  router,
  render: (h) => h(App),
}).$mount("#app");
