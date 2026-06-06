export interface RiskScore {
  likelihood: number;
  impact: number;
  score: number;
  likelihoodLabel: string;
  impactLabel: string;
}

export interface RiskRating {
  label: string;
  colour: string;
}

export interface LinkedEntity {
  entityId: string;
  localOwnerId?: string;
}

export interface RacmEntry {
  id: string;
  clientId: string;
  riskBankEntryId?: string;
  name: string;
  description: string;
  taxType: string;
  scope: string;
  globalOwnerId: string;
  grossScore: RiskScore;
  netScore: RiskScore;
  causes: string[];
  consequences: string[];
  status: string;
  notes: string;
  lastReviewed: string;
  linkedEntities: LinkedEntity[];
  grossRiskRating: string;
  netRiskRating: string;
  controlEffectivenessDelta: number;
}

export interface RiskBankEntry {
  id: string;
  name: string;
  taxType: string;
  description: string;
  causes: string[];
  consequences: string[];
  suggestedPreventiveControls: string[];
  suggestedMitigatingControls: string[];
  applicableCountries: string[];
  industries: string[];
  tags: string[];
  defaultGrossLikelihood: number;
  defaultGrossImpact: number;
  isActive: boolean;
}

export interface BowTieControl {
  id: string;
  name: string;
  type: string;
  effectiveness: string;
}

export interface BowTieData {
  riskId: string;
  riskName: string;
  causes: string[];
  consequences: string[];
  grossScore: number;
  netScore: number;
  preventiveControls: BowTieControl[];
  mitigatingControls: BowTieControl[];
  detectiveControls: BowTieControl[];
}
