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
        field: "attachedFiles",
        headerClass: '',
        headerName:"📁 첨부파일",
        floatingFilter: false,
        width:120,
        filter: 'agTextColumnFilter',
        cellRenderer: function(params) {
          const eCell = document.createElement('span');

          if(params.value?.length > 0) {
            eCell.textContent = params.value ? `${params.value.length} 파일` : '-';
          }
          else {
            eCell.textContent = params.value ? `-` : '-';
          }

          eCell.classList.add('file-link');

          let tooltip = null; // 툴팁을 저장할 변수

          eCell.onclick = (event) => {
            if(params.value === null || params.value.length === 0)
              return;

            if (tooltip === null) { // 툴팁이 이미 존재하지 않는 경우에만 생성
              tooltip = document.createElement('div');
              tooltip.classList.add('custom-tooltip');
              tooltip.style.cssText = `
                    position: absolute;
                    left: ${event.clientX - 30}px;
                    top: ${event.clientY }px;
                    background-color: white;
                    border:1px solid black;
                    color: grey;
                    border-radius: 10px;
                    z-index: 100;
                    padding:20px;
                    max-width: 500px; // 최대 너비 설정
                `;
              // 파일 목록을 링크로 생성
              const fileList = document.createElement('ul');
              fileList.style.listStyleType = 'none';
              fileList.style.padding = '0';
              fileList.style.margin = '0';
              params.value.forEach(file => {
                const fileItem = document.createElement('li');
                const fileLink = document.createElement('a');
                fileLink.textContent = file.name;
                fileLink.style.color = 'grey';
                fileLink.style.textDecoration = 'none';
                fileLink.style.display = 'block';
                fileLink.style.cursor = 'pointer';
                fileLink.style.padding = '5px';
                fileLink.style.overflow = 'hidden';
                fileLink.style.textOverflow = 'ellipsis';
                fileLink.style.whiteSpace = 'nowrap'; // 공백 무시
                fileLink.style.width = '100%'; // 링크 너비를 최대로 설정
                fileLink.onclick = function(event) {
                  event.preventDefault(); // 기본 링크 동작 방지
                  window.open('/'+file.url, '_blank'); // 새 탭에서 파일 다운로드
                };

                fileItem.appendChild(fileLink);
                fileList.appendChild(fileItem);
              });

              tooltip.appendChild(fileList);
              document.body.appendChild(tooltip);

              // 다른 요소를 클릭했을 때 툴팁을 제거하는 이벤트 리스너 추가
              document.addEventListener('click', function onClickOutside(event) {
                const target = event.target;

                if (!tooltip.contains(target) && !eCell.contains(target as HTMLTableCellElement)) {
                  removeTooltip();
                  document.removeEventListener('click', onClickOutside);
                }
              });
            }
          };

          // 툴팁을 제거하는 함수
          function removeTooltip() {
            if (tooltip) {
              document.body.removeChild(tooltip);
              tooltip = null;
            }
          }

          return eCell;
        },
        cellClass: 'files-cell'
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
    ]
    this.items = [];

  }
}
