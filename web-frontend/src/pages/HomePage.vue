<template>
  <main class="home-page">
    <n-card class="welcome-card">
      <n-space vertical :size="SPACING.lg">
        <n-tag type="success" :bordered="false">{{ t('home.badge') }}</n-tag>
        <div>
          <h1>{{ t('home.title') }}</h1>
          <p>{{ t('home.description') }}</p>
        </div>
        <n-space>
          <n-button type="primary" :loading="loading" @click="loadStatus">
            {{ t('home.checkService') }}
          </n-button>
          <n-text v-if="status" type="success">
            {{ t('home.serviceStatus', { status }) }}
          </n-text>
        </n-space>
      </n-space>
    </n-card>
  </main>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useI18n } from 'vue-i18n'
import { NButton, NCard, NSpace, NTag, NText } from 'naive-ui'
import { getServiceStatus } from '@/api/app'
import { SPACING } from '@/utils/constants'

const { t } = useI18n()
const loading = ref(false)
const status = ref('')

const loadStatus = async () => {
  loading.value = true
  try {
    status.value = (await getServiceStatus()).status
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.home-page {
  min-height: 100vh;
  display: grid;
  place-items: center;
  padding: var(--spacing-xl);
  background: var(--color-background);
}

.welcome-card {
  width: min(100%, var(--content-width));
  box-shadow: var(--shadow-card);
}

h1 {
  margin: 0 0 var(--spacing-sm);
  font-size: var(--font-size-heading);
  font-weight: 600;
}

p {
  margin: 0;
  color: var(--color-text-secondary);
}
</style>
