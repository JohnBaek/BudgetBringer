import type { ColumnFilterMatchModeOptions } from 'primevue/column'

/**
 * Represent Common grid Column
 */
export class CommonGridColumn {
  constructor(init?: Partial<CommonGridColumn>) {
    this.isUseFilter = init?.isUseFilter ?? false;
    this.filterOptions = init?.filterOptions ?? null;
    this.filterComponentType = init?.filterComponentType ?? null;
    this.filterItems = init?.filterItems ?? null;
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

  // When isUseFilter true and filterComponentType is 'Select' works. filter Items
  filterItems: [] | null = null;

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
export type CommonGridFilterComponentType = 'Text' | 'Select';

/**
 * Text Filter Options
 */
export const CommonGridTextFilters: Array<ColumnFilterMatchModeOptions> = [
  { label: '포함하는( "ab" = "abc" )', value: 'Contains' },
  { label: '같은( "ab" = "ab" )', value: 'Equals' },
];
/**
 * Number Filter Options
 */
export const CommonGridNumberFilters: Array<ColumnFilterMatchModeOptions> = [
  { label: '큰( "2" = "1" )', value: 'GreaterThan' },
  { label: '작은( "1" = "2" )', value: 'LessThan' },
  { label: '같은( "2" = "2" )', value: 'Equals' },
]
