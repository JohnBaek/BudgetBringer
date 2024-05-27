import { useMessageStore } from '@/services/stores/MessageStore'
import { useCommunicationStore } from '@/services/stores/CommunicationStore'
import { ApiClient } from '@/services/apis/ApiClient'

export abstract class ApiServiceBase {
  public readonly messageStore = useMessageStore();
  public readonly communicationStore = useCommunicationStore();
  abstract client: ApiClient;
}