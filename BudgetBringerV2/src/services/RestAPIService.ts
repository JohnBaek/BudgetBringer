import { finalize, map, Observable, tap } from 'rxjs'
import { ajax } from 'rxjs/internal/ajax/ajax'
import type { ResponseData } from '@/models/responses/response-data'
import { useMessageStore } from '@/services/state-managements/MessageStore'
import { useCommunicationStore } from '@/services/state-managements/CommunicationStore'
import type { AjaxResponse } from 'rxjs/internal/ajax/AjaxResponse'
import { type IResponse, Response } from '@/models/responses/response'

/**
 * Service for REST API
 */
export class RestAPIService {
  // Message Store
  private readonly messageStore = useMessageStore();

  // Communication Store
  private readonly communicationStore = useCommunicationStore();

  /**
   * Get Request to Server
   * @param url API URI
   * @param params Parameters
   */
  public requestGetAsync<T>(url: string, params?: Record<string, any>): Observable<T> {
    const uri = this.getParsedKeyValueParameter(url, params);
    let observable = new Observable<T>();
    try {
      observable = ajax.getJSON<T>(uri).pipe(map(response => response as T) ,
        tap(data => {
          const response = data as ResponseData<T>;
          if(response.error)
            this.messageStore.showError(response.code , response.message);
        }));
    }catch (ex) {
      console.error(ex);
    }
    return observable;
  }

  /**
   * Get Request to Server
   * @param url API URI
   * @param params Parameters
   */
  public requestGetAsyncAutoCommunications<T>(url: string, params?: Record<string, any>): Observable<T> {
    // Turn on communicate
    this.communicationStore.inCommunication();

    let observable = new Observable<T>();
    try {
      observable = this.requestGetAsync(url, params)
      .pipe(
        map(response => response as T) ,
        tap(data => {
          const response = data as ResponseData<T>;
          if(response.error)
            this.messageStore.showError(response.code , response.message);
        }) ,
        finalize(() => {
          // Turn off communicate
          this.communicationStore.offCommunication();
        })
      );
    }catch (ex) {
      console.error(ex);
    }
    return observable;
  }

  /**
   * Post Request to Server
   * @param url API URI
   * @param body body
   * @param headers headers
   */
  public requestPostAsync<T>(url: string, body: any, headers?: Record<string, string>): Observable<T> {
    const effectiveHeaders = body instanceof FormData ? headers : { 'Content-Type': 'application/json', ...headers };
    let observable = new Observable<T>();
    try {
      observable = ajax.post<T>(url, body, effectiveHeaders)
        .pipe(
          map(response => response as T) ,
          tap(data => {
            const response = data as ResponseData<T>;
            if(response.error)
              this.messageStore.showError(response.code , response.message);
          }),
        );
    }catch (ex) {
      console.error(ex);
    }
    return observable;
  }

  /**
   * Post Request to Server
   * @param url API URI
   * @param body body
   * @param headers headers
   */
  public requestPostAsyncAutoCommunications<T>(url: string, body: any, headers?: Record<string, string>): Observable<T> {
    // Turn on communicate
    this.communicationStore.inCommunication();
    const effectiveHeaders = body instanceof FormData ? headers : { 'Content-Type': 'application/json', ...headers };
    let observable: Observable<T> = new Observable<T>();
    try {
      observable = ajax.post<T>(url, body, effectiveHeaders)
      .pipe(
        map((response: AjaxResponse<T>) => response.response),
        tap((data: T)=> {
          const response = (data as Response);
          if(response.error)
            this.messageStore.showError(response.code , response.message);
        }),
        finalize(() => {
          // Turn off communicate
          this.communicationStore.offCommunication();
        })
      );
    }catch (ex) {
      console.error(ex);
    }
    return observable;
  }

