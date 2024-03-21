import {ResponseData} from "../models/Responses/ResponseData";
import {ResponseUser} from "../models/Responses/Users/ResponseUser";
import {EnumResponseResult} from "../models/Enums/EnumResponseResult";
import {AuthenticationStore} from "../store/AuthenticationStore";

export const loginService = {
  async tryLoginAsync(loginId: string, password: string): Promise<ResponseData<ResponseUser>> {
    try {
      // 여기에 실제 로그인 API 호출 코드를 구현합니다.
      // 예시로는 axios를 사용한 가짜 응답을 생성합니다.
      // const response = await axios.post('/api/login', { username, password });

      // 로그인 성공 가정
      let response: ResponseData<ResponseUser> = {
        result: EnumResponseResult.error,
        message: '아이디와 패스워드를 확인해주세요',
        data: { name: '' }, // 응답 구조에 맞게 조정 필요
      };

      if(loginId === 'admin' && password === '1234') {
        response = {
          result: EnumResponseResult.success,
          message: '',
          data: { name: '관리자' }, // 응답 구조에 맞게 조정 필요
        };
      }

      const authenticationStore = AuthenticationStore();

      if(response.result === EnumResponseResult.error) {
        authenticationStore.clearAuthenticated();
      }
      // 성공인경우
      else {
        authenticationStore.updateAuthenticated(response.data);
      }

      return response;
    } catch (error) {
      console.error('Login service error:', error);
      return {
        result: EnumResponseResult.error,
        data: null,
        message : ''
      };
    }
  },
};
