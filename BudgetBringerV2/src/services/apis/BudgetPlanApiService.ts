import { finalize, Observable, tap } from 'rxjs'
import { ApiClient } from '@/services/apis/ApiClient'
import type { RequestQuery } from '@/models/requests/query/request-query'
import type { ResponseBudgetPlan } from '@/models/responses/budgets/response-budget-plan'
import type { ResponseList } from '@/models/responses/response-list'
import { ApiServiceBase } from '@/services/apis/ApiServiceBase'

/**
 * BudgetPlan API Service
 */
export class BudgetPlanApiService extends ApiServiceBase  {
  // API Client
  client: ApiClient = new ApiClient('/api/v1/BudgetPlan');

  /**
   * Get List
   * @param request Request
   */
  public getListAsync( request: RequestQuery ) : Observable<ResponseList<ResponseBudgetPlan>> {
    this.communicationStore.inTransmission();
    return this.client.restApi.requestGetAsync<ResponseList<ResponseBudgetPlan>>(this.client.baseUrl, request)
      .pipe(
        tap((response) => {
            if (response.error)
              this.messageStore.showError(response.code, response.message);
          }),
        finalize(() => {
          this.communicationStore.offTransmission();
        })
      );
  };
}
