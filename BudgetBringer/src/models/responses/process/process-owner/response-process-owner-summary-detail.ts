import {ResponseProcessOwner} from "./response-process-owner";

/**
 * 상세
 */
export interface ResponseProcessOwnerSummaryDetail {
  /**
   * 시퀀스 정보 , 총 3가지의 종류로 나가기때문
   */
  sequence: number;
  /**
   * 타이틀 정보
   */
  title: string;
  /**
   * 상세 정보 리스트
   */
  items: ResponseProcessOwner[];
}
