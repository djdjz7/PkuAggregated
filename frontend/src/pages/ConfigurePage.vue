<script setup lang="ts">
import { useRouter } from "vue-router";
import { useConfigStore } from "../stores/configStore";
import { storeToRefs } from "pinia";
const { host, verificationToken } = storeToRefs(useConfigStore());
const router = useRouter();

function save() {
  router.back();
}
</script>

<template>
  <div flex="~ col gap-2" w-40em mx-auto style="max-width: calc(100vw - 2em);">
    <input v-model="host" placeholder="Host" />
    <input v-model="verificationToken" placeholder="Verification Token" />
    <button @click="save">OK</button>
  </div>
</template>
