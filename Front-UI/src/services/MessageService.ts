export type MessageType = 'success' | 'warning' | 'error';

export interface MessageInformation {
  id: number;
  content: string;
  type: MessageType;
  visible: boolean;
}

import { ref, Ref } from 'vue';

export const messageQueue: Ref<MessageInformation[]> = ref([]);

let nextId = 0;

const showMessage = (content: string, type: MessageType) => {
  const id = nextId++;
  messageQueue.value.push({ id, content, type, visible: true });

  // 메시지를 표시한 후 일정 시간이 지나면 큐에서 제거
  setTimeout(() => {
    const index = messageQueue.value.findIndex(message => message.id === id);
    if (index !== -1) {
      messageQueue.value.splice(index, 1);
    }
  }, 1000 * 10); // 10초 후 자동으로 메시지를 큐에서 제거
};

export const messageService = {
  showError: (content: string) => showMessage(content, 'error'),
  showWarning: (content: string) => showMessage(content, 'warning'),
  showSuccess: (content: string) => showMessage(content, 'success'), // `showInfo` 대신 `showSuccess` 사용 권장
};

