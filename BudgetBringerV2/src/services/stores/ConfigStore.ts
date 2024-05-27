import { defineStore } from 'pinia'
import type { ResponseBusinessUnit } from '@/models/responses/budgets/response-business-unit'
import type { ResponseCostCenter } from '@/models/responses/budgets/response-cost-center'
import type { ResponseSector } from '@/models/responses/budgets/response-sector'
import type { ResponseCountryBusinessManager } from '@/models/responses/budgets/response-country-business-manager'

/**
 * State of configs ( BusinessUnits .. etc )
 */
interface ConfigStore {
  businessUnits: Array<ResponseBusinessUnit> ,
  costCenters: Array<ResponseCostCenter> ,
  sectors: Array<ResponseSector> ,
  countryBusinessManager: Array<ResponseCountryBusinessManager> ,
}
/**
 * State of request/response
 */
export const useConfigStore = defineStore('configStore', {
  state: (): ConfigStore => <ConfigStore> ({
    businessUnits:[],
    costCenters:[],
    sectors:[],
    countryBusinessManager:[],
  }),
  actions: {
    /**
     * Retrieve all
     */
    retrieve() {
      // Clear Data
      this.businessUnits = [];
      this.costCenters = [];
      this.sectors = [];
      this.countryBusinessManager = [];


    },
  }
});