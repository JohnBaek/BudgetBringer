import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {ResponseBudgetApproved} from "../../../models/responses/budgets/response-budget-approved";
import {EnumApprovalStatus} from "../../../models/enums/enum-approval-status";

/**
 * 예산 그리드 모델
 */
export class BudgetApprovedGridData extends CommonGridModel{
  /**
   * 표현할 그리드의 RowData 를 받는다.
   */
  items : Array<ResponseBudgetApproved>;
  /**
   * 컬럼정보
   */
  columDefined : any [];
  /**
   * 생성자
   */
  constructor() {
    super();
    this.columDefined = [
      // 승인일
      {
        field: "approvalDate",
        headerClass: 'ag-grids-custom-header',
        headerName:"Approval Date" ,
        showDisabledCheckboxes: true,
        filter: 'agTextColumnFilter',
        floatingFilter: true,
        width:145,
      },
      {
        field: "approvalStatus",
        headerClass: 'ag-grids-custom-header',
        headerName:"ApprovalStatus"  ,
        width:160,
        cellRenderer: (params) => {
          switch (params.value) {
            case EnumApprovalStatus.None:
              return "상태없음";
            case EnumApprovalStatus.PoNotYetPublished:
              return "세금계산서 발행 전";
            case EnumApprovalStatus.PoPublished:
              return "세금계산서 발행";
            case EnumApprovalStatus.InVoicePublished:
              return "인보이스 발행";
            default:
              return "알 수 없는 상태"; // 값이 열거형에 없는 경우
          }
        },
        cellStyle: (params) => {
          switch (params.value) {
            case EnumApprovalStatus.None:
              return { backgroundColor: '#33CC3344', color: 'light-black' };
            case EnumApprovalStatus.PoNotYetPublished:
              return { backgroundColor: '#ccc42244', color: 'light-black' };
            case EnumApprovalStatus.PoPublished:
              return { backgroundColor: '#33CC3344', color: 'light-black' };
            case EnumApprovalStatus.InVoicePublished:
              return { backgroundColor: '#2244CC44', color: 'light-black' };
            default:
              return { backgroundColor: '#CC222244', color: 'light-black' }; // 값이 열거형에 없는 경우
          }
        },
        //
        // cellClassRules: {
        //   "rag-green": "value === EnumApprovalStatus.None || value === EnumApprovalStatus.PoPublished",
        //   "rag-yellow": "value === EnumApprovalStatus.PoNotYetPublished",
        //   "rag-blue": "value === EnumApprovalStatus.InVoicePublished",
        //   "rag-red": "value !== EnumApprovalStatus.None && value !== EnumApprovalStatus.PoNotYetPublished && value !== EnumApprovalStatus.PoPublished && value !== EnumApprovalStatus.InVoicePublished"
        // }

      },
      // 설명
      {
        field: "description",
        headerClass: 'ag-grids-custom-header',
        headerName:"Description"  ,
        filter: "agTextColumnFilter",
        filterParams: {
          filterOptions: ["포함하는"],
          maxNumConditions: 1,
        },
        floatingFilter: true,
        width:250,
      },
      // 섹터
      {
        field: "sectorName",
        headerClass: 'ag-grids-custom-header',
        headerName:"Sector",
        floatingFilter: true,
        width:100,
        filter: 'agTextColumnFilter',
      },
      // 부서
      {
        field: "businessUnitName",
        headerClass: 'ag-grids-custom-header',
        headerName:"BU",
        width:100,
        floatingFilter: true,
        filter: 'agTextColumnFilter',
      },
      // CC
      {
        field: "costCenterName",
        headerClass: 'ag-grids-custom-header',
        headerName:"CC"  ,
        width:100,
        floatingFilter: true,
        filter: 'agTextColumnFilter',
      },
      // 국가별 매니저
      {
        field: "countryBusinessManagerName",
        headerClass: 'ag-grids-custom-header',
        headerName:"CBM"  ,
        width:130,
        floatingFilter: true,
        filter: 'agTextColumnFilter',
      },
      {
        field: "actual",
        headerClass: 'ag-grids-custom-header',
        headerName:"Actual"  ,
        valueFormatter: this.numberValueFormatter,
        width:130,
      },
      {
        field: "approvalAmount",
        headerClass: 'ag-grids-custom-header',
        headerName:"ApprovalAmount"  ,
        valueFormatter: this.numberValueFormatter,
        width:130,
      },
      {
        field: "poNumber",
        headerClass: 'ag-grids-custom-header',
        headerName:"PoNumber"  ,
        filter: "agTextColumnFilter",
        filterParams: {
          filterOptions: ["포함하는"],
          maxNumConditions: 1,
        },
        floatingFilter: true,
        width:130,
      },
      {
        field: "ocProjectName",
        headerClass: 'ag-grids-custom-header',
        headerName:"OcProjectName"  ,
        filter: "agTextColumnFilter",
        filterParams: {
          filterOptions: ["포함하는"],
          maxNumConditions: 1,
        },
        floatingFilter: true,
        width:250,
      },
      {
        field: "bossLineDescription",
        headerClass: 'ag-grids-custom-header',
        headerName:"BossLineDescription"  ,
        filter: "agTextColumnFilter",
        filterParams: {
          filterOptions: ["포함하는"],
          maxNumConditions: 1,
        },
        floatingFilter: true,
        width:250,
      },

      // {
      //   field: "regDate",
      //   headerClass: 'ag-grids-custom-header',
      //   headerName:"Reg Date"  ,
      //   filter: "agTextColumnFilter",
      //   filterParams: {
      //     filterOptions: ["포함하는"],
      //     maxNumConditions: 1,
      //   },
      //   floatingFilter: true,
      //   width:150,
      //   valueFormatter: function(params) {
      //     if (params.value) {
      //       const date = new Date(params.value);
      //       return date.toLocaleString('ko-KR', {
      //         year: 'numeric',
      //         month: '2-digit',
      //         day: '2-digit',
      //         hour: '2-digit',
      //         minute: '2-digit',
      //         second: '2-digit',
      //       }).replace(/(\. )|(\.,)/g, ' ');
      //     }
      //     return null;
      //   }
      // },
      // {
      //   field: "regName",
      //   headerClass: 'ag-grids-custom-header',
      //   headerName:"Registration Name"  ,
      //   filter: "agTextColumnFilter",
      //   filterParams: {
      //     filterOptions: ["포함하는"],
      //     maxNumConditions: 1,
      //   },
      //   floatingFilter: true,
      //   width:120,
      // },
      // 예산

    ]
    this.items = [];

  }
}
