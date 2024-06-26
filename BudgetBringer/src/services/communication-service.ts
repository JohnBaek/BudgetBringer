import {Subject} from "rxjs";

/**
 * 통신중 여부 구독
 */
export class CommunicationService {
  /**
   * 커뮤니케이션
   */
  public communication : boolean = false;
  /**
   * 커뮤니케이션 여부
   * @private
   */
  public communicationSubject = new Subject<boolean>();
  /**
   * transmissions
   */
  public transmission : boolean = false;
  public transmissionSubject = new Subject<boolean>();

  /**
   * 통신중 상태변경
   */
  notifyInCommunication() {
    this.communication = true;
    this.communicationSubject.next(true);
  }

  /**
   * 통신중 상태변경
   */
  notifyOffCommunication() {
    this.communication = false;
    this.communicationSubject.next(false);
  }

  /**
   * 통신중 상태변경
   */
  inTransmission() {
    this.transmission = true;
    this.transmissionSubject.next(true);
  }

  /**
   * 통신중 상태변경
   */
  offTransmission() {
    this.transmission = false;
    this.transmissionSubject.next(false);
  }

  /**
   * 커뮤니테이션 상태 구독
   */
  subscribeCommunication() {
    return this.communicationSubject.asObservable();
  }
  subscribeTransmission() {
    return this.transmissionSubject.asObservable();
  }
}

// 싱글톤 인스턴스 생성 및 내보내기
export const communicationService = new CommunicationService();

