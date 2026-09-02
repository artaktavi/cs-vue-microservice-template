import type { GlobalThemeOverrides } from 'naive-ui'
import { BORDER_RADIUS, COLORS } from '@/utils/constants'

export const naiveUITheme: GlobalThemeOverrides = {
  common: {
    primaryColor: COLORS.primary,
    primaryColorHover: COLORS.primaryHover,
    bodyColor: COLORS.background,
    cardColor: COLORS.surface,
    textColor1: COLORS.textPrimary,
    textColor2: COLORS.textSecondary,
    borderColor: COLORS.border,
    successColor: COLORS.success,
    borderRadius: BORDER_RADIUS.md,
  },
}

