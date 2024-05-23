
export class baseApiService {
  baseUri : string;

  /**
   * 생성자
   * @param baseUri
   */
  constructor(baseUri: string) {
    this.baseUri = baseUri;
  }

  /**
   * baseURI
   */
  getBaseUri() {
    return this.baseUri;
  }
}
