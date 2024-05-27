import { RequestLogin } from '@/models/requests/login/request-login'
import { Observable, tap } from 'rxjs'
import { Response } from '@/models/responses/response'
import { ApiClient } from '@/services/apis/ApiClient'
import type { ResponseData } from '@/models/responses/response-data'
import type { ResponseUser } from '@/models/responses/users/response-user'
import { useAuthenticationStore } from '@/services/stores/AuthenticationStore'

/**
 * Authentication API Service
 */
export class AuthenticationApiService {
  // API Client
  private readonly client: ApiClient = new ApiClient('/api/v1/Authentication');

  // Authentication Store
  private readonly authenticationStore  = useAuthenticationStore();

  /**
   * Try login to Server
   * @param request
   */
  public tryLoginAsync(request: RequestLogin) : Observable<ResponseData<ResponseUser>> {
    return this.client.restApi.requestPostAsyncAutoCommunications<ResponseData<ResponseUser>>(`${this.client.baseUrl}/login`, request)
      .pipe(
        tap((response) => {
          // Login has completed
          if(response.success && response.data) {
            this.authenticationStore.updateAuthenticated(response.data);
          }
        })
      );
  };

  /**
   * Logout
   */
  public logoutAsync()  : Observable<Response> {
    return this.client.restApi.requestGetAsync<Response>(`${this.client.baseUrl}/logout`)
      .pipe(
        tap(() => {
          this.authenticationStore.clearAuthenticated();
        })
      );
  };

  /**
   * Check has authenticated
   */
  public isAuthenticatedAsync() : Observable<Response> {
    return this.client.restApi.requestGetAsync(`${this.client.baseUrl}/IsAuthenticated`);
  }
}
