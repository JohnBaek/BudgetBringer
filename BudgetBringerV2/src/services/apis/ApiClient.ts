import { RestAPIService } from '@/services/RestAPIService'
import type { Observable } from 'rxjs'
import { RequestQuery } from '@/models/requests/query/request-query'

export class ApiClient {
  public baseUrl: string;
  public restApi: RestAPIService = new RestAPIService();

  /**
   * Constructor
   * @param baseUrl endpoint URL
   */
  constructor(baseUrl: string) {
    this.baseUrl = baseUrl
  }

  /**
   * Get Request to Server
   */
  public requestGetAsync<T>(query: RequestQuery): Observable<T> {
    return this.restApi.requestGetAsync(this.baseUrl, query);
  }
}