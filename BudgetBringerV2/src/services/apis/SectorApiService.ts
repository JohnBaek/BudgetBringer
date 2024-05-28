import { Observable, tap } from 'rxjs'
import { ApiClient } from '@/services/apis/ApiClient'
import type { ResponseList } from '@/models/responses/response-list'
import { ApiServiceBase } from '@/services/apis/ApiServiceBase'
import { RequestQuery } from '@/models/requests/query/request-query'
import type { ResponseSector } from '@/models/responses/budgets/response-sector'

/**
 * Sector API Service
 */
export class SectorApiService extends ApiServiceBase {
  // API Client
  client: ApiClient = new ApiClient('/api/v1/Sector');

  /**
   * Get List
   * @param request Request
   */
  public getListAsync( request: RequestQuery = new RequestQuery() ) : Observable<ResponseList<ResponseSector>> {
    return this.client.restApi.requestGetAsync<ResponseList<ResponseSector>>(this.client.baseUrl, request)
      .pipe(
        tap((response) => {
            if (response.error)
              this.messageStore.showError(response.code, response.message);
          }),
      );
  };
}
