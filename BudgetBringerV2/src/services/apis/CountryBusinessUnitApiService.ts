import { Observable, tap } from 'rxjs'
import { ApiClient } from '@/services/apis/ApiClient'
import type { RequestQuery } from '@/models/requests/query/request-query'
import type { ResponseList } from '@/models/responses/response-list'
import type { ResponseBusinessUnit } from '@/models/responses/budgets/response-business-unit'
import { ApiServiceBase } from '@/services/apis/ApiServiceBase'

/**
 * BudgetPlan API Service
 */
export class BusinessUnitApiService extends ApiServiceBase{
  // API Client
  client: ApiClient = new ApiClient('/api/v1/BusinessUnits');

  /**
   * Get List
   * @param request Request
   */
  public getListAsync( request: RequestQuery ) : Observable<ResponseList<ResponseBusinessUnit>> {
    return this.client.restApi.requestGetAsync<ResponseList<ResponseBusinessUnit>>(this.client.baseUrl, request)
      .pipe(
        tap((response) => {
            if (response.error)
              this.messageStore.showError(response.code, response.message);
          }),
      );
  };
}
