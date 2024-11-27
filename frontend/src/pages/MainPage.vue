<script setup lang="ts">
import { onMounted, provide, ref } from "vue";
import { useConfigStore } from "../stores/configStore";
import { storeToRefs } from "pinia";
import { useRouter } from "vue-router";
import { ChevronRightIcon, Cog6ToothIcon, MagnifyingGlassIcon } from "@heroicons/vue/24/outline";
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
const loading = ref(false);

async function search() {
  if(loading.value) {
    alert("正在搜索，请稍后再试。");
    return;
  }
  loading.value = true;
  const treeholePromise = client.get<TreeholeSearchResult>(
    `api/search/treehole?keyword=${encodeURIComponent(content.value)}`
  );
  const otherPromise = client.get<SearchResult[]>(
    `api/search?keyword=${encodeURIComponent(content.value)}`
  );

  let treehole, others;
  try {
    [treehole, others] = await Promise.allSettled([treeholePromise, otherPromise]);
  } catch (e) {
    console.error(e);
  }
  if (treehole?.status === "fulfilled") {
    treeholeResults.value = treehole.value.data;
  }
  if (others?.status === "fulfilled") {
    otherResults.value = others.value.data;
  }
  loading.value = false;
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

function smoothScroll(targetId: string, e: Event) {
  e.preventDefault();
  document.querySelector(targetId)?.scrollIntoView({ behavior: "smooth" });
}
</script>

<template>
  <div flex="~ col" flex-shrink flex-grow items-center>
    <div flex="~" max-w-140 w-full>
      <input flex-grow w-1 flex-shrink rounded="lt-lg lb-lg rt-none rb-none" text-lg type="text" v-model="content"
        @keydown.enter="search" placeholder="搜索内容..." />
      <button rounded="lt-none lb-none rt-lg rb-lg" w-12 h-12 flex="~ items-center justify-center" @click="search">
        <MagnifyingGlassIcon class="h-5 w-5" />
      </button>
      <button m-l-2 w-12 h-12 flex="~ items-center justify-center" @click="openConfig">
        <Cog6ToothIcon class="h-5 w-5" />
      </button>
    </div>
    <div flex style="max-width: calc(100vw - 2em)" overflow-scroll gap-2>
      <div max-w-140 w-full gap-2 flex="~ col" m-t-2 flex-grow flex-shrink overflow-auto>
        <TreeholeSearchSourceResult v-if="treeholeResults" :result="treeholeResults" />
        <SearchSourceResult v-for="result in otherResults" :result="result" />
      </div>
      <div v-if="otherResults.length > 0" flex="~ col items-start" class="!hidden md:!flex bg-gray/10" p-4 self-start rounded-lg m-t-2>
        <span m-b-2 font-semibold text-lg>导航</span>
        <a color-unset underline-offset-4 underline-.5
          href="#treehole" class="group" @click="smoothScroll('#treehole', $event)">
          <div flex="~ items-center">
            <span>树洞</span>
            <ChevronRightIcon class="h-4 w-4 m-l-1 group-hover:translate-x-1 transition-all duration-150" />
          </div>
        </a>
        <a color-unset underline-offset-4 underline-.5 v-for="result in otherResults" :key="result.sourceInfo.name"
          :href="`#${result.sourceInfo.name}`" class="group" @click="smoothScroll(`#${result.sourceInfo.id}`, $event)">
          <div flex="~ items-center">
            <span>{{ result.sourceInfo.name }}</span>
            <ChevronRightIcon class="h-4 w-4 m-l-1 group-hover:translate-x-1 transition-all duration-150" />
          </div>
        </a>
      </div>
    </div>
    <span v-if="loading" class="absolute top-0 left-0 w-full h-full flex items-center justify-center backdrop-blur-xl">正在加载...</span>
    <TreeholeDetails ref="detailsElementRef" />
  </div>
</template>
