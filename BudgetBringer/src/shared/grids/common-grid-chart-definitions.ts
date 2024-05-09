/**
 * Grid Common chart definitions
 */
export namespace CommonChartDefinitions {
  export const createChartLegend = (type : string , xKey: string , yKey: string, yName: string  ) => {
    return {
        type: type
      , xKey: xKey
      , yKey: yKey
      , yName: yName
      , tooltip: {
        renderer: function ({ datum, xKey, yKey }) {
          return {
            content: Intl.NumberFormat('en-US', { style: 'decimal', maximumFractionDigits: 0 }).format(datum[yKey]) ,
            title: datum[xKey]
          };
        },
      },
    };
  }

  export const createColumnDefinitionForTextFilter = (width: number , field: string , headerName: string, valueFormatter?: {} , isFloatingFilter: boolean = true  ) => {
    return {
      field: field,
      headerClass: 'ag-grids-custom-header',
      filter: 'agTextColumnFilter' ,
      headerName: headerName  ,
      valueFormatter: valueFormatter,
      width: width,
      floatingFilter: isFloatingFilter,
    }
  }
}

