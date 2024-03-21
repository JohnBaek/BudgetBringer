import CommonCodeComponent from "../components/CommonCode/CommonCodeComponent.vue";
import LoginComponent from '../components/LoginComponent.vue';
import {createRouter, createWebHistory, NavigationGuardNext, RouteLocationNormalized, RouteRecordRaw} from 'vue-router';
import HomeComponent from "../components/HomeComponent.vue";
import NoPageComponent from "../components/NoPageComponent.vue";
import BudgetProcessComponent from "../components/Budget/BudgetProcessComponent.vue";
import BudgetPlanComponent from "../components/Budget/BudgetPlanComponent.vue";
import BudgetApprovedComponent from "../components/Budget/BudgetApprovedComponent.vue";
import {AuthenticationStore} from "../store/AuthenticationStore";


/**
 * 라우트 정보
 */
const routes: Array<RouteRecordRaw> = [
  // 로그인 페이지
  {
    path: '/login',
    name: 'Login',
    component: LoginComponent,
  },
  // 없는 페이지 인경우
  {
    path: '/:pathMatch(.*)*',
    name: 'page-not-found',
    component: NoPageComponent,
  },
  // 메인 홈
  {
    path: '/',
    name: 'Home',
    component: HomeComponent,
    meta: {requiresAuth: true},
    children: [
      {
        // 코드관리/공통코드 페이지
        path: '/common-code',
        name: 'CommonCode',
        component: CommonCodeComponent,
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
            component: BudgetPlanComponent,
          },
          // 예산 승인
          {
            path: 'approved',
            name: 'BudgetApproved',
            component: BudgetApprovedComponent,
          },
          // 예산 계획
          {
            path: 'process',
            name: 'BudgetProcess',
            component: BudgetProcessComponent,
          },
        ]
      },
    ]
  },
];

/**
 * 라우트생성
 */
const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
});

export default router;



/**
 * 라우터 실행 핸들러
 * 로그인여부를 체크한다.
 */
router.beforeEach((to: RouteLocationNormalized, from: RouteLocationNormalized, next: NavigationGuardNext) => {
  const authenticationStore = AuthenticationStore();

  // 인증정보가 필요할 경우
  if (to.matched.some(record => record.meta.requiresAuth)) {
    // 로그인 되어있지 않다면
    if (!authenticationStore.isAuthenticated) {
      // 로그인 페이지로 이동한다.
      next({name: 'Login'});
      // 로그인 되어있다면
    } else {
      next();
    }
    // 인증정보가 필요하지 않은경우
  } else {
    next();
  }
});
