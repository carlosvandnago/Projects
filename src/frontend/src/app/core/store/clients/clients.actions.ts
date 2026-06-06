import { createAction, props } from '@ngrx/store';
import { Client } from '../../models/client.model';

export const loadClients = createAction('[Clients] Load Clients');
export const loadClientsSuccess = createAction('[Clients] Load Clients Success', props<{ clients: Client[] }>());
export const loadClientsFailure = createAction('[Clients] Load Clients Failure', props<{ error: string }>());
export const selectClient = createAction('[Clients] Select Client', props<{ clientId: string }>());
