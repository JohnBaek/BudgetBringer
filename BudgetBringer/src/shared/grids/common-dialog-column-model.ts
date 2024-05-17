import {RequestQuery} from "../../models/requests/query/request-query";

/**
 * Dialog and Grid Model Column
 */
export class CommonDialogColumnModel {
  /**
   * constructor
   * @param field
   * @param headerName
   * @param width
   */
  constructor(field: string, headerName: string, width: number) {
    this.field = field;
    this.headerName = headerName;
    this.width = width;
  }

  // [Grid] field
  field: string = '';
  icon:string = '';
  // Model PropertyName
  modelPropertyName: string = '';
  // [Grid] header class
  headerClass: string = "ag-grids-custom-header";
  // [Grid] header name
  headerName: string = '';
  // [Grid] filter delegate
  filter = "agTextColumnFilter";
  // [Grid] filter params
  filterParams = {
    filterOptions: ["포함하는"],
    maxNumConditions: 1,
  };
  // [Grid] use floating filter
  floatingFilter: boolean = true;
  // [Grid] width
  width: number = 150;
  // [Grid] cell class
  cellClass: string = '';
  // [Model] Is use as Model?
  useAsModel: boolean = false;
  // [Model] type of Model ( ex: select, text ... )
  inputType: EnumModelType = EnumModelType.text;
  // [Model] is Required Property?
  isRequired: boolean = false;
  // [Model:selectList]
  selectItems: Array<any> = [];
  // [Model:selectList] title property
  selectTitle: string = '';
  // [Model:selectList] value property
  selectValue: string = '';
  // [Model:selectList] Is need to Async initialize?
  selectShouldInitAsync: boolean = false;
  // [Model:selectList] selectShouldInitAsync need to this object
  selectRequestQuery: RequestQuery = null;
  cellRenderer;
  cellStyle;
  valueFormatter;
}

export enum EnumModelType {
  text ,
  select ,
  number ,
}


