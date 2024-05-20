import {EnumApprovalStatus, GetApprovalStatusList} from "../../models/enums/enum-approval-status";
import {getYearList} from "../../services/utils/date-util";
import {SectorGridData} from "../../pages/common-code/sector/sector-grid-data";
import {CommonDialogColumnModel, EnumModelType} from "./common-dialog-column-model";
import {BusinessUnitGridData} from "../../pages/common-code/business-unit/business-unit-grid-data";
import {CostCenterGridData} from "../../pages/common-code/cost-center/cost-center-grid-data";
import {
  CountryBusinessManagerGridData
} from "../../pages/common-code/country-business-manager/country-business-manager-grid-data";
import {CommonGridModel} from "./common-grid-model";


/**
 * Grid Common column definitions
 */
export namespace CommonColumnDefinitions {
  export const getSector = () => {
    const column = new CommonDialogColumnModel("sectorName", "Sector", 150);
    column.isRequired = true;
    column.selectShouldInitAsync = true;
    column.useAsModel = true;
    column.selectValue = 'id';
    column.selectTitle = 'value';
    column.selectRequestQuery = (new SectorGridData()).requestQuery;
    column.inputType = EnumModelType.select;
    column.modelPropertyName = 'sectorId'
    column.icon = 'mdi-note-edit-outline'
    return column;
  }

  export const getBaseYearColumn = () => {
    const column = new CommonDialogColumnModel("baseYearForStatistics", "통계 기준년도", 160);
    column.useAsModel = true;
    column.isRequired = true;
    column.inputType = EnumModelType.select;
    column.selectItems = getYearList(true);
    column.modelPropertyName = 'baseYearForStatistics'
    column.icon = 'mdi-calendar-check'
    return column;
  };

  export const getApprovalStatus =() => {
    const column = new CommonDialogColumnModel("approvalStatus", "ApprovalStatus", 160);
    column.useAsModel = true;
    column.isRequired = true;
    column.inputType = EnumModelType.select;
    column.selectItems = GetApprovalStatusList();
    column.selectValue = 'id';
    column.selectTitle = 'title';
    column.modelPropertyName = 'approvalStatus'
    column.icon = 'mdi-note-edit-outline'
    column.cellRenderer = (params) => {
      switch (params.value) {
        case EnumApprovalStatus.None:
          return "None";
        case EnumApprovalStatus.NotYetIssuePo:
          return "Not Yet Issue PO";
        case EnumApprovalStatus.IssuePo:
          return "Issue PO";
        case EnumApprovalStatus.SpendingAndIssuePo:
          return "Spending & Issue PO";
        default:
          return "Error"; // 값이 열거형에 없는 경우
      }
    };
    column.cellStyle = (params) => {
      switch (params.value) {
        case EnumApprovalStatus.None:
          return { backgroundColor: '#33CC3344', color: 'light-black' };
        case EnumApprovalStatus.NotYetIssuePo:
          return { backgroundColor: '#ccc42244', color: 'light-black' };
        case EnumApprovalStatus.IssuePo:
          return { backgroundColor: '#33CC3344', color: 'light-black' };
        case EnumApprovalStatus.SpendingAndIssuePo:
          return { backgroundColor: '#2244CC44', color: 'light-black' };
        default:
          return { backgroundColor: '#CC222244', color: 'light-black' }; // 값이 열거형에 없는 경우
      }
    }
    return column;
  };

  export const getApprovalDate =() => {
    const column = new CommonDialogColumnModel("approvalDate", "Approval Date", 145);
    column.useAsModel = true;
    column.isRequired = true;
    column.inputType = EnumModelType.text;
    column.selectValue = 'id';
    column.selectTitle = 'title';
    column.modelPropertyName = 'approvalDate'
    column.icon = 'mdi-calendar-check'
    return column;
  };

  export const getDescription = () => {
    const column = new CommonDialogColumnModel("description", "Description", 250);
    column.useAsModel = true;
    column.inputType = EnumModelType.text;
    column.modelPropertyName = 'description'
    column.icon = 'mdi-note-text-outline'
    return column;
  };

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

  export const getBusinessUnit = () => {
    const column = new CommonDialogColumnModel("businessUnitName", "Business Unit", 150);
    column.isRequired = true;
    column.selectShouldInitAsync = true;
    column.useAsModel = true;
    column.selectValue = 'id';
    column.selectTitle = 'name';
    column.selectRequestQuery = (new BusinessUnitGridData()).requestQuery;
    column.inputType = EnumModelType.select;
    column.modelPropertyName = 'businessUnitId'
    column.icon = 'mdi-note-edit-outline'
    return column;
  };

  export const getCostCenter = () => {
    const column = new CommonDialogColumnModel("costCenterName", "Cost Center", 150);
    column.isRequired = true;
    column.selectShouldInitAsync = true;
    column.useAsModel = true;
    column.selectValue = 'id';
    column.selectTitle = 'value';
    column.selectRequestQuery = (new CostCenterGridData()).requestQuery;
    column.inputType = EnumModelType.select;
    column.modelPropertyName = 'costCenterId'
    column.icon = 'mdi-note-edit-outline'
    return column;
  };

