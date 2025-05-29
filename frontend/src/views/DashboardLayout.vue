<template>
  <div style="display: flex;">
    <SideBarMenu />
    <div style="flex: 1; padding: 20px;">
      <router-view />
    </div>
  </div>
</template>

<script setup>
import SideBarMenu from '../components/SideBarMenu.vue'
</script>
