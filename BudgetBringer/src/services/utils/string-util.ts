/**
 * Change word to Pascal Case
 * @param target
 */
export const toPascalCase = (target: string) => {
  return target
  .replace(new RegExp(/[-_]+/, 'g'), ' ') // 대시와 언더스코어를 공백으로 대체
  .replace(new RegExp(/[^\w\s]/, 'g'), '') // 문자와 공백이 아닌 모든 것 제거
  .replace(
    /\s+(.)(\w*)/g, // 각 단어를 대문자로 시작하도록 변경
    (_, firstChar, rest) => firstChar.toUpperCase() + rest.toLowerCase()
  )
  .replace(/\s/g, '') // 공백 제거
  .replace(
    /^./, // 첫 글자도 대문자로
    str => str.toUpperCase()
  );
}
