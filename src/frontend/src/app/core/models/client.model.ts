export interface TaxEntity {
  id: string;
  clientId: string;
  name: string;
  country: string;
  jurisdiction: string;
  entityType: string;
  region: string;
}

export interface Client {
  id: string;
  name: string;
  industry: string;
  fiscalYearEndMonth: number;
  onboardedDate: string;
  isActive: boolean;
  entities: TaxEntity[];
}
