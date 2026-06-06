import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Store } from '@ngrx/store';
import { filter } from 'rxjs';
import { RiskBankEntry } from '../../core/models/risk.model';
import { selectSelectedClientId } from '../../core/store/clients/clients.selectors';
import { loadRiskBank, toggleBankRisk } from '../../core/store/risks/risks.actions';
import { selectRiskBank } from '../../core/store/risks/risks.selectors';

@Component({
  selector: 'app-risk-bank',
  standalone: true,
  imports: [AsyncPipe, NgFor, NgIf, FormsModule, MatCardModule, MatChipsModule, MatButtonModule, MatIconModule, MatInputModule, MatSelectModule],
  template: `
    <div class="page-header">
      <h2>Risk Bank</h2>
      <p class="subtitle">KPMG-curated library of tax risks available to add to client RACMs</p>
    </div>

    <div class="filters">
      <mat-select [(ngModel)]="filterTaxType" (selectionChange)="applyFilter()" placeholder="Tax Type">
        <mat-option value="">All Tax Types</mat-option>
        <mat-option *ngFor="let t of taxTypes" [value]="t">{{ t }}</mat-option>
      </mat-select>
      <mat-select [(ngModel)]="filterCountry" (selectionChange)="applyFilter()" placeholder="Country">
        <mat-option value="">All Countries</mat-option>
        <mat-option *ngFor="let c of countries" [value]="c">{{ c }}</mat-option>
      </mat-select>
    </div>

    <div class="bank-grid" *ngIf="riskBank$ | async as bank">
      <mat-card *ngFor="let entry of bank" class="bank-card">
        <mat-card-header>
          <mat-card-title>{{ entry.name }}</mat-card-title>
          <mat-card-subtitle>{{ entry.taxType }}</mat-card-subtitle>
        </mat-card-header>
        <mat-card-content>
          <p class="description">{{ entry.description }}</p>
          <div class="score-row">
            <span class="label">Default Gross Score:</span>
            <strong>{{ entry.defaultGrossLikelihood * entry.defaultGrossImpact }}/25</strong>
            (L{{ entry.defaultGrossLikelihood }} × I{{ entry.defaultGrossImpact }})
          </div>
          <div class="tags">
            <mat-chip *ngFor="let tag of entry.tags">{{ tag }}</mat-chip>
          </div>
          <div class="countries">
            <mat-chip *ngFor="let c of entry.applicableCountries" class="country-chip">{{ c }}</mat-chip>
          </div>
        </mat-card-content>
        <mat-card-actions>
          <button mat-raised-button color="primary" (click)="addToRacm(entry)" [disabled]="!(clientId$ | async)">
            <mat-icon>add</mat-icon> Add to RACM
          </button>
        </mat-card-actions>
      </mat-card>
    </div>
  `,
  styles: [`
    .page-header { margin-bottom: 16px; }
    .subtitle { color: #666; font-size: 0.9rem; margin-top: -8px; }
    .filters { display: flex; gap: 16px; margin-bottom: 24px; }
    .filters mat-select { width: 200px; }
    .bank-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(360px, 1fr)); gap: 16px; }
    .bank-card { display: flex; flex-direction: column; }
    .description { font-size: 0.85rem; color: #555; margin: 8px 0; }
    .score-row { margin: 8px 0; font-size: 0.85rem; }
    .label { color: #888; margin-right: 4px; }
    .tags, .countries { display: flex; flex-wrap: wrap; gap: 6px; margin: 8px 0; }
    .country-chip { background: #e3f2fd !important; }
  `]
})
export class RiskBankComponent implements OnInit {
  private readonly store = inject(Store);

  riskBank$ = this.store.select(selectRiskBank);
  clientId$ = this.store.select(selectSelectedClientId);

  taxTypes = ['CorporateIncomeTax', 'IndirectTax', 'TransferPricing', 'EmploymentTax', 'Customs', 'Payroll'];
  countries = ['GB', 'DE', 'FR', 'NL', 'US', 'SG', 'AU', 'JP', 'IE', 'LU'];
  filterTaxType = '';
  filterCountry = '';

  ngOnInit(): void {
    this.store.dispatch(loadRiskBank({}));
  }

  applyFilter(): void {
    this.store.dispatch(loadRiskBank({
      taxType: this.filterTaxType || undefined,
      country: this.filterCountry || undefined
    }));
  }

  addToRacm(entry: RiskBankEntry): void {
    this.clientId$.pipe(filter(id => !!id)).subscribe(clientId => {
      const requestedById = '00000000-0000-0000-0000-000000000001';
      this.store.dispatch(toggleBankRisk({ clientId: clientId!, riskBankEntryId: entry.id, requestedById }));
    }).unsubscribe();
  }
}
