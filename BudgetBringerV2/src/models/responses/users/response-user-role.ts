import type { ResponseUserRoleClaim } from '@/models/responses/users/response-user-role-claim'

/**
 * 사용자 역할 클래스
 */
export interface ResponseUserRole {
  /**
   * 역할 명
   */
  name : string;
  /**
   * 상세 권한
   */
  claims : Array<ResponseUserRoleClaim>;
}
