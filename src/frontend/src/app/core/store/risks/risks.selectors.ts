import { createFeatureSelector, createSelector } from '@ngrx/store';
import { RisksState, selectAll, selectEntities } from './risks.reducer';

export const selectRisksState = createFeatureSelector<RisksState>('risks');

export const selectAllRacm = createSelector(selectRisksState, selectAll);
export const selectRacmEntities = createSelector(selectRisksState, selectEntities);
export const selectSelectedRiskId = createSelector(selectRisksState, s => s.selectedRiskId);
export const selectBowTie = createSelector(selectRisksState, s => s.bowTie);
export const selectRiskBank = createSelector(selectRisksState, s => s.riskBank);
export const selectRisksLoading = createSelector(selectRisksState, s => s.loading);

export const selectSelectedRisk = createSelector(
  selectRacmEntities,
  selectSelectedRiskId,
  (entities, id) => (id ? entities[id] : null)
);
