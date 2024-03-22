<template>
  <v-dialog v-model="visible" persistent max-width="290">
    <v-card>
      <v-card-title class="text-h5">{{ title }}</v-card-title>
      <v-card-text>{{ message }}</v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="green darken-1"  @click="confirm">
          Yes
        </v-btn>
        <v-btn color="red darken-1"  @click="cancel">
          No
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { defineComponent, ref, toRefs } from 'vue';

export default defineComponent({
  name: 'ConfirmDialog',
  props: {
    title: {
      type: String,
      default: 'Confirm',
    },
    message: {
      type: String,
      default: 'Are you sure?',
    },
  },
  setup(props, { emit }) {
    const visible = ref(false);

    function confirm() {
      emit('confirm');
      visible.value = false;
    }

    function cancel() {
      emit('cancel');
      visible.value = false;
    }

    // Expose the methods to allow external control
    function showDialog() {
      visible.value = true;
    }

    return {
      ...toRefs(props),
      visible,
      confirm,
      cancel,
      showDialog,
    };
  },
});
</script>
