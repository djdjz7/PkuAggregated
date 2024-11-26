<script setup lang="ts">
import { ChevronRightIcon } from "@heroicons/vue/24/outline";
import { onMounted, ref, watch } from "vue";

const contentWrapperRef = ref<HTMLDivElement>();
const contentDesiredSizeWrapperRef = ref<HTMLDivElement>();

const content = ref(0);
const collapsed = ref(false);

function toggleCollapsed() {
  collapsed.value = !collapsed.value;
  if (collapsed.value) {
    contentWrapperRef.value!.style.height = "0";
  } else {
    contentWrapperRef.value!.style.height = `${content.value}px`;
  }
}

onMounted(() => {
  if (!contentWrapperRef.value || !contentDesiredSizeWrapperRef.value) return;

  watch(content, (newSize, _) => {
    if (!collapsed.value)
      contentWrapperRef.value!.style.height = `${newSize}px`;
  });

  content.value = contentDesiredSizeWrapperRef.value!.scrollHeight;

  var observer = new ResizeObserver(() => {
    content.value = contentDesiredSizeWrapperRef.value!.scrollHeight;
  });

  observer.observe(contentDesiredSizeWrapperRef.value);
});
</script>

<template>
  <div
    rounded-lg
    class="bg-gray/10"
    flex="~ col items-stretch"
    text-left
    overflow-clip
  >
    <div flex="~ items-stratch" h-13>
      <div
        flex="~ items-center"
        transition-all
        duration-200
        p-l-4
        class="hover:bg-gray/20"
        text-unset
        text-lg
        flex-grow
        flex-shrink
        font-500
      >
        <slot name="header"></slot>
      </div>
      <button
        @click="toggleCollapsed"
        rounded-unset
        transition-all
        duration-200
        class="hover:bg-gray/20 bg-transparent"
        flex="~ items-center justify-center"
        self-stretch
        w-13
      >
        <ChevronRightIcon
          :class="[
            'h-5 w-5 transition-all duration-300',
            collapsed ? 'rotate-0' : 'rotate-90',
          ]"
        />
      </button>
    </div>

    <div ref="contentWrapperRef" transition-all duration-300 h-0>
      <div ref="contentDesiredSizeWrapperRef">
        <slot></slot>
      </div>
    </div>
  </div>
</template>
