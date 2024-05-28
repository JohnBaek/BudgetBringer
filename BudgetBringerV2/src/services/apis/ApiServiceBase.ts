import { useMessageStore } from '@/services/stores/MessageStore'
import { useCommunicationStore } from '@/services/stores/CommunicationStore'
import { ApiClient } from '@/services/apis/ApiClient'

export abstract class ApiServiceBase {
  // Message Store
  public readonly messageStore: ReturnType<typeof useMessageStore>  = useMessageStore();

  // Communication Store
  public readonly communicationStore: ReturnType<typeof useCommunicationStore>   = useCommunicationStore();

  // Should implement in descendents API Request client
  abstract client: ApiClient;
}