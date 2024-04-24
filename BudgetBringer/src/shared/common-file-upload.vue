<script setup lang="ts">
import {onMounted, onUnmounted, ref,watch} from "vue";
const props = defineProps({
  files: Array
});

/**
 * Contains files
 */
const files = ref([...props.files]);
/**
 * Remove file of Contains
 * @param index
 */
const remove = (index) => {
    files.value.splice(index, 1);
};
/**
 * Changed file Contains
 */
const inputChanged = () => {
  console.log(files);
}
const emits = defineEmits(['update-files']);
watch(files, (newFiles) => {
  emits('update-files', newFiles);
});
defineExpose({
  getFiles() {
    return files.value;
  }
});
</script>

<template>
  <v-file-input
    v-model="files"
    small-chips
    show-size
    multiple
    clearable
    @change="inputChanged"
  >
    <template v-slot:selection="{ text, index, file }">
      <v-chip
        small
        text-color="white"
        color="#295671"
        close
        @click:close="remove(index)"
      >
        {{ text }}
      </v-chip>
    </template>
  </v-file-input>
</template>

<style scoped lang="css">
</style>
