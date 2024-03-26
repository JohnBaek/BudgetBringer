import { createRouter, createWebHistory } from 'vue-router'
import {NavigationGuardNext, RouteLocationNormalized, RouteRecordRaw} from "vue-router";
import home from "../pages/home.vue";
import login from "../pages/login/login.vue";
import commonCode from "../pages/common-code/common-code.vue";
import BudgetPlan from "../pages/budget/budget-plan/budget-plan.vue";
import NoPage from "../pages/no-page.vue";
import BudgetApproved from "../pages/budget/budget-approved/budget-approved.vue";
import BudgetProcess from "../pages/budget/budget-process/budget-process.vue";


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
    // meta: {requiresAuth: true},
    children: [
      {
        // 코드관리/공통코드 페이지
        path: '/common-code',
        name: 'common-code',
        component: commonCode,
      },
      // 예산 관련 컴포넌트
      {
        // 코드관리/공통코드 페이지
        path: '/budget',
        name: 'Budget',
        redirect : '/budget/plan' ,
        children: [
          // 예산 계획
          {
            path: 'plan',
            name: 'BudgetPlan',
            component: BudgetPlan,
          },
          // 예산 승인
          {
            path: 'approved',
            name: 'BudgetApproved',
            component: BudgetApproved,
          },
          // 예산 계획
          {
            path: 'process',
            name: 'BudgetProcess',
            component: BudgetProcess,
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
  history: createWebHistory(process.env.BASE_URL),
  routes
});

/**
 * 라우터 클래스 재정의
 */
export default router;

/**
 * 라우팅 인터셉터
 */
router.beforeEach((to: RouteLocationNormalized, from: RouteLocationNormalized, next: NavigationGuardNext) => {
  next();
});
