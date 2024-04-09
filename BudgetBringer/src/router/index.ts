import {createRouter, createWebHistory, NavigationGuardNext, RouteLocationNormalized, RouteRecordRaw} from 'vue-router'
import home from "../pages/home.vue";
import login from "../pages/login/login.vue";
import commonCode from "../pages/common-code/common-code.vue";
import BudgetPlan from "../pages/budget/budget-plan/budget-plan.vue";
import NoPage from "../pages/no-page.vue";
import BudgetApproved from "../pages/budget/budget-approved/budget-approved.vue";
import BudgetProcess from "../pages/budget/budget-process/budget-process.vue";
import LogAction from "../pages/logs/action/log-action.vue";
import {AuthenticationStore} from "../services/stores/authentication-store";
import {messageService} from "../services/message-service";
import {authenticationService} from "../services/api-services/authentication-service";
import {firstValueFrom} from "rxjs";


/**
 * 라우트 정보
 */
const routes: Array<RouteRecordRaw> = [
  // 로그인 페이지
  {
    path: '',
    name: 'login',
    component: login,
  },
  // 로그인 페이지
  {
    path: '/login',
    name: 'login',
    component: login,
  },
  // 없는 페이지 인경우
  {
    path: '/:pathMatch(.*)*',
    name: 'page-not-found',
    component: NoPage,
  },
  // 메인 홈
  {
    path: '',
    name: 'home',
    component: home,
    meta: { requiresAuth: true },
    children: [
      {
        // 코드관리/공통코드 페이지
        path: '/common-code',
        name: 'common-code',
        component: commonCode,
        meta: { requiresAuth: true , permissions: ['common-code'] },
      },
      // 예산 관련 컴포넌트
      {
        // 코드관리/공통코드 페이지
        path: '/budget',
        name: 'Budget',
        redirect : '/budget/plan' ,
        meta: { requiresAuth: true  },
        children: [
          // 예산 계획
          {
            path: 'plan',
            name: 'BudgetPlan',
            component: BudgetPlan,
            meta: { requiresAuth: true , permissions: ['budget-plan'] },
          },
          // 예산 승인
          {
            path: 'approved',
            name: 'BudgetApproved',
            component: BudgetApproved,
            meta: { requiresAuth: true , permissions: ['budget-approved'] },
          },
          // 예산 계획
          {
            path: 'process',
            name: 'BudgetProcess',
            component: BudgetProcess,
            meta: { requiresAuth: true , permissions: ['process-result-view' , 'process-result'] },
          },
        ]
      },
      // 로그 관련 컴포넌트
      {
        path: '/logs',
        name: 'Logs',
        redirect : '' ,
        meta: { requiresAuth: true },
        children: [
          // 액션로그
          {
            path: 'action',
            name: 'ActionLog',
            component: LogAction,
            meta: { requiresAuth: true , permissions: ['log-action'] },
          },
        ]
      },
    ]
  },
];

/**
 * 라우터를 주입한다.
 */
const router = createRouter({
  history: createWebHistory(),
  routes
});

/**
 * 라우터 클래스 재정의
 */
export default router;

/**
 * 라우팅 인터셉터
 */
router.beforeEach(async (to: RouteLocationNormalized, from: RouteLocationNormalized, next: NavigationGuardNext) => {
  /**
   * 인증 상태 관리
   */
  const authenticationStore = AuthenticationStore();
  const requiresAuth = to.matched.some(record => record.meta.requiresAuth);

  // 인증이 필요한 페이지이고
  if (requiresAuth) {
    const response = await firstValueFrom(authenticationService.isAuthenticatedAsync());

    // 인증여부 확인
    if (!response.isAuthenticated) {
      authenticationStore.clearAuthenticated();
      // messageService.showError("인증정보가 없거나 만료되었습니다.");
      next("/Login");
    }

    console.log('to.meta.permissions',to.meta.permissions);

    // Permission 이 필요한경우
    if (to.meta.permissions) {
      // 요구하는 Claim 정보를 가져온다.
      const requiredPermissions = to.meta.permissions as string [];

      console.log('requiredPermissions',requiredPermissions);

      // 권한을 가지고있는지 확인한다.
      const hasPermission = requiredPermissions.some(permission => authenticationStore.hasPermission([permission]));

      // 권한이 없는경우
      if(!hasPermission) {
        messageService.showWarning("접근 권한이 없습니다.");
        next('/Budget/ProcessResult');
        return;
      }
    }

    next();
  } else {
    next();
  }
});
