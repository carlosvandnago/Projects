import { createEntityAdapter, EntityAdapter, EntityState } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { BowTieData, RacmEntry, RiskBankEntry } from '../../models/risk.model';
import * as RiskActions from './risks.actions';

export interface RisksState extends EntityState<RacmEntry> {
  selectedRiskId: string | null;
  bowTie: BowTieData | null;
  riskBank: RiskBankEntry[];
  loading: boolean;
  error: string | null;
}

const adapter: EntityAdapter<RacmEntry> = createEntityAdapter<RacmEntry>();

const initialState: RisksState = adapter.getInitialState({
  selectedRiskId: null,
  bowTie: null,
  riskBank: [],
  loading: false,
  error: null
});

export const risksReducer = createReducer(
  initialState,
  on(RiskActions.loadRacm, state => ({ ...state, loading: true, error: null })),
  on(RiskActions.loadRacmSuccess, (state, { entries }) => adapter.setAll(entries, { ...state, loading: false })),
  on(RiskActions.loadRacmFailure, (state, { error }) => ({ ...state, loading: false, error })),
  on(RiskActions.selectRisk, (state, { riskId }) => ({ ...state, selectedRiskId: riskId })),
  on(RiskActions.loadBowTieSuccess, (state, { data }) => ({ ...state, bowTie: data })),
  on(RiskActions.loadRiskBankSuccess, (state, { entries }) => ({ ...state, riskBank: entries }))
);

export const { selectAll, selectEntities } = adapter.getSelectors();
