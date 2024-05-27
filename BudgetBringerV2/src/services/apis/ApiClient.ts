import { RestAPIService } from '@/services/RestAPIService'

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
}