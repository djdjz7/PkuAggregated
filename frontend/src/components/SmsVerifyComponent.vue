<script setup lang="ts">
import { ref } from "vue";
import { client } from "../client";

let interval;
let count = 60;

const text = ref("获取验证码");
const code = ref("");
const disabled = ref(false);
const success = ref(false);

function countDown() {
  count--;
  if (count === 0) {
    clearInterval(interval!);
    text.value = "获取验证码";
    disabled.value = false;
  } else {
    text.value = `${count} 秒后重发`;
  }
}

async function sendMsg() {
  count = 60;
  countDown();
  interval = setInterval(countDown, 1000);
  disabled.value = true;

  const response = await client.post<boolean>("/api/treehole/send-msg");
  if (!response.data) {
    alert("验证码发送失败");
  }
}

async function verifyCode() {
  const response = await client.post<boolean>(
    "/api/treehole/verify",
    `"${code.value}"`,
    {
      headers: {
        "Content-Type": "application/json",
      },
    }
  );
  if (response.data) {
    success.value = true;
  } else {
    alert("验证失败!");
  }
}
</script>

<template>
  <div h-10 flex justify-center v-if="!success">
    <input
      placeholder="短信验证码"
      rounded="rt-0 rb-0"
      @keypress.enter="verifyCode"
      v-model="code"
    />
    <button rounded="lt-0 lb-0 " :disabled="disabled" @click="sendMsg">
      {{ text }}
    </button>
  </div>
  <span v-else>验证成功，请重试搜索。</span>
</template>
