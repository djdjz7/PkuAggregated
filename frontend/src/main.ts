import { createApp } from "vue";
import "./style.css";
import App from "./App.vue";
import "virtual:uno.css";
import { createWebHistory, createRouter } from "vue-router";
import { createPinia } from "pinia";
import piniaPluginPersistedstate from "pinia-plugin-persistedstate";

const routes = [
  {
    path: "/",
    component: () => import("./pages/MainPage.vue"),
  },
  {
    path: "/config",
    component: () => import("./pages/ConfigurePage.vue"),
  },
  {
    path: "/eula",
    component: () => import("./pages/EulaPage.vue"),
  },
  {
    path: "/faq",
    component: () => import("./pages/FaqPage.vue"),
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

const pinia = createPinia();
pinia.use(piniaPluginPersistedstate);

createApp(App).use(router).use(pinia).mount("#app");
