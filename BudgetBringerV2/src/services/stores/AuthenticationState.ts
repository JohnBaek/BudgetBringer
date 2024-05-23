import {defineStore} from "pinia";
import type { ResponseUser } from '@/models/responses/users/response-user'


/**
 * 인증 상태
 */
interface AuthenticationState {
  /**
   * 인증여부
   */
  isAuthenticated: boolean;

  /**
   * 유저 정보
   */
  authenticatedUser: ResponseUser;
}

/**
 * 인증 상태 관리
 */
export const AuthenticationState = defineStore('authenticated', {
  // 인증상태 정의
  state: (): AuthenticationState => {
    return {
      isAuthenticated: false,
      authenticatedUser: JSON.parse(localStorage.getItem('authenticatedUser') || '{}')
    }
  },
  actions: {
    /**
     * 상태를 업데이트한다.
     * @param userInformation 유저 정보
     */
    updateAuthenticated( userInformation: ResponseUser) {
      this.isAuthenticated = true;
      this.authenticatedUser = userInformation;

      // 로컬 스토리지에 사용자의 정보를 저장한다.
      localStorage.setItem('authenticatedUser', JSON.stringify(userInformation));
    },

    /**
     * 인증정보를 초기화한다.
     */
    clearAuthenticated() {
      this.isAuthenticated = false;
      this.authenticatedUser = new ResponseUser();

      // 로컬 스토리지에 사용자의 정보를 제거한다.
      localStorage.removeItem('authenticatedUser')
    },


    /**
     * Permission 여부를 확인한다.
     * @param permissions
     */
    hasPermission(permissions: Array<string>): boolean {
      let isHasPermission : boolean = false;

      // 모든 역할에 대해 검색한다.
      for (const role of this.authenticatedUser.roles) {


        // Claim 값에서 값이 있는지 찾는다.
        isHasPermission = role.claims.some(i => permissions.includes(i.value));

        // 찾은경우
        if(isHasPermission)
          break;
      }

      return isHasPermission;
    }
  }
});
