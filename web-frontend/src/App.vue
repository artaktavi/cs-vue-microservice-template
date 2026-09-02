<template>
  <n-config-provider :theme-overrides="naiveUITheme">
    <n-message-provider>
      <RouterView />
    </n-message-provider>
  </n-config-provider>
</template>

<script setup lang="ts">
import { NConfigProvider, NMessageProvider } from 'naive-ui'
import { naiveUITheme } from '@/config/naiveUITheme'
</script>