  //
  // /**
  //  * GET request For Blob file
  //  * @param url
  //  * @param params
  //  */
  // static requestGetFile(url: string, params?: Record<string, any>): Observable<Blob> {
  //   const searchParams = new URLSearchParams();
  //
  //   // 파라미터 처리
  //   if (params) {
  //     for (const [key, value] of Object.entries(params)) {
  //       if (Array.isArray(value)) {
  //         value.forEach(item => searchParams.append(key, item));
  //       } else {
  //         searchParams.append(key, value);
  //       }
  //     }
  //   }
  //
  //   const queryString = searchParams.toString() ? `?${searchParams}` : '';
  //   const fullUrl = `${url}${queryString}`;
  //
  //   return ajax({
  //     url: fullUrl,
  //     method: 'GET',
  //     responseType: 'blob'
  //   }).pipe(
  //     map(response => response.response)
  //   ) as Observable<Blob>;
  // }
  // static requestGetFileAutoNotify(url: string, params?: Record<string, any>): Observable<Blob> {
  //   communicationService.notifyInCommunication();
  //   const searchParams = new URLSearchParams();
  //
  //   // 파라미터 처리
  //   if (params) {
  //     for (const [key, value] of Object.entries(params)) {
  //       if (Array.isArray(value)) {
  //         value.forEach(item => searchParams.append(key, item));
  //       } else {
  //         searchParams.append(key, value);
  //       }
  //     }
  //   }
  //
  //   const queryString = searchParams.toString() ? `?${searchParams}` : '';
  //   const fullUrl = `${url}${queryString}`;
  //
  //   return ajax({
  //     url: fullUrl,
  //     method: 'GET',
  //     responseType: 'blob'
  //   }).pipe(
  //     // Before Transfer
  //     tap(() => {
  //       communicationService.notifyOffCommunication();
  //     }),
  //     // To Transfer
  //     map(response => response.response)
  //   ) as Observable<Blob>;
  // }
  //
  // /**
  //  * POST 요청을 수행하는 메서드.
  //  * @param url 요청할 리소스의 URL
  //  * @param body 요청에 포함할 본문 데이터
  //  * @param headers 요청에 포함할 추가 헤더 (선택사항)
  //  * @returns Observable로 요청 응답을 반환한다.
  //  */
  // static requestPost<T>(url: string, body: any, headers?: Record<string, string>): Observable<T> {
  //   // FormData인 경우 Content-Type 헤더를 자동으로 설정하도록 함
  //   const effectiveHeaders = body instanceof FormData ? headers : { 'Content-Type': 'application/json', ...headers };
  //   return ajax.post(url, body, effectiveHeaders)
  //   .pipe(
  //     map(response => response.response as T)
  //   );
  // }
  // static requestPostAutoNotify<T>(url: string, body: any, headers?: Record<string, string>): Observable<T> {
  //   // FormData인 경우 Content-Type 헤더를 자동으로 설정하도록 함
  //   const effectiveHeaders = body instanceof FormData ? headers : { 'Content-Type': 'application/json', ...headers };
  //   communicationService.notifyInCommunication();
  //   return ajax.post(url, body, effectiveHeaders)
  //   .pipe(
  //     map(response => response.response as T) ,
  //     tap(data => {
  //       const response = data as ResponseData<any>;
  //       if (response.error)
  //         messageService.showError(`[${response.code}] ${response.message}`);
  //
  //       if(response.success)
  //         messageService.showSuccess("데이터가 추가되었습니다.");
  //       communicationService.notifyOffCommunication();
  //     })
  //   );
  // }
  //
  // /**
  //  * PUT 요청을 수행하는 메서드.
  //  * @param url 요청할 리소스의 URL
  //  * @param body 요청에 포함할 본문 데이터
  //  * @param headers 요청에 포함할 추가 헤더 (선택사항)
  //  * @returns Observable로 요청 응답을 반환한다.
  //  */
  // static requestPut<T>(url: string, body: any, headers?: Record<string, string>): Observable<T> {
  //   return ajax.put(url, body, { ...headers, 'Content-Type': 'application/json' })
  //   .pipe(
  //     map(response => response.response as T)
  //   );
  // }
  // static requestPutAutoNotify<T>(url: string, body: any, headers?: Record<string, string>): Observable<T> {
  //   communicationService.notifyInCommunication();
  //   return ajax.put(url, body, { ...headers, 'Content-Type': 'application/json' })
  //   .pipe(
  //     map(response => response.response as T) ,
  //     tap(data => {
  //       const response = data as ResponseData<any>;
  //       if (response.error)
  //         messageService.showError(`[${response.code}] ${response.message}`);
  //
  //       if(response.success)
  //         messageService.showSuccess("데이터가 수정되었습니다.");
  //       communicationService.notifyOffCommunication();
  //     })
  //   );
  // }
  //
  // /**
  //  * DELETE 요청을 수행하는 메서드.
  //  * @param url 요청할 리소스의 URL
  //  * @param headers 요청에 포함할 추가 헤더 (선택사항)
  //  * @returns Observable로 요청 응답을 반환한다.
  //  */
  // static requestDelete<T>(url: string, headers?: Record<string, string>): Observable<T> {
  //   return ajax.delete(url, headers)
  //   .pipe(
  //     map(response => response.response as T)
  //   );
  // }
  // // static requestDeleteAutoNotify<T>(url: string, body: any, headers?: Record<string, string>): Observable<T> {
  // //   communicationService.notifyInCommunication();
  // //   return ajax.delete(url, headers)
  // //   .pipe(
  // //     map(response => response.response as T) ,
  // //     tap(data => {
  // //       const response = data as ResponseData<any>;
  // //       if (response.error)
  // //         messageService.showError(`[${response.code}] ${response.message}`);
  // //
  // //       if(response.success)
  // //         messageService.showSuccess("데이터가 삭제되었습니다.");
  // //       communicationService.notifyOffCommunication();
  // //     })
  // //   );
  // // }
  //
  /**
   * get Key/Value parameters to URLSearchParams
   * @param url API End point
   * @param params Key/Value
   */
  private getParsedKeyValueParameter (url: string,params?: Record<string, any>) : string {
    const searchParams = new URLSearchParams();

    // params is null
    if(!params)
      return url;

    // Process all property
    for (const [key, value] of Object.entries(params)) {
      if (Array.isArray(value)) {
        value.forEach(item => searchParams.append(key, item));
      } else {
        searchParams.append(key, value);
      }
    }
    const query = searchParams.toString() ? `?${searchParams}` : '';
    return `${url}${query}`
  }
}