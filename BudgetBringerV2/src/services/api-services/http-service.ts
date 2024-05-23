import {map, Observable, tap} from "rxjs";
import {ajax} from "rxjs/internal/ajax/ajax";
import {communicationService} from "../CommunicationService";
import {ResponseData} from "../../models/responses/response-data";
import {messageService} from "../message-service";
import {RequestQuery} from "../../models/requests/query/request-query";

/**
 * http 클라이언트 요청 서비스
 */
export class HttpService {
  /**
   * Get RequestList
   * @param requestQuery
   */
  static requestListAsync<T> (requestQuery : RequestQuery): Observable<T> {
    return this.requestGet<T>(requestQuery.apiUri,requestQuery);
  }


  /**
   * GET 요청을 수행하는 메서드.
   * @param url 요청할 리소스의 URL
   * @param params 요청에 포함할 파라미터 오브젝트
   * @returns Observable로 요청 응답을 반환한다.
   */
  static requestGet<T>(url: string, params?: Record<string, any>): Observable<T> {
    const searchParams = new URLSearchParams();

    // params 객체를 반복하여 모든 키-값 쌍을 처리합니다.
    if (params) {
      for (const [key, value] of Object.entries(params)) {
        if (Array.isArray(value)) {
          // 값이 배열인 경우, 배열의 각 요소를 별도로 추가합니다.
          value.forEach(item => searchParams.append(key, item));
        } else {
          // 배열이 아닌 경우, 단일 값으로 추가합니다.
          searchParams.append(key, value);
        }
      }
    }

    const queryString = searchParams.toString() ? `?${searchParams}` : '';

    const fullUrl = `${url}${queryString}`;
    return ajax.getJSON<T>(fullUrl).pipe(
      tap(data=>{
        const response = data as ResponseData<any>;
        if (response.error) {
          messageService.showError(`[${response.code}] ${response.message}`);
        }
        communicationService.notifyOffCommunication();
      })
    );
  }
  static requestGetAutoNotify<T>(url: string, params?: Record<string, any>): Observable<T> {
    const searchParams = new URLSearchParams();
    communicationService.notifyInCommunication();

    // params 객체를 반복하여 모든 키-값 쌍을 처리합니다.
    if (params) {
      for (const [key, value] of Object.entries(params)) {
        if (Array.isArray(value)) {
          // 값이 배열인 경우, 배열의 각 요소를 별도로 추가합니다.
          value.forEach(item => searchParams.append(key, item));
        } else {
          // 배열이 아닌 경우, 단일 값으로 추가합니다.
          searchParams.append(key, value);
        }
      }
    }

    const queryString = searchParams.toString() ? `?${searchParams}` : '';
    const fullUrl = `${url}${queryString}`;
    return ajax.getJSON<T>(fullUrl).pipe(
      tap(data=>{
        const response = data as ResponseData<any>;
        if (response.error) {
          messageService.showError(`[${response.code}] ${response.message}`);
        }

        communicationService.notifyOffCommunication();
      })
    );
  }

  /**
   * GET request For Blob file
   * @param url
   * @param params
   */
  static requestGetFile(url: string, params?: Record<string, any>): Observable<Blob> {
    const searchParams = new URLSearchParams();

    // 파라미터 처리
    if (params) {
      for (const [key, value] of Object.entries(params)) {
        if (Array.isArray(value)) {
          value.forEach(item => searchParams.append(key, item));
        } else {
          searchParams.append(key, value);
        }
      }
    }

    const queryString = searchParams.toString() ? `?${searchParams}` : '';
    const fullUrl = `${url}${queryString}`;

    return ajax({
      url: fullUrl,
      method: 'GET',
      responseType: 'blob'
    }).pipe(
      map(response => response.response)
    ) as Observable<Blob>;
  }
  static requestGetFileAutoNotify(url: string, params?: Record<string, any>): Observable<Blob> {
    communicationService.notifyInCommunication();
    const searchParams = new URLSearchParams();

    // 파라미터 처리
    if (params) {
      for (const [key, value] of Object.entries(params)) {
        if (Array.isArray(value)) {
          value.forEach(item => searchParams.append(key, item));
        } else {
          searchParams.append(key, value);
        }
      }
    }

    const queryString = searchParams.toString() ? `?${searchParams}` : '';
    const fullUrl = `${url}${queryString}`;

    return ajax({
      url: fullUrl,
      method: 'GET',
      responseType: 'blob'
    }).pipe(
      // Before Transfer
      tap(() => {
        communicationService.notifyOffCommunication();
      }),
      // To Transfer
      map(response => response.response)
    ) as Observable<Blob>;
  }

