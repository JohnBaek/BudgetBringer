import { Observable, tap } from 'rxjs'
import { ApiClient } from '@/services/apis/ApiClient'
import type { RequestQuery } from '@/models/requests/query/request-query'
import type { ResponseList } from '@/models/responses/response-list'
import { useMessageStore } from '@/services/stores/MessageStore'
import { useCommunicationStore } from '@/services/stores/CommunicationStore'
import type { ResponseCostCenter } from '@/models/responses/budgets/response-cost-center'
import { ApiServiceBase } from '@/services/apis/ApiServiceBase'

/**
 * BudgetPlan API Service
 */
export class CostCenterApiServiceApiService extends ApiServiceBase{
  // API Client
  client: ApiClient = new ApiClient('/api/v1/CostCenter');

  /**
   * Get List
   * @param request Request
   */
  public getListAsync( request: RequestQuery ) : Observable<ResponseList<ResponseCostCenter>> {
    return this.client.restApi.requestGetAsync<ResponseList<ResponseCostCenter>>(this.client.baseUrl, request)
      .pipe(
        tap((response) => {
            if (response.error)
              this.messageStore.showError(response.code, response.message);
          }),
      );
  };
}
