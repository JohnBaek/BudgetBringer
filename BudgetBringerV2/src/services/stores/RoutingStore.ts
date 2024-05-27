import { defineStore } from 'pinia'
import type { MenuItem } from 'primevue/menuitem'

/**
 * Routing Interface
 */
export interface RoutingInformation extends MenuItem{
  key:string;
  label: string;
  subTitle: string | null;
  route: string | null;
  permissions: Array<string>;
  icon: string;
  items: Array<RoutingInformation>;
}

/**
 * Routing Interface
 */
interface RoutingStore {
  // Routes
  routingList: Array<RoutingInformation>;

  // Current Route
  currentRoute: RoutingInformation | null;
}

/**
 * Routing Store
 */
export const useRoutingStore = defineStore('routingStore', {
  // 인증상태 정의
  state: () : RoutingStore => {
    return {
      currentRoute: null,
      routingList: [
        {
          key: 'user-management', label: '사용자관리' , subTitle:'사용자를 관리합니다.', route:'/users/management', permissions:['budget-plan'] , icon: 'pi pi-users' ,  items: [] ,
        } ,
        {
          key: 'budget-plan', label: '예산계획' , subTitle:'예산계획을 세우고 작성합니다', route:'/budget/plan', permissions:['budget-plan'] , icon: 'pi pi-calendar' ,  items: [] ,
        } ,
        { key: 'budget-statistics' ,label: '예산진행현황' , subTitle:'예산 사용 진행 현황에 대해서 확인합니다.', route:'/budget/statistics', permissions:['process-result','process-result-view'] , icon: 'pi pi-sparkles' ,  items: [],
        } ,
        {
          key: 'common-code' , label: '공통코드관리' , subTitle:null, icon: 'pi pi-link' , route: null , permissions:['budget-plan'] ,
          breadItems : [
            { label:'예산진행현황' }
          ],
          items: [
            {
              key: 'common-code-cost-center' , label: '코스트센터' , subTitle:'사용자를 관리합니다.' , route:'/common-code/cost-center', permissions:['budget-plan'] , icon: '' , items: [] ,
              breadItems : [
                { label:'예산진행현황' }
              ]
            } ,
          ]
        },
        {
          key: 'logout', label: '나가기' , subTitle:'', route:'/logout', permissions:['process-result','process-result-view'] , icon: 'pi pi-sign-out' ,  items: [] ,
          breadItems : []
        } ,
      ]
    }
  },
  actions: {
    getRouting (key: string) {
      function findRoute(items: Array<RoutingInformation>) {
        for (const item of items) {
          if (item.key === key) {
            return item.route;
          }
          if (item.items && item.items.length > 0) {
            const found = findRoute(item.items) as RoutingInformation;
            if (found) {
              return found;
            }
          }
        }
        return null;
      }
      return findRoute(this.routingList);
    },
    getRoutingByPath(path: string) {
      function findRoute(items: Array<RoutingInformation>) {
        for (const item of items) {
          if (item.route?.toLowerCase() === path.toLowerCase()) {
            return item;
          }
          if (item.items && item.items.length > 0) {
            const found = findRoute(item.items) as RoutingInformation;
            if (found) {
              return found;
            }
          }
        }
        return null;
      }
      return findRoute(this.routingList);
    },
    updateRoutingByPath(path: string) {
      const routing = this.getRoutingByPath(path);
      if(routing)
        this.currentRoute = routing as RoutingInformation;
    }
  }
});
