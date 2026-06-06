import { inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, map, of, switchMap } from 'rxjs';
import { ApiService } from '../../services/api.service';
import * as RiskActions from './risks.actions';

@Injectable()
export class RisksEffects {
  private readonly actions$ = inject(Actions);
  private readonly api = inject(ApiService);

  loadRacm$ = createEffect(() =>
    this.actions$.pipe(
      ofType(RiskActions.loadRacm),
      switchMap(({ clientId, taxType, rating }) =>
        this.api.getRacm(clientId, taxType, rating).pipe(
          map(entries => RiskActions.loadRacmSuccess({ entries })),
          catchError(error => of(RiskActions.loadRacmFailure({ error: error.message })))
        )
      )
    )
  );

  loadBowTie$ = createEffect(() =>
    this.actions$.pipe(
      ofType(RiskActions.loadBowTie),
      switchMap(({ clientId, riskId }) =>
        this.api.getBowTie(clientId, riskId).pipe(
          map(data => RiskActions.loadBowTieSuccess({ data })),
          catchError(error => of(RiskActions.loadBowTieFailure({ error: error.message })))
        )
      )
    )
  );

  loadRiskBank$ = createEffect(() =>
    this.actions$.pipe(
      ofType(RiskActions.loadRiskBank),
      switchMap(({ taxType, country, industry }) =>
        this.api.getRiskBank(taxType, country, industry).pipe(
          map(entries => RiskActions.loadRiskBankSuccess({ entries })),
          catchError(error => of(RiskActions.loadRiskBankFailure({ error: error.message })))
        )
      )
    )
  );

  toggleBankRisk$ = createEffect(() =>
    this.actions$.pipe(
      ofType(RiskActions.toggleBankRisk),
      switchMap(({ clientId, riskBankEntryId, requestedById }) =>
        this.api.toggleBankRisk(clientId, riskBankEntryId, requestedById).pipe(
          map(() => RiskActions.toggleBankRiskSuccess()),
          catchError(error => of(RiskActions.toggleBankRiskFailure({ error: error.message })))
        )
      )
    )
  );
}
