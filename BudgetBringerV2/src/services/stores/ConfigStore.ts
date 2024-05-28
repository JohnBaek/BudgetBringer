import { defineStore } from 'pinia'
import type { ResponseBusinessUnit } from '@/models/responses/budgets/response-business-unit'
import type { ResponseCostCenter } from '@/models/responses/budgets/response-cost-center'
import type { ResponseSector } from '@/models/responses/budgets/response-sector'
import type { ResponseCountryBusinessManager } from '@/models/responses/budgets/response-country-business-manager'
import { firstValueFrom, forkJoin, retry } from 'rxjs'
import { BusinessUnitApiService } from '@/services/apis/BusinessUnitApiService'
import { CostCenterApiService } from '@/services/apis/CostCenterApiService'
import { CountryBusinessManagerApiService } from '@/services/apis/CountryBusinessManagerApiService'
import { SectorApiService } from '@/services/apis/SectorApiService'
import { useCommunicationStore } from '@/services/stores/CommunicationStore'

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
     * @param useCommunication is Use Communication
     * @returns {Promise<{businessUnits: Array<ResponseBusinessUnit>, costCenters: Array<ResponseCostCenter>, sectors: Array<ResponseSector>, countryBusinessManager: Array<ResponseCountryBusinessManager>}>}
     */
    async retrieve(useCommunication: boolean = false) {
      if (useCommunication)
        useCommunicationStore().inCommunication();

      // Clear Data
      this.businessUnits = [];
      this.costCenters = [];
      this.sectors = [];
      this.countryBusinessManager = [];

      try {
        // List of APIs
        const businessUnitsRequest = new BusinessUnitApiService().getListAsync().pipe(retry(2));
        const costCenterRequest = new CostCenterApiService().getListAsync().pipe(retry(2));
        const countryBusinessManagerRequest = new CountryBusinessManagerApiService().getListAsync().pipe(retry(2));
        const sectorRequest = new SectorApiService().getListAsync().pipe(retry(2));

        // Request fork
        console.log("Request to Configs");

        const [
            businessUnitResponse
          , costCenterResponse
          , countryBusinessManagerResponse
          , sectorResponse] =
          await firstValueFrom(
            forkJoin([
              businessUnitsRequest,
              costCenterRequest,
              countryBusinessManagerRequest,
              sectorRequest
            ]
          )
        );

        // Update List
        this.businessUnits = businessUnitResponse.items;
        this.costCenters = costCenterResponse.items;
        this.countryBusinessManager = countryBusinessManagerResponse.items;
        this.sectors = sectorResponse.items;

        console.log(businessUnitResponse, costCenterResponse, countryBusinessManagerResponse, sectorResponse);

        // Return the results
        return {
          businessUnits: this.businessUnits,
          costCenters: this.costCenters,
          sectors: this.sectors,
          countryBusinessManager: this.countryBusinessManager
        };
      } catch (error) {
        console.error(error);
        if (useCommunication)
          useCommunicationStore().offCommunication();
        throw error; // Rethrow the error to handle it in the calling code
      } finally {
        console.log("retrieve completed!");
        if (useCommunication)
          useCommunicationStore().offCommunication();
      }
    },
  }
});