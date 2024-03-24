<script setup lang="ts">
import {messageQueue} from "../services/message-service";
import {MessageInformation} from "../services/message-service-model";

/**
 * 메세지를 닫는다.
 * @param messageInformation 메세지 정보
 */
const closeMessage = (messageInformation:MessageInformation) => {
  messageInformation.visible = false;
}
</script>

<template>
  <v-snackbar
    top
    right
    location="top"
    v-for="message in messageQueue"
    :key="message.id"
    :color="message.type"
    :style="{ right: '50%', transform: 'translateX(50%)' }"
    v-model="message.visible"
    @click="closeMessage(message)"
  >
    {{ message.content }}
  </v-snackbar>
</template>

<style scoped lang="css">
</style>
