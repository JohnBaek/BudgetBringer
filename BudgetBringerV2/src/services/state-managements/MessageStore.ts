import { defineStore } from 'pinia';
import { useToast } from 'primevue/usetoast';

export const useMessageStore = defineStore('messageStore', {
  state: () => ({
    toast: useToast()
  }),
  actions: {
    showInfo(summary: string, message: string, delay: number = 3000) {
      this.toast.add({ severity: 'info', summary: summary, detail: message, life: delay });
    },
    showSuccess(summary: string, message: string, delay: number = 3000) {
      this.toast.add({ severity: 'success', summary: summary, detail: message, life: delay });
    },
    showWarning(summary: string, message: string, delay: number = 3000) {
      this.toast.add({ severity: 'warn', summary: summary, detail: message, life: delay });
    },
    showError(summary: string, message: string, delay: number = 3000) {
      this.toast.add({ severity: 'error', summary: summary, detail: message, life: delay });
    },
  }
});