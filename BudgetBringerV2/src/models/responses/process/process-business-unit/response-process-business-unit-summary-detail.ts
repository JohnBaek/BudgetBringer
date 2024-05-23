import {ResponseProcessBusinessUnit} from "./response-process-business-unit";

/**
 * 상세
 */
export class ResponseProcessBusinessUnitSummaryDetail {
  /**
   * 시퀀스 정보 , 총 3가지의 종류로 나가기때문
   */
  sequence: number;
  /**
   * 타이틀 정보 CAPEX below CHF500K CAPEX above CHF500K Total CAPEX
   */
  title: string;
  /**
   * 상세 정보 리스트
   */
  items: ResponseProcessBusinessUnit[];
}
