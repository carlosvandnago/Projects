import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { Store } from '@ngrx/store';
import { filter } from 'rxjs';
import { ApiService } from '../../core/services/api.service';
import { selectSelectedClientId } from '../../core/store/clients/clients.selectors';

@Component({
  selector: 'app-import',
  standalone: true,
  imports: [AsyncPipe, NgFor, NgIf, FormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatSelectModule, MatProgressSpinnerModule],
  template: `
    <div class="page-header">
      <h2>AI Document Import</h2>
      <p class="subtitle">Paste tax legislation, contracts, or board minutes to extract risks using Claude AI</p>
    </div>

    <mat-card>
      <mat-card-content>
        <mat-form-field class="full-width">
          <mat-label>Tax Type Focus</mat-label>
          <mat-select [(ngModel)]="taxType">
            <mat-option *ngFor="let t of taxTypes" [value]="t">{{ t }}</mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field class="full-width">
          <mat-label>Entity Context</mat-label>
          <input matInput [(ngModel)]="entityContext" placeholder="e.g. GlobalTech UK HoldCo — manufacturing group" />
        </mat-form-field>

        <mat-form-field class="full-width">
          <mat-label>Document Text</mat-label>
          <textarea matInput [(ngModel)]="documentText" rows="12" placeholder="Paste document text here..."></textarea>
        </mat-form-field>

        <div class="actions">
          <button mat-raised-button color="primary" [disabled]="loading || !documentText || !taxType || !(clientId$ | async)" (click)="analyse()">
            {{ loading ? 'Analysing...' : 'Analyse Document' }}
          </button>
          <mat-progress-spinner *ngIf="loading" mode="indeterminate" diameter="24"></mat-progress-spinner>
        </div>
      </mat-card-content>
    </mat-card>

    <mat-card *ngIf="result" class="result-card">
      <mat-card-header>
        <mat-card-title>{{ result.suggestedRisks?.length || 0 }} Risks Identified</mat-card-title>
        <mat-card-subtitle>{{ result.summary }}</mat-card-subtitle>
      </mat-card-header>
      <mat-card-content>
        <div *ngFor="let risk of result.suggestedRisks" class="risk-suggestion">
          <h4>{{ risk.name }}</h4>
          <p>{{ risk.description }}</p>
          <div class="risk-meta">
            <span class="badge">{{ risk.taxType }}</span>
            <span>Suggested score: {{ risk.suggestedLikelihood * risk.suggestedImpact }}/25</span>
          </div>
        </div>
      </mat-card-content>
    </mat-card>
  `,
  styles: [`
    .page-header { margin-bottom: 16px; }
    .subtitle { color: #666; margin-top: -8px; }
    .full-width { width: 100%; margin-bottom: 16px; }
    .actions { display: flex; align-items: center; gap: 16px; }
    .result-card { margin-top: 24px; }
    .risk-suggestion { padding: 16px; border-bottom: 1px solid #eee; }
    .risk-suggestion h4 { margin: 0 0 8px; }
    .risk-suggestion p { color: #555; font-size: 0.9rem; }
    .risk-meta { display: flex; gap: 16px; align-items: center; margin-top: 8px; }
    .badge { background: #1a237e; color: white; padding: 2px 10px; border-radius: 12px; font-size: 0.8rem; }
  `]
})
export class ImportComponent {
  private readonly store = inject(Store);
  private readonly api = inject(ApiService);

  clientId$ = this.store.select(selectSelectedClientId);

  taxType = 'TransferPricing';
  entityContext = '';
  documentText = '';
  loading = false;
  result: any = null;

  taxTypes = ['CorporateIncomeTax', 'IndirectTax', 'TransferPricing', 'EmploymentTax', 'Customs', 'Payroll'];

  analyse(): void {
    this.clientId$.pipe(filter(id => !!id)).subscribe(clientId => {
      this.loading = true;
      this.api.analyseDocument(clientId!, '00000000-0000-0000-0000-000000000001', this.taxType, this.entityContext, this.documentText).subscribe({
        next: result => { this.result = result; this.loading = false; },
        error: () => { this.loading = false; }
      });
    }).unsubscribe();
  }
}
