import { ClientsState } from './clients/clients.reducer';
import { RisksState } from './risks/risks.reducer';

export interface AppState {
  clients: ClientsState;
  risks: RisksState;
}
