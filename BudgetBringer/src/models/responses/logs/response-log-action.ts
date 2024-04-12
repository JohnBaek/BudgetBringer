import {ResponseCommonWriter} from "../response-common-writer";
import {EnumDatabaseLogActionType} from "../../enums/enum-database-log-action-type";

/**
 * 액션 로그 응답 클래스
 */
export class ResponseLogAction extends ResponseCommonWriter {
  /**
   * 내용
   */
  contents: string;
  /**
   * 카테고리
   */
  category: string ;
  /**
   * 액션타입
   */
  actionType: EnumDatabaseLogActionType ;
}
