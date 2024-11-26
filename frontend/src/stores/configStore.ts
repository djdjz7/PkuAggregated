import { defineStore } from "pinia";
import { ref } from "vue";

export const useConfigStore = defineStore(
  "config",
  () => {
    const host = ref("");
    const verificationToken = ref("");
    const readEula = ref(false);

    return { host, verificationToken, readEula };
  },
  {
    persist: true,
  }
);
