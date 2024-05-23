// Statistics by Owners
export interface ResponseProcessOwner {
  // Id of Country Business Manager
  countryBusinessManagerId: string;

  // Name of Country Business Manager
  countryBusinessManagerName: string;

  // This year Budget
  budgetYear: number;

  // This year Approved
  approvedYear: number;

  // [budgetYear] - [approvedYear]
  remainingYear: number;
}
