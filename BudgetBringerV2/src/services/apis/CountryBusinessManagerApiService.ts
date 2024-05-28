import { Observable, tap } from 'rxjs'
import { ApiClient } from '@/services/apis/ApiClient'
import type { ResponseList } from '@/models/responses/response-list'
import { ApiServiceBase } from '@/services/apis/ApiServiceBase'
import { RequestQuery } from '@/models/requests/query/request-query'
import type { ResponseCountryBusinessManager } from '@/models/responses/budgets/response-country-business-manager'

/**
 * BudgetPlan API Service
 */
export class CountryBusinessManagerApiService extends ApiServiceBase{
  // API Client
  client: ApiClient = new ApiClient('/api/v1/CountryBusinessManager');

  /**
   * Get List
   * @param request Request
   */
  public getListAsync( request: RequestQuery = new RequestQuery() ) : Observable<ResponseList<ResponseCountryBusinessManager>> {
    return this.client.restApi.requestGetAsync<ResponseList<ResponseCountryBusinessManager>>(this.client.baseUrl, request)
      .pipe(
        tap((response) => {
            if (response.error)
              this.messageStore.showError(response.code, response.message);
          }),
      );
  };
}
