import { createAction, props } from '@ngrx/store';
import { BowTieData, RacmEntry, RiskBankEntry } from '../../models/risk.model';

export const loadRacm = createAction('[Risks] Load RACM', props<{ clientId: string; taxType?: string; rating?: string }>());
export const loadRacmSuccess = createAction('[Risks] Load RACM Success', props<{ entries: RacmEntry[] }>());
export const loadRacmFailure = createAction('[Risks] Load RACM Failure', props<{ error: string }>());

export const selectRisk = createAction('[Risks] Select Risk', props<{ riskId: string }>());

export const loadBowTie = createAction('[Risks] Load BowTie', props<{ clientId: string; riskId: string }>());
export const loadBowTieSuccess = createAction('[Risks] Load BowTie Success', props<{ data: BowTieData }>());
export const loadBowTieFailure = createAction('[Risks] Load BowTie Failure', props<{ error: string }>());

export const loadRiskBank = createAction('[Risks] Load Risk Bank', props<{ taxType?: string; country?: string; industry?: string }>());
export const loadRiskBankSuccess = createAction('[Risks] Load Risk Bank Success', props<{ entries: RiskBankEntry[] }>());
export const loadRiskBankFailure = createAction('[Risks] Load Risk Bank Failure', props<{ error: string }>());

export const toggleBankRisk = createAction('[Risks] Toggle Bank Risk', props<{ clientId: string; riskBankEntryId: string; requestedById: string }>());
export const toggleBankRiskSuccess = createAction('[Risks] Toggle Bank Risk Success');
export const toggleBankRiskFailure = createAction('[Risks] Toggle Bank Risk Failure', props<{ error: string }>());
