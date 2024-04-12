import {defineStore} from "pinia";
import {DrawerLink} from "../pages/models-view/drawer-link";
import {toRaw} from "vue";

interface RoutingState {
  /**
   * 현재 라우팅 정보
   */
  currentRoute: DrawerLink | null;

  /**
   * 라우팅정보
   */
  drawerRouting : DrawerLink[];
}

export const RoutingStore = defineStore('routingStore', {
  state : () : RoutingState  => {
      return  {
        currentRoute : null ,
        drawerRouting: [
          new DrawerLink('예산계획', '예산계획을 세우고 작성합니다.', '/budget/plan', 'mdi-notebook', ['budget-plan'],false,[]),
          new DrawerLink('예산승인', '계획된 예산을 승인합니다.', '/budget/approved', 'mdi-check', ['budget-approved'],false,[]),
          new DrawerLink('예산진행현황', '예산 사용 진행 현황에 대해서 확인합니다.', '/budget/process', 'mdi-currency-usd', ['process-result','process-result-view'],false,[]),
          new DrawerLink('액션로그', '사용자 작업 로그에대해서 확인합니다.', '/logs/action', 'mdi-notebook-minus-outline', ['log-action','log-action-view'],false,[]),
          new DrawerLink('공통코드관리', '', '', 'mdi-code-tags', ['common-code'] , true,[
            new DrawerLink('CostCenter', 'CostCenter 를 관리합니다.', '/common-code/cost-center', '', ['common-code'] , false,[]),
            new DrawerLink('BusinessUnit', 'BusinessUnit 를 관리합니다.', '/common-code/business-unit', '', ['common-code'] , false,[]),
            new DrawerLink('Sector', 'Sector 를 관리합니다.', '/common-code/sector', '', ['common-code'] , false,[]),
            new DrawerLink('CBM', 'CountryBusinessManager 를 관리합니다.', '/common-code/country-business-manager', '', ['common-code'] , false,[]),
          ]),
        ]
      }
  },
  actions: {
    /**
     * 입력받은 주소로 해당하는 라우팅이 있는경우 라우팅 주소를 업데이트한다.
     * @param routePath 라우팅 패스
     */
    tryUpdateRoute(routePath: string) {
      const target = routePath.toLowerCase();
      let finds;

      // 모든 draw 정보에서 찾는다.
      for (const drawLink of this.drawerRouting) {
        if(drawLink.route === target) {
          finds = [drawLink];
        }
        for (const child of drawLink.childMenus) {
          if(child.route === target) {
            finds = [child];
            break;
          }
        }
      }

      // 찾지 못한경우
      if(finds.length === 0)
        return false;

      this.currentRoute = finds[0];

      console.log(this.currentRoute);
      return true;
    },

    /**
     * 라우팅 목록을 반환한다.
     */
    getRoutingList() {
      return toRaw(this.drawerRouting);
    },

    /**
     * 현재 라우팅 정보를 가져온다.
     */
    getCurrentRoute() {
      return toRaw(this.currentRoute);
    }
  }
});
