<script setup lang="ts">
import { ref } from "vue";
const props = defineProps({
  files: Array,
  title: String,
});

/**
 * Contains files
 */
// eslint-disable-next-line vue/no-dupe-keys
const files = ref([]);
</script>

<template>
<!--  <v-card v-if="fileInfos.length > 0" class="mx-auto">-->
<!--    <v-card-subtitle>Attached Files</v-card-subtitle>-->
<!--    <v-list>-->
<!--      -->
<!--      <v-subheader>List of Files</v-subheader>-->
<!--      <v-list-item-group color="primary">-->
<!--        <v-list-item v-for="(file, index) in fileInfos" :key="index">-->
<!--          <a :href="file.url">{{ file.name }}</a>-->
<!--        </v-list-item>-->
<!--      </v-list-item-group>-->
<!--    </v-list>-->
<!--  </v-card>-->
</template>

<style scoped>
</style>