  /**
   * POST 요청을 수행하는 메서드.
   * @param url 요청할 리소스의 URL
   * @param body 요청에 포함할 본문 데이터
   * @param headers 요청에 포함할 추가 헤더 (선택사항)
   * @returns Observable로 요청 응답을 반환한다.
   */
  static requestPost<T>(url: string, body: any, headers?: Record<string, string>): Observable<T> {
    // FormData인 경우 Content-Type 헤더를 자동으로 설정하도록 함
    const effectiveHeaders = body instanceof FormData ? headers : { 'Content-Type': 'application/json', ...headers };
    return ajax.post(url, body, effectiveHeaders)
    .pipe(
      map(response => response.response as T)
    );
  }
  static requestPostAutoNotify<T>(url: string, body: any, headers?: Record<string, string>): Observable<T> {
    // FormData인 경우 Content-Type 헤더를 자동으로 설정하도록 함
    const effectiveHeaders = body instanceof FormData ? headers : { 'Content-Type': 'application/json', ...headers };
    communicationService.notifyInCommunication();
    return ajax.post(url, body, effectiveHeaders)
    .pipe(
      map(response => response.response as T) ,
      tap(data => {
        const response = data as ResponseData<any>;
        if (response.error)
          messageService.showError(`[${response.code}] ${response.message}`);

        if(response.success)
          messageService.showSuccess("데이터가 추가되었습니다.");
        communicationService.notifyOffCommunication();
      })
    );
  }

  /**
   * PUT 요청을 수행하는 메서드.
   * @param url 요청할 리소스의 URL
   * @param body 요청에 포함할 본문 데이터
   * @param headers 요청에 포함할 추가 헤더 (선택사항)
   * @returns Observable로 요청 응답을 반환한다.
   */
  static requestPut<T>(url: string, body: any, headers?: Record<string, string>): Observable<T> {
    return ajax.put(url, body, { ...headers, 'Content-Type': 'application/json' })
    .pipe(
      map(response => response.response as T)
    );
  }
  static requestPutAutoNotify<T>(url: string, body: any, headers?: Record<string, string>): Observable<T> {
    communicationService.notifyInCommunication();
    return ajax.put(url, body, { ...headers, 'Content-Type': 'application/json' })
    .pipe(
      map(response => response.response as T) ,
      tap(data => {
        const response = data as ResponseData<any>;
        if (response.error)
          messageService.showError(`[${response.code}] ${response.message}`);

        if(response.success)
          messageService.showSuccess("데이터가 수정되었습니다.");
        communicationService.notifyOffCommunication();
      })
    );
  }

  /**
   * DELETE 요청을 수행하는 메서드.
   * @param url 요청할 리소스의 URL
   * @param headers 요청에 포함할 추가 헤더 (선택사항)
   * @returns Observable로 요청 응답을 반환한다.
   */
  static requestDelete<T>(url: string, headers?: Record<string, string>): Observable<T> {
    return ajax.delete(url, headers)
    .pipe(
      map(response => response.response as T)
    );
  }
  // static requestDeleteAutoNotify<T>(url: string, body: any, headers?: Record<string, string>): Observable<T> {
  //   communicationService.notifyInCommunication();
  //   return ajax.delete(url, headers)
  //   .pipe(
  //     map(response => response.response as T) ,
  //     tap(data => {
  //       const response = data as ResponseData<any>;
  //       if (response.error)
  //         messageService.showError(`[${response.code}] ${response.message}`);
  //
  //       if(response.success)
  //         messageService.showSuccess("데이터가 삭제되었습니다.");
  //       communicationService.notifyOffCommunication();
  //     })
  //   );
  // }
}
