<script setup lang="ts">
import { LinkIcon, PhotoIcon } from "@heroicons/vue/24/outline";
import {
  TreeholeSearchResultItem,
  TreeholeSearchResult,
} from "../models/TreeholeSearchResult";
import Expander from "./Expander.vue";
import { inject, onMounted } from "vue";
import SmsVerifyComponent from "./SmsVerifyComponent.vue";

let openDetailsElement: (hole: TreeholeSearchResultItem) => void;
onMounted(() => {
  openDetailsElement = inject("openDetails") as (
    pid: TreeholeSearchResultItem
  ) => void;
});
defineProps<{
  result: TreeholeSearchResult | undefined;
}>();

function openDetails(hole: TreeholeSearchResultItem) {
  openDetailsElement(hole);
}
</script>

<template>
  <Expander>
    <template #header>
      <a
        flex="~ items-center"
        text-unset
        :href="result?.sourceInfo.url"
        id="treehole"
        decoration-none
        target="_blank"
        ><span>{{ result?.sourceInfo.name }}</span
        ><LinkIcon class="h-4 w-4 m-l-2"
      /></a>
    </template>

    <div v-if="!result?.isSuccess" p-x-4 p-y-1>
      <SmsVerifyComponent v-if="result?.errorMessage == 'NEED_SMS_VERIFY'" />
      <div v-else text-gray-500 dark:text-gray-400>
        <span>未能成功完成搜索</span><br /><span
          >错误消息：{{ result?.errorMessage }}</span
        >
      </div>
    </div>
    <span p-x-4 p-y-1 text-gray-500 dark:text-gray-400 v-else-if="result.results.length === 0"
      >无匹配结果</span
    >
    <div
      v-else
      p-x-4
      p-y-1
      class="hover:bg-gray/20"
      transition-all
      duration-200
      text-unset
      decoration-none
      v-for="item in result.results"
      flex="~ col items-stretch"
      cursor-pointer
      @click="openDetails(item)"
    >
      <div flex="~ items-center" gap-2 font-300>
        <span>#{{ item.pid }}</span>
        <span text-gray-500 dark:text-gray-400 text-sm>{{
          new Date(item.time).toLocaleString()
        }}</span>
      </div>
      <p whitespace-pre-wrap text-wrap m-y-0>{{ item.text }}</p>
      <div
        v-if="item.imageId != undefined"
        flex="~ items-center"
        text-gray-500 dark:text-gray-400
        m-t-2
      >
        <PhotoIcon class="h-5 w-5" />
        <span m-l-2>图片附件</span>
      </div>
    </div>
    <div h-3></div>
  </Expander>
</template>
