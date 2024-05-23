export const toClone = (target: any) => {
  return JSON.parse(JSON.stringify(target));
}
