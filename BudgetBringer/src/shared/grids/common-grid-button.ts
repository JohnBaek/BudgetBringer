// Type of Button Action
export type ButtonActionType = 'ADD' | 'REMOVE' | 'UPDATE' | 'REFRESH' | 'EXPORT_EXCEL' | 'EXPORT_PDF' | 'TO_CHART' | 'TO_LIST';

/**
 * Grid Button Action
 */
export class CommonGridButton {
  // Action Type of Button
  public actionType: ButtonActionType;

  // Name of Button
  public name: string = '';

  // To dispatch To parent or not when clicked
  public toDispatch: boolean;

  // Icon of Button
  public icon: string = '';
}

/**
 * Pre Defined Buttons
 */
export namespace CommonButtonDefinitions {
  export const add: CommonGridButton = ({
    actionType:'ADD',
    name:'추가' ,
    toDispatch: false ,
    icon: 'mdi-checkbox-marked-circle'
  });

  export const remove: CommonGridButton = ({
    actionType:'REMOVE',
    name:'삭제' ,
    toDispatch: false ,
    icon: 'mdi-delete-circle'
  });

  export const update: CommonGridButton = ({
    actionType:'UPDATE',
    name:'수정' ,
    toDispatch: false ,
    icon: 'mdi-checkbox-multiple-marked-circle'
  });

  export const refresh: CommonGridButton = ({
    actionType:'REFRESH',
    name:'' ,
    toDispatch: false ,
    icon: 'mdi-refresh-circle'
  });

  export const exportExcel: CommonGridButton = ({
    actionType:'EXPORT_EXCEL',
    name:'' ,
    toDispatch: false ,
    icon: 'mdi-file-excel-outline'
  });

  export const exportPDF: CommonGridButton = ({
    actionType:'EXPORT_PDF',
    name:'' ,
    toDispatch: false ,
    icon: 'mdi-file-pdf-box'
  });

  export const toChart: CommonGridButton = ({
    actionType:'TO_CHART',
    name:'' ,
    toDispatch: false ,
    icon: 'mdi-chart-bar'
  });
  export const toList: CommonGridButton = ({
    actionType:'TO_LIST',
    name:'' ,
    toDispatch: false ,
    icon: 'mdi-view-list'
  });
}



