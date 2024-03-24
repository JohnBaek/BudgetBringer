/**
 * 로그인 요청 모델 클래스
 */
export class RequestLogin {
  /**
   * 로그인 아이디
   */
  public loginId : string = '';

  /**
   * 패스워드
   */
  public password : string = '';

  /**
   * 생성자
   * @param loginId 로그인 아이디
   * @param password 패스워드
   */
  constructor(loginId: string, password: string) {
    this.loginId = loginId;
    this.password = password;
  }
}
