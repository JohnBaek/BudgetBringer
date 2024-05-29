import type { ColumnFilterMatchModeOptions } from 'primevue/column'
import { FilterMatchMode } from 'primevue/api'

/**
 * Represent Common grid Column
 */
export class CommonGridColumn {
  constructor(init?: Partial<CommonGridColumn>) {
    this.isUseFilter = init?.isUseFilter ?? false;
    this.filterOptions = init?.filterOptions ?? null;
    this.filterComponentType = init?.filterComponentType ?? null;
    if(init?.filterItems)
      this.filterItems = init?.filterItems;
    this.field = init?.field ?? '';
    this.header = init?.header ?? '';
    this.useAsModel = init?.useAsModel ?? true;
  }

  // Whether Using Filter
  isUseFilter: boolean = false;

  // When isUseFilter true works. Filtering options
  filterOptions: Array<CommonGridFilterOptionCore> | null = null;

  // When isUseFilter true works. Filtering options
  filterComponentType: CommonGridFilterComponentType | null = null;

  // Filter Match Mode
  filterMatchMode = FilterMatchMode.CONTAINS;

  // When isUseFilter true and filterComponentType is 'Select' works. filter Items
  filterItems: Array<any> = [];

  // Field of Columns. Property name
  field: string = '';

  // Header Name
  header: string = '';

  // Is Used as Model?
  useAsModel: boolean = true;
}

/**
 * Filter Option Property
 */
export interface CommonGridFilterOptionCore {
  label: string;
  value: CommonGridFilterOptionType;
}

/**
 * Options Types
 */
export type CommonGridFilterOptionType = 'Contains' | 'Equals' | 'GreaterThan' | 'LessThan';

/**
 * Filter Component Types
 */
export type CommonGridFilterComponentType = 'Text' | 'Select' | 'TagSelect';

/**
 * Text Filter Options
 */
export const CommonGridTextFilters: Array<ColumnFilterMatchModeOptions> = [
  { label: '포함하는( "ab" = "abc" )', value: FilterMatchMode.CONTAINS },
  { label: '같은( "ab" = "ab" )', value: FilterMatchMode.EQUALS},
];
/**
 * Number Filter Options
 */
export const CommonGridNumberFilters: Array<ColumnFilterMatchModeOptions> = [
  { label: '큰( "2" = "1" )', value: FilterMatchMode.GREATER_THAN },
  { label: '작은( "1" = "2" )', value: FilterMatchMode.LESS_THAN },
  { label: '같은( "2" = "2" )', value: FilterMatchMode.EQUALS },
]

interface Filter {
  value: any;
  matchMode: string;
}

interface Filters {
  [key: string]: Filter;
}

/**
 * Parse to columns to filters
 * @param columns Array<CommonGridColumn>
 */
export const ParseFilters = (columns: Array<CommonGridColumn>) => {
  const filters: Filters = {};
  columns.forEach(def => {
    filters[def.field] = { value: null, matchMode: def.filterMatchMode };
  });
  return filters;
}
