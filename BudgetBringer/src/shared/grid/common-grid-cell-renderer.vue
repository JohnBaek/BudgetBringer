<script>
export default {
  data() {
    return {
      value: this.params.valueFormatted ? this.params.valueFormatted : this.params.value,
    };
  },
};
</script>

<template>
  <span>{{value}}</span>
</template>

<style scoped lang="css">

</style>
