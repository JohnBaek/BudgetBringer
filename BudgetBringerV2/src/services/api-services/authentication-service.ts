import {RequestLogin} from "../../models/requests/login/request-login";
import {Observable} from "rxjs";
import {HttpService} from "./http-service";
import {Response} from "../../models/responses/response";
import {ResponseUser} from "../../models/responses/users/response-user";
import {ResponseData} from "../../models/responses/response-data";


const baseURI = '/api/v1/Authentication';

/**
 * 로그인 서비스
 */
export const authenticationService  = {
  /**
   * 로그인을 시도한다.
   * @param request
   */
  tryLoginAsync(request: RequestLogin): Observable<ResponseData<ResponseUser>> {
    return HttpService.requestPost<Response>(`${baseURI}/Login`, request);
  },

  /**
   * 로그아웃을 처리한다.
   */
  logout() : Observable<Response> {
    return HttpService.requestGet(`${baseURI}/Logout`);
  },

  /**
   * 로그인여부를 확인한다.
   */
  isAuthenticatedAsync() : Observable<Response> {
    return HttpService.requestGet(`${baseURI}/IsAuthenticated`);
  },
};
