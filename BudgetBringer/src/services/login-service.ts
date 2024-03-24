import {ResponseData} from "../models/responses/response-data";
import {ResponseUser} from "../models/responses/users/response-user";
import {RequestLogin} from "../models/requests/login/request-login";
import {EnumResponseResult} from "../models/enums/enum-response-result";
import {AuthenticationStore} from "./stores/authentication-store";

/**
 * 인증정보 Store
 */

/**
 * 로그인 서비스
 */
export const loginService = {
  /**
   * 로그인을 시도한다.
   * @param request
   */
  async requestLoginAsync(request: RequestLogin): Promise<ResponseData<ResponseUser>> {
    try {
      // 인증정보 상태를 가져온다.
      const authenticationStore = AuthenticationStore();

      // TODO Fake 서버 요청
      let response: ResponseData<ResponseUser> = {
        result: EnumResponseResult.error,
        message: '아이디와 패스워드를 확인해주세요',
        data: { name: '' },
      };
      if(request.loginId === 'admin' && request.password === '1234') {
        response = {
          result: EnumResponseResult.success,
          message: '',
          data: { name: '관리자' },
        };
      }

      // 응답에 실패한 경우
      if(response.result !== EnumResponseResult.success) {
        // 저장된 인증 정보를 초기화 한다.
        authenticationStore.clearAuthenticated();
        return response;
      }

      // 인증정보를 보관한다.
      authenticationStore.updateAuthenticated(response.data);
      return response;
    } catch (error) {
      return new ResponseData<ResponseUser>(EnumResponseResult.error, '서버와 통신중 문제가 발생했습니다.' , null);
    }
  },
};
