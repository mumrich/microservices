<template>
  <div class="hello">
    <h1>{{ msg }}</h1>
    <h2>ServerTime: {{ ServerTime }}</h2>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import * as signalR from "@microsoft/signalr";

export default defineComponent({
  name: "HelloWorld",
  prop: {
    msg: String,
  },
  data() {
    return {
      ServerTime: null,
    };
  },
  created() {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("/dotnetify")
      .build();
    connection.on("response_vm", this.onResponseVm);
    connection
      .start()
      .then(() => connection.invoke("Request_VM", "HelloWorldVm", {}))
      .catch((error) => console.error(error));
  },
  methods: {
    onResponseVm(...args: any[]): void {
      const [vmName, serializedState] = args[0];
      const state = JSON.parse(serializedState);
      Object.assign(this.$data, state);
    },
  },
});
</script>
