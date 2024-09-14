import axios from "axios";
import { storeToRefs } from "pinia";
import { useConfigStore } from "./stores/configStore";
import { watch } from "vue";

const configStore = useConfigStore();
const { host, verificationToken } = storeToRefs(configStore);

export const client = axios.create({
  baseURL: host.value,
  headers: {
    "X-Private-Verification": verificationToken.value,
  },
});

watch(host, (newValue, _) => {
  console.log("host changed to", newValue);
  client.defaults.baseURL = newValue;
});

watch(verificationToken, (newValue, _) => {
  console.log("verificationToken changed to", newValue);
  client.defaults.headers["X-Private-Verification"] = newValue;
});
