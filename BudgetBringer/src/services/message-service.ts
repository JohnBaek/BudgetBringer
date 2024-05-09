import {Ref, ref} from "vue";
import {MessageInformation} from "./message-service-model";

/**
 * 메세지 타입
 */
export type MessageType = 'success' | 'warning' | 'error';

/**
 * 호출한 메세지를 보관할 Queue
 */
export const messageQueue : Ref<MessageInformation[]>  = ref([]);

/**
 * 다음 호출메세지의 Id
 */
let nextId = 0;

/**
 * 메세지를 출력한다.
 * @param content 메세지의 내용
 * @param messageType 메세지의 종류
 */
const showMessage = (content: string, messageType: string ) => {
  // 메세지의 아이디를 지정한다.
  const id = nextId++;

  // Queue 에 메세지를 보관한다.
  messageQueue.value.push({ id, content, type: messageType, visible: true });

  // 메시지를 표시한 후 일정 시간이 지나면 큐에서 제거
  setTimeout(() => {
    const index = messageQueue.value.findIndex(message => message.id === id);
    if (index !== -1) {
      messageQueue.value.splice(index, 1);
    }
  }, 1000 * 10); // 10초 후 자동으로 메시지를 큐에서 제거
};

/**
 * 메세지 서비스
 */
export const messageService = {
  /**
   * 에러 메세지를 출력한다.
   * @param content 메세지의 내용
   */
  showError: (content: string) => showMessage(content, 'error'),

  /**
   * 겅고 메세지를 출력한다.
   * @param content 메세지의 내용
   */
  showWarning: (content: string) => showMessage(content, 'warning'),

  /**
   * 성공 메세지를 출력한다.
   * @param content 메세지의 내용
   */
  showSuccess: (content: string) => showMessage(content, 'success'),
};
