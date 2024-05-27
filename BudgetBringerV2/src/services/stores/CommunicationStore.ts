import { defineStore } from 'pinia'

/**
 * State of request/response
 */
interface CommunicationStore {
  // For request server and should place loading state
  communication : boolean;

  // For only represent on communications
  transmission : boolean;
}
/**
 * State of request/response
 */
export const useCommunicationStore = defineStore('communicationState', {
  state: (): CommunicationStore => <CommunicationStore> ({
    communication: false ,
    transmission: false,
  }),
  actions: {
    /**
     * Set communication on
     */
    inCommunication() {
      this.communication = true;
    },

    /**
     * Set communication off
     */
    offCommunication() {
      this.communication = false;
    },

    /**
     * Set transmission on
     */
    inTransmission() {
      this.transmission = true;
    },

    /**
     * Set transmission off
     */
    offTransmission() {
      this.transmission = false;
    }
  }
});