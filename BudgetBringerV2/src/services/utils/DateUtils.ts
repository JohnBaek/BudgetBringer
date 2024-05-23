/**
 * Format Current date for file
 */
export const getDateFormatForFile = (): string => {
  const date = new Date();
  const year = date.getFullYear();
  const month = (date.getMonth() + 1).toString().padStart(2, '0');
  const day = date.getDate().toString().padStart(2, '0');
  const hours = date.getHours().toString().padStart(2, '0');
  const minutes = date.getMinutes().toString().padStart(2, '0');
  const seconds = date.getSeconds().toString().padStart(2, '0');

  return `${year}${month}${day}_${hours}${minutes}${seconds}_`;
}

/**
 * Get Year List
 */
export const getYearList = (toDesc: boolean) => {
  // Get Current year of date
  const currentYear = new Date().getFullYear();

  // Set Start Year
  const startYear = 2000;

  // Set End year + 3
  const endYear = currentYear + 3;

  let years = [];
  for (let year = startYear; year <= endYear; year++) {
    years.push(year);
  }

  // To Desc List
  if(toDesc)
    years = years.reverse();

  return years;
}
