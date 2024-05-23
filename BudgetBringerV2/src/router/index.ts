import { createRouter, createWebHistory, type NavigationGuardNext, type RouteLocationNormalized } from 'vue-router'
import Login from '@/components/login/Login.vue'
import { useAuthenticationStore } from '@/services/state-managements/AuthenticationStore'
import { firstValueFrom } from 'rxjs'
import { AuthenticationAPIService } from '@/services/RestAPIServices/AuthenticationAPIService'
import { useMessageStore } from '@/services/state-managements/MessageStore'
import Home from '@/components/Home.vue'
import BudgetStatistics from '@/components/budget-management/BudgetStatistics.vue'

/**
 * 라우터를 주입한다.
 */
const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/login', name: 'login', component: Login, },
    { path: '/', redirect: '/login' } ,
    {
      path: '', name: 'home', component: Home,
      meta: { requiresAuth: true },
      children: [
        {
          path: '/budget/management', name: 'Budget', meta: { requiresAuth: true },
          children: [
            {
              path: 'statistics', name: 'BudgetStatistics', component: BudgetStatistics,
              meta: { requiresAuth: true, permissions: ['process-result-view', 'process-result'] },
            }
          ]
        }
      ]
    }
  ]
});

/**
 * 라우터 클래스 재정의
 */
export default router;

/**
 * 라우팅 인터셉터
 */
router.beforeEach(async (to: RouteLocationNormalized, from: RouteLocationNormalized, next: NavigationGuardNext) => {
  const requiresAuth = to.matched.some(record => record.meta.requiresAuth);
  const restApi: AuthenticationAPIService = new AuthenticationAPIService();
  const authenticationStore = useAuthenticationStore();
  const messageStore = useMessageStore();

  // Need Authentication
  if (requiresAuth) {
    const response = await firstValueFrom(restApi.isAuthenticatedAsync());

    // 인증여부 확인
    if (!response.isAuthenticated) {
      restApi.logoutAsync();
      next("/Login");
    }

    // Permission 이 필요한경우
    if (to.meta.permissions) {
      // 요구하는 Claim 정보를 가져온다.
      const requiredPermissions = to.meta.permissions as string [];

      // 권한을 가지고있는지 확인한다.
      const hasPermission = requiredPermissions.some(permission => authenticationStore.hasPermission([permission]));

      // 권한이 없는경우
      if(!hasPermission) {
        messageStore.showWarning("권한" ,"접근 권한이 없습니다.");
        next('/login');
        return;
      }
    }
    next();
  } else {
    next();
  }
});

