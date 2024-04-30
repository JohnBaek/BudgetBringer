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
files.value = ['test','test123'];

/**
 * Remove Files
 */
const removeFile = (index) => {
  console.log(index)
}
/**
 * Upload Files
 */
const uploadFiles = ref([]);
</script>

<template>
  <v-list density="compact">
    <v-card-title>Uploaded Files</v-card-title>
    <v-list-item
      v-for="(item, index) in files"
      :key="index"
      link
    >
      <v-row dense >
        <v-col class="justify-space-between">
          {{ item }}
        </v-col>
        <v-col align="right" @click="removeFile(index)">
          <v-icon>mdi-close</v-icon>
        </v-col>
      </v-row>
    </v-list-item>
  </v-list>
  <v-file-input label="파일을 추가하려면 클릭하세요" variant="outlined"></v-file-input>
</template>

<style scoped>
</style>
