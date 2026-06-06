import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ClientsState, selectAll, selectEntities } from './clients.reducer';

export const selectClientsState = createFeatureSelector<ClientsState>('clients');

export const selectAllClients = createSelector(selectClientsState, selectAll);
export const selectClientEntities = createSelector(selectClientsState, selectEntities);
export const selectSelectedClientId = createSelector(selectClientsState, s => s.selectedClientId);
export const selectClientsLoading = createSelector(selectClientsState, s => s.loading);

export const selectSelectedClient = createSelector(
  selectClientEntities,
  selectSelectedClientId,
  (entities, id) => (id ? entities[id] : null)
);
