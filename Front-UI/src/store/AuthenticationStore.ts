import {defineStore} from "pinia";
import {ResponseUser} from "../models/Responses/Users/ResponseUser";


/**
 * 인증 상태
 */
interface AuthenticationState {
  /**
   * 인증여부
   */
  isAuthenticated: boolean;

  /**
   * 유저 상태 정보
   */
  userInformation: ResponseUser;
}

/**
 * 인증 상태 관리
 */
export const AuthenticationStore = defineStore('authenticated', {
  // 인증상태 정의
  state: (): AuthenticationState => {
    return {
      isAuthenticated: false,
      userInformation: new ResponseUser()
    }
  },
  actions: {
    /**
     * 상태를 업데이트한다.
     * @param userInformation 유저 정보
     */
    updateAuthenticated( userInformation: ResponseUser) {
      this.isAuthenticated = true;
      this.userInformation = userInformation;
    },

    /**
     * 인증정보를 초기화한다.
     */
    clearAuthenticated() {
      this.isAuthenticated = false;
      this.userInformation = new ResponseUser()
    }
  }
});
