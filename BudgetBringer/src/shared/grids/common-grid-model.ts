/**
 * Common Grid 모델 정의
 */
export abstract class CommonGridModel<T> {
  /**
   * 표현할 그리드의 RowData 를 받는다.
   */
  items : Array<T>;
  /**
   * 컬럼정보
   */
  columDefined : any [];
  /**
   * Insert 그리드 사용여부
   */
  isUseInsert : boolean;


  /**
   * 넘버 포매터
   * @param params
   */
  numberValueFormatter = (params) => {
    // 넘버형식인 경우
    if( typeof params.value === 'number')
      return new Intl.NumberFormat('en-US', { style: 'decimal', maximumFractionDigits: 0 }).format(params.value);

    // 그 외
    return 0;
  }
}
