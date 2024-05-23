export class logger {
  static originalConsoleLog = console.log;
  static originalConsoleWarn = console.warn;
  static originalConsoleError = console.error;

  static getCallerFunctionName(): string {
    const error = new Error();
    const stack = error.stack || '';
    const stackLines = stack.split('\n');
    const callerStackLine = stackLines[3] || ''; // 호출한 함수의 스택 라인 (index 3)
    const functionNameMatch = callerStackLine.match(/at (\w+)/);
    return functionNameMatch ? functionNameMatch[1] : 'unknown';
  }

  static log(...args: unknown[]): void {
    const timestamp = new Date().toISOString();
    const callerFunctionName = logger.getCallerFunctionName();
    logger.originalConsoleLog.call(console, `[${timestamp}] [LOG] [${callerFunctionName}]`, ...args);
  }

  static warn(...args: unknown[]): void {
    const timestamp = new Date().toISOString();
    const callerFunctionName = logger.getCallerFunctionName();
    logger.originalConsoleWarn.call(console, `[${timestamp}] [WARN] [${callerFunctionName}]`, ...args);
  }

  static error(...args: unknown[]): void {
    const timestamp = new Date().toISOString();
    const callerFunctionName = logger.getCallerFunctionName();
    logger.originalConsoleError.call(console, `[${timestamp}] [ERROR] [${callerFunctionName}]`, ...args);
  }
}
