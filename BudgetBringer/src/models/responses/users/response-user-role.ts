import {ResponseUserRoleClaim} from "./response-user-role-claim";

/**
 * 사용자 역할 클래스
 */
export class ResponseUserRole {
  /**
   * 역할 명
   */
  name : string;

  /**
   * 상세 권한
   */
  claims : Array<ResponseUserRoleClaim>  = [];
}
