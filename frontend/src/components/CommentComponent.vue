<script setup lang="ts">
import { TreeholeComment } from "../models/TreeholeSearchResult";

defineProps<{
  comment: TreeholeComment;
}>();


// QWen Generated
function stringToColor(input: string): string {
  const hash = input.split("").reduce((acc, curr) => {
    return acc * 31 + curr.charCodeAt(0);
  }, 0);
  const hue = hash % 360;
  const saturation = 70;
  const minLightness = 80;
  let lightness = Math.abs((hash % 20) + minLightness);

  return `hsl(${hue}, ${saturation}%, ${lightness}%)`;
}
</script>

<template>
  <div
    rounded-lg
    bg-yellow-100
    p-2
    :style="`background-color: ${stringToColor(comment.name)};`"
  >
    <span font-500>{{ comment.name }}</span>
    <span text-gray-500 text-sm m-l-2>#{{ comment.cid }}</span>
    <div
      v-if="comment.quote != null"
      m-y-1
      p-2
      rounded-md
      class="bg-gray-100"
      opacity-65
    >
      <span font-500>{{ comment.quote.name_tag }}</span>
      <p whitespace-pre text-wrap m-t-1 m-b-0>{{ comment.quote.text }}</p>
    </div>
    <p whitespace-pre text-wrap m-t-1 m-b-0>{{ comment.text }}</p>
  </div>
</template>
