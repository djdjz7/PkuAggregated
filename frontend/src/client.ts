import axios from "axios";
import { storeToRefs } from "pinia";
import { useConfigStore } from "./stores/configStore";
import { watch } from "vue";
import CryptoJS from "crypto-js";

const configStore = useConfigStore();
const { host, verificationToken } = storeToRefs(configStore);

export const client = axios.create({
  baseURL: host.value,
});

watch(host, (newValue, _) => {
  client.defaults.baseURL = newValue;
});

client.interceptors.request.use((x) => {
  const date = new Date();
  x.headers["X-Private-Verification"] = calculateVerification(date);
  x.headers["X-Private-Token-Gen-Time"] = date.toISOString();
  return x;
});

function calculateVerification(date: Date): string {
  let dateStr = `${date.getFullYear()}${(date.getMonth() + 1)
    .toString()
    .padStart(2, "0")}${date.getDate().toString().padStart(2, "0")}${date
    .getHours()
    .toString()
    .padStart(2, "0")}${date.getMinutes().toString().padStart(2, "0")}${date
    .getSeconds()
    .toString()
    .padStart(2, "0")}`;
  let combinedStr = dateStr + verificationToken.value;
  return CryptoJS.SHA256(combinedStr).toString().toUpperCase();
}
