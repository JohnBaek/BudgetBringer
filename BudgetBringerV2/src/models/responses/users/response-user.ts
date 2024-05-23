import type { ResponseUserRole } from '@/models/responses/users/response-user-role'

/**
 * 사용자 응답정보 모델
 */
export interface ResponseUser {
  // 아이디
  id : string;

  // 유저명
  displayName : string;

  // 로그인 아이디
  loginId: string;

  // 사용자 역할
  roles: Array<ResponseUserRole>;
}
