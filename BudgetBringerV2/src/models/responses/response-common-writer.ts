/**
 * With Common Writer Response
 */
export interface ResponseCommonWriter {
  // 등록일
  regDate: Date | null;

  // 수정일
  modDate: Date | null;

  // 등록자명
  regName: string | null;

  // 수정자명
  modName: string | null;
}
