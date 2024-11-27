<script setup lang="ts">
import { ref } from "vue";
import { TreeholeComment } from "../models/TreeholeSearchResult";
import { usePreferredDark } from "@vueuse/core";

const props = defineProps<{
  comment: TreeholeComment;
}>();

const isDark = usePreferredDark();
const hslString = ref(stringToColor(props.comment.name));


// QWen Generated
function stringToColor(input: string) : string {
  const hash = input.split("").reduce((acc, curr) => {
    return acc * 31 + curr.charCodeAt(0);
  }, 0);
  const hue = hash % 360;
  const saturation = 70;
  const minLightness = 80;
  const maxLightnessDark = 20;
  let lightness;
  if(!isDark.value) {
    lightness = Math.abs((hash % 20) + minLightness);
  } else {
    lightness = hash % maxLightnessDark + maxLightnessDark;
  }

  return `hsl(${hue}, ${saturation}%, ${lightness}%)`;
}
</script>

<template>
  <div
    rounded-lg
    bg-yellow-100
    p-2
    :style="`background-color: ${hslString};`"
  >
    <span font-500>{{ comment.name }}</span>
    <span text-gray-500 dark:text-gray-300 text-sm m-l-2>#{{ comment.cid }}</span>
    <div
      v-if="comment.quote != null"
      m-y-1
      p-2
      rounded-md
      class="bg-gray-100 dark:bg-black/40"
      opacity-65
    >
      <span font-500>{{ comment.quote.name_tag }}</span>
      <p whitespace-pre text-wrap m-t-1 m-b-0>{{ comment.quote.text }}</p>
    </div>
    <p whitespace-pre text-wrap m-t-1 m-b-0>{{ comment.text }}</p>
  </div>
</template>
