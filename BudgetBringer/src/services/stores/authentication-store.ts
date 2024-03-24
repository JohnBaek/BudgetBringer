import {defineStore} from "pinia";
import {ResponseUser} from "../../models/responses/users/response-user";


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
export const AuthenticationStore = defineStore('authenticated', {
  // 인증상태 정의
  state: (): AuthenticationState => {
    return {
      isAuthenticated: false,
      authenticatedUser: new ResponseUser()
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
    },

    /**
     * 인증정보를 초기화한다.
     */
    clearAuthenticated() {
      this.isAuthenticated = false;
      this.authenticatedUser = new ResponseUser()
    }
  }
});
