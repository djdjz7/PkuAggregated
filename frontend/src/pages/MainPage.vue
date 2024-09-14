<script setup lang="ts">
import { onMounted, provide, ref } from "vue";
import { useConfigStore } from "../stores/configStore";
import { storeToRefs } from "pinia";
import { useRouter } from "vue-router";
import { Cog6ToothIcon, MagnifyingGlassIcon } from "@heroicons/vue/24/outline";
import { client } from "../client";
import { SearchResult } from "../models/SearchResult";
import SearchSourceResult from "../components/SearchSourceResult.vue";
import TreeholeSearchSourceResult from "../components/TreeholeSearchSourceResult.vue";
import { TreeholeSearchResult } from "../models/TreeholeSearchResult";
import TreeholeDetails from "../components/TreeholeDetails.vue";

const router = useRouter();

const { host, verificationToken, readEula } = storeToRefs(useConfigStore());
const content = ref("");
const treeholeResults = ref<TreeholeSearchResult | undefined>();
const otherResults = ref<SearchResult[]>([]);
const detailsElementRef = ref<typeof TreeholeDetails>();

async function search() {
  const treeholePromise = client.get<TreeholeSearchResult>(
    `api/search/treehole?keyword=${encodeURIComponent(content.value)}`
  );
  const otherPromise = client.get<SearchResult[]>(
    `api/search?keyword=${encodeURIComponent(content.value)}`
  );

  const [treehole, others] = await Promise.all([treeholePromise, otherPromise]);

  treeholeResults.value = treehole.data;
  otherResults.value = others.data;
}

function openConfig() {
  router.push("/config");
}

onMounted(() => {
  if (!readEula.value) {
    router.push("/eula");
    return;
  }
  if (!Boolean(verificationToken.value) || !Boolean(host.value)) {
    alert("未配置主机与验证令牌。将重定向至配置页面。");
    router.push("/config");
    return;
  }

  const openDetails = detailsElementRef.value?.toggleCollapsed;

  provide("openDetails", openDetails);
});
</script>

<template>
  <div flex="~ col" flex-shrink flex-grow>
    <div flex="~">
      <input
        flex-grow
        w-1
        flex-shrink
        rounded="lt-lg lb-lg rt-none rb-none"
        text-lg
        type="text"
        v-model="content"
        @keydown.enter="search"
        placeholder="搜索内容..."
      />
      <button
        rounded="lt-none lb-none rt-lg rb-lg"
        w-12
        h-12
        flex="~ items-center justify-center"
        @click="search"
      >
        <MagnifyingGlassIcon class="h-5 w-5" />
      </button>
      <button
        m-l-2
        w-12
        h-12
        flex="~ items-center justify-center"
        @click="openConfig"
      >
        <Cog6ToothIcon class="h-5 w-5" />
      </button>
    </div>
    <div
      self-stretch
      gap-2
      flex="~ col"
      m-t-2
      flex-grow
      flex-shrink
      overflow-auto
    >
      <TreeholeSearchSourceResult
        v-if="treeholeResults"
        :result="treeholeResults"
      />
      <SearchSourceResult v-for="result in otherResults" :result="result" />
    </div>
    <TreeholeDetails ref="detailsElementRef" />
  </div>
</template>
