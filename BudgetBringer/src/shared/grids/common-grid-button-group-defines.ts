/**
 * Defines dispatches to parent.
 */
export type CommonGridButtonGroupDefinesButtonEmits = {
  (e: 'onNewRowAdded', params: any): void,
  (e: 'onAdd'): void,
  (e: 'onRemove', params: any): void,
  (e: 'onUpdate', params: any): void,
  (e: 'onRefresh'): void,
  (e: 'onCellClicked', params: any): void,
  (e: 'onExportExcel'): void,
  (e: 'exportPdf'): void,
  (e: 'chart'): void,
  (e: 'grid'): void,
  (e: 'print'): void,
};
