<script setup lang="ts">
import { LinkIcon } from "@heroicons/vue/24/outline";
import { SearchResult } from "../models/SearchResult";
import Expander from "./Expander.vue";

defineProps<{
  result: SearchResult;
}>();
</script>

<template>
  <Expander rounded-lg class="bg-gray/10" flex="~ col items-stretch" text-left overflow-clip>
    <template #header>
      <a flex="~ items-center" :id="result.sourceInfo.id" text-unset :href="result?.sourceInfo.url" decoration-none
        target="_blank"><span>{{ result?.sourceInfo.name }}</span>
        <LinkIcon class="h-4 w-4 m-l-2" />
      </a>
    </template>
    <div v-if="!result.isSuccess" p-x-4 p-y-1 text-gray-500 dark:text-gray-400>
      <span>未能成功完成搜索</span><br /><span>错误消息：{{ result.errorMessage }}</span>
    </div>
    <span p-x-4 p-y-1 text-gray-500 v-else-if="result.results.length === 0">无匹配结果</span>
    <a v-else p-x-4 p-y-1 class="hover:bg-gray/20" transition-all duration-200 text-unset decoration-none
      v-for="item in result.results" flex="~ col items-stretch" :href="item.url" target="_blank">
      <span text-ellipsis whitespace-nowrap overflow-hidden>{{
        item.title
      }}</span>

      <span text-gray-500 dark:text-gray-400 text-sm whitespace-pre-wrap>{{ item.description }}</span>
    </a>
    <div h-2></div>
  </Expander>
</template>
