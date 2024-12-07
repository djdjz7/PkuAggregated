<script setup>
import { useRouter } from "vue-router";
import { useConfigStore } from "../stores/configStore";
const configStore = useConfigStore();
const router = useRouter();

function agree() {
  configStore.$patch({ readEula: true });
  router.push("/");
}

function disagree() {
  alert("其实我是真的无所谓啦~爱用不用呗~");
  configStore.$patch({ readEula: false });
}
</script>

<template>
  <div max-w-1200px mx-auto>
    <h2>最终用户许可协议</h2>
    <div text-left>
      <p>请您仔细阅读以下条款，如果您同意以下条款，请点击“我同意”按钮。</p>
      <ol>
        <li line-through>
          不是这东西有什么好写的，你自己部署的后端你自己看着办咯。
        </li>
        <li>请不要窃取其他同学的账号密码用于登录此工具后端。</li>
        <li>请不要将此工具用于违规用途，否则后果自负。</li>
        <li>自己的账号，其他事情需要你自己衡量！</li>
      </ol>
      <p>另可参见 <a href="/faq" text-purple decoration-none>FAQ</a>。</p>
    </div>
    <div flex="~ items-center justify-center md:justify-end wrap" gap-2 float-right>
      <button text-1em @click="disagree">我们拒绝签字！</button>
      <button bg-purple text-white hover:bg-purple-5 text-1em @click="agree">
        我同意
      </button>
    </div>
  </div>

</template>

<style scoped>
h2 {
  --at-apply: font-400;
}
</style>
