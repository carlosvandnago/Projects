import { inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, map, of, switchMap } from 'rxjs';
import { ApiService } from '../../services/api.service';
import * as ClientActions from './clients.actions';

@Injectable()
export class ClientsEffects {
  private readonly actions$ = inject(Actions);
  private readonly api = inject(ApiService);

  loadClients$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ClientActions.loadClients),
      switchMap(() =>
        this.api.getClients().pipe(
          map(clients => ClientActions.loadClientsSuccess({ clients })),
          catchError(error => of(ClientActions.loadClientsFailure({ error: error.message })))
        )
      )
    )
  );
}
