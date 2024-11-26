<script setup lang="ts">
import { RouterView, useRouter } from "vue-router";
import GlowEffect from "./components/GlowEffect.vue";
import { QuestionMarkCircleIcon } from "@heroicons/vue/24/outline";

const router = useRouter();

function openFaq() {
  router.push("/faq");
}
</script>

<template>
  <GlowEffect />
  <div flex="~ items-center justify-center" class="group">
    <h1
      font-200
      class="translate-x-5 group-hover:translate-x-0 transition-all duration-300"
    >
      PKU Aggregated
    </h1>
    <button
      m-l-2
      w-8
      h-8
      bg-transparent
      p-unset
      rounded-full
      class="-translate-x-1 opacity-0 group-hover:opacity-100 group-hover:translate-x-0 transition-all duration-300"
      flex="~ items-center justify-center" @click="openFaq"
    >
      <QuestionMarkCircleIcon class="h-6 w-6 stroke-1px" />
    </button>
  </div>
  <RouterView flex-shrink flex-grow overflow-y-auto />
</template>
