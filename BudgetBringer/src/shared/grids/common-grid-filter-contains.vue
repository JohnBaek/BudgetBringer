<template>
  <input
    type="text"
    v-model="text"
    @keydown.enter="onEnter"
    placeholder="Type and hit enter to filter..."
  />
</template>

<script>
export default {
  props: ['params'],
  data() {
    return {
      text: '',
    };
  },
  methods: {
    onEnter() {
      // 필터링 메소드 호출
      this.params.parentFilterInstance(instance => {
        instance.setModel({ // 이 부분은 해당 필터 타입에 맞게 설정하세요.
          type: 'equals',
          filter: this.text
        });
        this.params.api.onFilterChanged();
      });
    }
  }
};
</script>
