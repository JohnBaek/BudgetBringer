import {EnumResponseResult} from "../models/enums/enum-response-result";

/**
 * 메세지 기본 모델
 */
export interface MessageInformation {
  /**
   * 구분값
   */
  id: number;
  /**
   * 들어갈 내용
   */
  content: string;
  /**
   * Message 종류
   */
  type: string;
  /**
   * 보임 또는 안보임 여부
   */
  visible: boolean;
}
