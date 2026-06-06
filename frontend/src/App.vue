<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <nav class="navbar">
    <RouterLink to="/products">Products</RouterLink>
  </nav>
  <RouterView />
</template>

<style scoped>
.navbar {
  padding: 12px 24px;
  background: #fff;
  border-bottom: 1px solid #eee;
}
.navbar a {
  text-decoration: none;
  color: #333;
  font-weight: 500;
}
</style>