  export const getCountryBusinessManager = () => {
    const column = new CommonDialogColumnModel("countryBusinessManagerName", "Country Business Manager", 230);
    column.isRequired = true;
    column.selectShouldInitAsync = true;
    column.useAsModel = true;
    column.selectValue = 'id';
    column.selectTitle = 'name';
    column.selectRequestQuery = (new CountryBusinessManagerGridData()).requestQuery;
    column.inputType = EnumModelType.select;
    column.modelPropertyName = 'countryBusinessManagerId'
    column.icon = 'mdi-note-edit-outline'
    return column;
  };

  export const getActual = () => {
    const column = new CommonDialogColumnModel("actual", "Actual", 130);
    column.useAsModel = true;
    column.inputType = EnumModelType.number;
    column.valueFormatter = (new CommonGridModel()).numberValueFormatter;
    column.modelPropertyName = 'actual' ;
    column.icon = 'mdi-currency-usd'
    return column;
  };

  export const getApprovalAmount = () => {
    const column = new CommonDialogColumnModel("approvalAmount", "ApprovalAmount", 150);
    column.useAsModel = true;
    column.inputType = EnumModelType.number;
    column.valueFormatter = new CommonGridModel().numberValueFormatter;
    column.modelPropertyName = 'approvalAmount'
    column.icon = 'mdi-currency-usd'
    return column;
  };

  export const getBudgetTotal = () => {
    const column = new CommonDialogColumnModel("budgetTotal", "BudgetTotal", 150);
    column.useAsModel = true;
    column.inputType = EnumModelType.number;
    column.valueFormatter = new CommonGridModel().numberValueFormatter;
    column.modelPropertyName = 'budgetTotal'
    column.icon = 'mdi-currency-usd'
    return column;
  };

  export const getPoNumber = () => {
    const column = new CommonDialogColumnModel("poNumber", "PoNumber", 130);
    column.useAsModel = true;
    column.inputType = EnumModelType.number;
    column.modelPropertyName = 'poNumber'
    column.icon = 'mdi-numeric'
    return column;
  };

  export const getBossLineDescription = () => {
    const column = new CommonDialogColumnModel("bossLineDescription", "BossLineDescription", 190);
    column.useAsModel = true;
    column.inputType = EnumModelType.number;
    column.modelPropertyName = 'bossLineDescription'
    column.icon = 'mdi-note-text-outline'
    return column;
  };

  export const getOcProjectName = () => {
    const column = new CommonDialogColumnModel("ocProjectName", "Oc Project Name", 190);
    column.useAsModel = true;
    column.inputType = EnumModelType.number;
    column.modelPropertyName = 'ocProjectName'
    column.icon = 'mdi-note-text-outline'
    return column;
  };

  export const getSequence = () => {
    const column = new CommonDialogColumnModel("sequence", "순서", 100);
    column.useAsModel = true;
    column.inputType = EnumModelType.number;
    column.modelPropertyName = 'sequence'
    column.icon = 'mdi-numeric'
    return column;
  };

  export const getName = () => {
    const column = new CommonDialogColumnModel("name", "이름", 250);
    column.useAsModel = true;
    column.inputType = EnumModelType.text;
    column.modelPropertyName = 'name'
    column.icon = 'mdi-note-text-outline'
    return column;
  };


  export const getSectorName = () => {
    const column = new CommonDialogColumnModel("value", "이름", 250);
    column.useAsModel = true;
    column.inputType = EnumModelType.text;
    column.modelPropertyName = 'value'
    column.icon = 'mdi-note-text-outline'
    return column;
  };

  export const getRegNameColumn = () => CommonColumnDefinitions.createColumnDefinitionForTextFilter(250, 'regName' , '등록자명');

  export const getNameColumn = () => ({
    field: 'name',
    headerName: '이름',
    headerClass: 'ag-grids-custom-header',
    filter: 'agTextColumnFilter',
    floatingFilter: true,
    useAsModel: true,
    inputType : 'text',
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





  // export const getBusinessUnit = () => CommonColumnDefinitions.createColumnDefinitionForTextFilter(150, "businessUnitName", "Business Unit");


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

  export const createColumnDefinitionForTextFilter = (width: number , field: string , headerName: string, valueFormatter?: {} , isFloatingFilter: boolean = true, isUseInChart: boolean = true, isRequired: boolean = false) => {
    return {
      field: field,
      headerClass: 'ag-grids-custom-header',
      filter: 'agTextColumnFilter' ,
      headerName: headerName  ,
      valueFormatter: valueFormatter,
      width: width,
      floatingFilter: isFloatingFilter,
      isUseInChart: isUseInChart ,
      isRequired: isRequired ,
    }
  }
}

