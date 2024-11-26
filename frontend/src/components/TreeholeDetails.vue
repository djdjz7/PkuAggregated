<script setup lang="ts">
import { XMarkIcon } from "@heroicons/vue/24/outline";
import { onMounted, ref } from "vue";
import {
  TreeholeComment,
  TreeholeSearchResultItem,
} from "../models/TreeholeSearchResult";
import { client } from "../client";
import { useConfigStore } from "../stores/configStore";
import CommentComponent from "./CommentComponent.vue";

const collapsed = ref(true);
const currentPid = ref(0);
const currentHole = ref<TreeholeSearchResultItem | null>();
const currentHoleComments = ref<TreeholeComment[]>();
const bgMaskRef = ref<HTMLDivElement>();
const configStore = useConfigStore();
const loadingComments = ref(false);

const toggleCollapsed = (hole: TreeholeSearchResultItem | null) => {
  if (collapsed.value) {
    bgMaskRef.value?.classList.remove("hidden");
  }
  // 为了动画我只能这么写了
  setTimeout(async () => {
    collapsed.value = !collapsed.value;
    if (!collapsed.value) {
      currentPid.value = hole?.pid ?? 0;
      currentHole.value = hole;
      await getTreeholeComments(currentPid.value);
    } else {
      setTimeout(() => {
        if (collapsed.value) bgMaskRef.value?.classList.add("hidden");
      }, 400);
    }
  }, 1);
};
defineExpose({ toggleCollapsed });
onMounted(() => {
  bgMaskRef.value?.classList.add("hidden");
});

async function getTreeholeComments(pid: number) {
  currentHoleComments.value = [];
  loadingComments.value = true;

  var response = await client.get<TreeholeComment[]>(
    `/api/treehole/details/${pid}`
  );
  currentHoleComments.value = response.data;

  loadingComments.value = false;
}
</script>

<template>
  <div
    fixed
    w-100vw
    h-100vh
    top-0
    left-0
    transition-backdrop-filter
    duration-400
    :class="[collapsed ? 'backdrop-brightness-100' : 'backdrop-brightness-70']"
    ref="bgMaskRef"
  >
    <div
      fixed
      bottom-0
      left-0
      w-full
      h-90vh
      md:h-80vh
      rounded="lt-lg rt-lg"
      bg-light-1
      transition-all
      duration-400
      ease-in-out
      p-4
      p-b-0
      md:p-8
      md:p-b-0
      box-border
      overflow-y-auto
      flex="~ col"
      :class="[
        collapsed ? 'translate-y-90vh md:translate-y-80vh' : 'translate-y-0',
      ]"
    >
      <div flex="~ items-center">
        <h2 flex-grow flex-shrink m-0 font-400 text-left>
          树洞详情 #{{ currentPid }}
        </h2>
        <button
          @click="toggleCollapsed(null)"
          float-right
          class="h-12 w-12"
          flex="~ items-center justify-center"
          p-0
          rounded-full
          bg-transparent
        >
          <XMarkIcon class="h-6 w-6" />
        </button>
      </div>
      <span text-left text-gray-500 text-sm m-b-2>{{
        new Date(currentHole?.time ?? "").toLocaleString()
      }}</span>
      <div text-left overflow-y-auto md:grid md:cols-2>
        <div overflow-auto overscroll-contain md:p-r-2>
          <p whitespace-pre text-wrap>{{ currentHole?.text }}</p>
          <img
            max-w-full
            v-if="currentHole?.imageId && currentHole?.imageId != 0"
            :src="`${configStore.host}api/treehole/image/${currentHole.imageId}`"
          />
        </div>
        <div overflow-auto overscroll-contain md:p-l-2>
          <h3>评论</h3>
          <div v-if="loadingComments">
            少女祈祷中...
          </div>
          <div flex="~ col" gap-2>
            <CommentComponent
              v-for="comment in currentHoleComments"
              :comment="comment"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
