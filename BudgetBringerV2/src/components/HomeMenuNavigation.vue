
<script setup>
import { ref } from "vue";
const menuItems = ref([
  { label: 'Home', icon: 'pi pi-fw pi-home', to: '/' },
  { label: 'Login', icon: 'pi pi-fw pi-sign-in', to: '/login' },
  // 다른 메뉴 항목들 추가
]);
const menuVisible = ref(true);
</script>

<template>
  <Sidebar :visible="menuVisible" @hide="menuVisible = false">
    <Menu :model="menuItems" />
  </Sidebar>
</template>

<style scoped>

</style>
