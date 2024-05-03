import {CommonGridModel} from "../../../shared/grids/common-grid-model";

/**
 * 예산 그리드 모델
 */
export class BudgetPlanGridData extends CommonGridModel{
  /**
   * 컬럼정보
   */
  columDefined : any [];
  /**
   * Insert 그리드 사용여부
   */
  isUseInsert : boolean;
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
        width:160,
      },
      // 승인일
      {
        field: "isIncludeInStatistics",
        headerClass: 'ag-grids-custom-header',
        headerName:"통계포함여부" ,
        width:130,
        cellRenderer: (params) => {
          if(params.value)
            return '포함';
          else
            return '미포함';
        },
        cellStyle: (params) => {
          if(params.value)
            return { backgroundColor: '#33CC3344', color: 'light-black' };
          else
            return { backgroundColor: '#CC222244', color: 'light-black' };
        },
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
      // 예산
      {
        field: "budgetTotal",
        headerClass: 'ag-grids-custom-header',
        headerName:"FvBudget"  ,
        valueFormatter: this.numberValueFormatter,
      },

    ]
    this.isUseInsert = false;
  }
}

