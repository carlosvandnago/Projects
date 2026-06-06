import { createEntityAdapter, EntityAdapter, EntityState } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { Client } from '../../models/client.model';
import * as ClientActions from './clients.actions';

export interface ClientsState extends EntityState<Client> {
  selectedClientId: string | null;
  loading: boolean;
  error: string | null;
}

const adapter: EntityAdapter<Client> = createEntityAdapter<Client>();

const initialState: ClientsState = adapter.getInitialState({
  selectedClientId: null,
  loading: false,
  error: null
});

export const clientsReducer = createReducer(
  initialState,
  on(ClientActions.loadClients, state => ({ ...state, loading: true, error: null })),
  on(ClientActions.loadClientsSuccess, (state, { clients }) =>
    adapter.setAll(clients, { ...state, loading: false })),
  on(ClientActions.loadClientsFailure, (state, { error }) => ({ ...state, loading: false, error })),
  on(ClientActions.selectClient, (state, { clientId }) => ({ ...state, selectedClientId: clientId }))
);

export const { selectAll, selectEntities, selectIds } = adapter.getSelectors();
