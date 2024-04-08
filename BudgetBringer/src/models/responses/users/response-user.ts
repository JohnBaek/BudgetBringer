import {ResponseUserRole} from "./response-user-role";

/**
 * 사용자 응답정보 모델
 */
export class ResponseUser {
  /**
   * 유저명
   */
  name : string = '';

  /**
   * 사용자 역할
   */
  roles: Array<ResponseUserRole> = [];
}
