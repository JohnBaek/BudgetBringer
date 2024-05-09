import {EnumApprovalStatus} from "../../models/enums/enum-approval-status";

/**
 * Grid Common column definitions
 */
export namespace CommonColumnDefinitions {
  export const getBaseYearColumn = () => ({
    field: 'baseYearForStatistics',
    headerName: '통계 기준년도',
    headerClass: 'ag-grids-custom-header',
    filter: 'agTextColumnFilter',
    floatingFilter: true,
    width: 160
  });

  export const getRegDateColumn = () => ({
    field: "regDate",
    headerClass: 'ag-grids-custom-header',
    headerName:"등록일"  ,
    filter: "agTextColumnFilter",
    filterParams: {
      filterOptions: ["포함하는"],
      maxNumConditions: 1,
    },
    floatingFilter: true,
    width:250,
    valueFormatter: function(params) {
      if (params.value) {
        const date = new Date(params.value);
        return date.toLocaleString('ko-KR', {
          year: 'numeric',
          month: '2-digit',
          day: '2-digit',
          hour: '2-digit',
          minute: '2-digit',
          second: '2-digit',
        }).replace(/(\. )|(\.,)/g, ' ');
      }
      return null;
    }
  })

  export const getRegNameColumn = () => CommonColumnDefinitions.createColumnDefinitionForTextFilter(250, 'regName' , '등록자명');

  export const getNameColumn = () => ({
    field: 'name',
    headerName: '이름',
    headerClass: 'ag-grids-custom-header',
    filter: 'agTextColumnFilter',
    floatingFilter: true,
    width: 250
  });

  export const getIsIncludeInStatistics =() => ({
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
  });

  export const getApprovalDate = () => CommonColumnDefinitions.createColumnDefinitionForTextFilter(145, "approvalDate", "Approval Date");

  export const getAttachedFiles = () => ({
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
  });

  export const getApprovalStatus =() => ({
    field: "approvalStatus",
    headerClass: 'ag-grids-custom-header',
    headerName:"ApprovalStatus"  ,
    width:160,
    cellRenderer: (params) => {
      switch (params.value) {
        case EnumApprovalStatus.None:
          return "None";
        case EnumApprovalStatus.PoNotYetPublished:
          return "Po Not Yet Published";
        case EnumApprovalStatus.PoPublished:
          return "Po Published";
        case EnumApprovalStatus.InVoicePublished:
          return "In Voice Published";
        default:
          return "Error"; // 값이 열거형에 없는 경우
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
    }
  });

  export const getDescription = () => ({
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
  });

  export const getSector = () => CommonColumnDefinitions.createColumnDefinitionForTextFilter(150, "sectorName", "Sector");
  export const getBusinessUnit = () => CommonColumnDefinitions.createColumnDefinitionForTextFilter(150, "businessUnitName", "Business Unit");
  export const getCostCenter = () => CommonColumnDefinitions.createColumnDefinitionForTextFilter(150, "costCenterName", "Cost Center");
  export const getCountryBusinessManager = () => CommonColumnDefinitions.createColumnDefinitionForTextFilter(230, "countryBusinessManagerName", "Country Business Manager");

  /**
   * Generate Column Definitions
   * @param width
   * @param field
   * @param headerName
   * @param valueFormatter
   * @param isUseInChart
   * @param cellRenderer
   */
  export const createColumnDefinitionForNumberFilter = (width: number , field: string , headerName: string, valueFormatter?: {}  , isUseInChart: boolean = true, cellRenderer:any = null) => {
    return {
      field: field,
      headerClass: 'ag-grids-custom-header',
      headerName: headerName  ,
      valueFormatter: valueFormatter,
      width: width,
      isUseInChart: isUseInChart ,
      cellRenderer: cellRenderer
    }
  }

  export const createColumnDefinitionForTextFilter = (width: number , field: string , headerName: string, valueFormatter?: {} , isFloatingFilter: boolean = true, isUseInChart: boolean = true) => {
    return {
      field: field,
      headerClass: 'ag-grids-custom-header',
      filter: 'agTextColumnFilter' ,
      headerName: headerName  ,
      valueFormatter: valueFormatter,
      width: width,
      floatingFilter: isFloatingFilter,
      isUseInChart: isUseInChart ,
    }
  }
}

