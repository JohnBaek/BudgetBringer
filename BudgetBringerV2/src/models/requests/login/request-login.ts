/**
 * Request Login Model
 */
export class RequestLogin {
  // Id
  loginId : string = '';

  // Password
  password : string = '';

  /**
   * Validator
   */
  isInvalid() {
    return this.loginId === '' || this.password === '';
  }
}
