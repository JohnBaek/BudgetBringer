import { EnumResponseResult } from "../enums/enum-response-result";

/**
 * Response Interface
 */
export interface IResponse {
  result: EnumResponseResult;
  message: string;
  code: string;
  isAuthenticated: boolean;
  error: boolean;
  success: boolean;
  warning: boolean;
}

/**
 * Implement IResponse
 */
export class Response implements IResponse {
  // State of Response
  public result: EnumResponseResult = EnumResponseResult.error;

  // Message of Response
  public message: string = '';

  // Code of Response
  public code: string = '';

  // State of Authenticated of Response of user
  public isAuthenticated: boolean = false;

  // State of Response
  public error: boolean = false;

  // State of Response
  public success: boolean = false;

  // State of Response
  public warning: boolean = false;



  //
  // /**
  //  * Is State of Error?
  //  */
  // public error(): boolean {
  //   return this.result === EnumResponseResult.error;
  // }
  //
  // /**
  //  * Is State of Success?
  //  */
  // public success(): boolean {
  //   return this.result === EnumResponseResult.success;
  // }
  //
  // /**
  //  * Is State of Warning?
  //  */
  // public warning(): boolean {
  //   return this.result === EnumResponseResult.waring;
  // }
}
