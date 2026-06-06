import { AsyncPipe, NgFor, NgIf, NgClass } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { Store } from '@ngrx/store';
import { filter } from 'rxjs';
import { selectSelectedClientId } from '../../core/store/clients/clients.selectors';
import { loadRacm, selectRisk } from '../../core/store/risks/risks.actions';
import { selectAllRacm, selectRisksLoading, selectSelectedRisk } from '../../core/store/risks/risks.selectors';
import { RiskDetailPanelComponent } from './risk-detail-panel.component';

@Component({
  selector: 'app-racm',
  standalone: true,
  imports: [AsyncPipe, NgFor, NgIf, NgClass, FormsModule, MatTableModule, MatCardModule, MatChipsModule, MatButtonModule, MatIconModule, MatSelectModule, MatProgressSpinnerModule, RiskDetailPanelComponent],
  template: `
    <div class="racm-header">
      <h2>RACM — Risk Assessment & Controls Matrix</h2>
      <div class="filters">
        <mat-select [(ngModel)]="filterTaxType" (selectionChange)="applyFilter()" placeholder="Tax Type">
          <mat-option value="">All</mat-option>
          <mat-option *ngFor="let t of taxTypes" [value]="t">{{ t }}</mat-option>
        </mat-select>
        <mat-select [(ngModel)]="filterRating" (selectionChange)="applyFilter()" placeholder="Rating">
          <mat-option value="">All Ratings</mat-option>
          <mat-option *ngFor="let r of ratings" [value]="r">{{ r }}</mat-option>
        </mat-select>
      </div>
    </div>

    <ng-container *ngIf="!(loading$ | async); else spinner">
      <div class="racm-layout">
        <mat-card class="table-card">
          <table mat-table [dataSource]="(racm$ | async) ?? []" class="racm-table">
            <ng-container matColumnDef="name">
              <th mat-header-cell *matHeaderCellDef>Risk</th>
              <td mat-cell *matCellDef="let r">
                <div class="risk-name">{{ r.name }}</div>
                <div class="risk-tax-type">{{ r.taxType }}</div>
              </td>
            </ng-container>

            <ng-container matColumnDef="gross">
              <th mat-header-cell *matHeaderCellDef>Gross Score</th>
              <td mat-cell *matCellDef="let r">
                <span class="score-badge" [class]="'rating-' + r.grossRiskRating?.toLowerCase()">
                  {{ r.grossScore?.score }} ({{ r.grossRiskRating }})
                </span>
              </td>
            </ng-container>

            <ng-container matColumnDef="net">
              <th mat-header-cell *matHeaderCellDef>Net Score</th>
              <td mat-cell *matCellDef="let r">
                <span class="score-badge" [class]="'rating-' + r.netRiskRating?.toLowerCase()">
                  {{ r.netScore?.score }} ({{ r.netRiskRating }})
                </span>
              </td>
            </ng-container>

            <ng-container matColumnDef="delta">
              <th mat-header-cell *matHeaderCellDef>CE Delta</th>
              <td mat-cell *matCellDef="let r">{{ r.controlEffectivenessDelta }}</td>
            </ng-container>

            <ng-container matColumnDef="status">
              <th mat-header-cell *matHeaderCellDef>Status</th>
              <td mat-cell *matCellDef="let r">{{ r.status }}</td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef></th>
              <td mat-cell *matCellDef="let r">
                <button mat-icon-button (click)="selectRisk(r.id)">
                  <mat-icon>chevron_right</mat-icon>
                </button>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns; sticky: true"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;"
                [class.selected-row]="row.id === (selectedRisk$ | async)?.id"
                (click)="selectRisk(row.id)"></tr>
          </table>
        </mat-card>

        <app-risk-detail-panel *ngIf="selectedRisk$ | async" class="detail-panel"></app-risk-detail-panel>
      </div>
    </ng-container>

    <div *ngIf="!(clientId$ | async)" class="no-client">
      <mat-card><mat-card-content>Please select a client to view RACM.</mat-card-content></mat-card>
    </div>

    <ng-template #spinner><mat-progress-spinner mode="indeterminate"></mat-progress-spinner></ng-template>
  `,
  styles: [`
    .racm-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px; }
    .filters { display: flex; gap: 16px; }
    .filters mat-select { width: 160px; }
    .racm-layout { display: grid; grid-template-columns: 1fr; gap: 16px; }
    .table-card { overflow: auto; max-height: calc(100vh - 200px); }
    .racm-table { width: 100%; }
    .risk-name { font-weight: 500; }
    .risk-tax-type { font-size: 0.75rem; color: #888; }
    .score-badge { padding: 4px 10px; border-radius: 12px; font-size: 0.8rem; font-weight: 600; color: white; }
    .rating-critical { background: #b71c1c; }
    .rating-high { background: #e65100; }
    .rating-medium { background: #f57f17; }
    .rating-low { background: #388e3c; }
    .rating-minimal { background: #90a4ae; }
    .selected-row { background: #e8eaf6 !important; }
    tr:hover { background: #f5f5f5; cursor: pointer; }
    .detail-panel { height: calc(100vh - 200px); overflow-y: auto; }
    .no-client { margin-top: 40px; }
  `]
})
export class RacmComponent implements OnInit {
  private readonly store = inject(Store);

  clientId$ = this.store.select(selectSelectedClientId);
  racm$ = this.store.select(selectAllRacm);
  loading$ = this.store.select(selectRisksLoading);
  selectedRisk$ = this.store.select(selectSelectedRisk);

  displayedColumns = ['name', 'gross', 'net', 'delta', 'status', 'actions'];
  taxTypes = ['CorporateIncomeTax', 'IndirectTax', 'TransferPricing', 'EmploymentTax', 'Customs', 'Payroll'];
  ratings = ['Critical', 'High', 'Medium', 'Low', 'Minimal'];
  filterTaxType = '';
  filterRating = '';

  ngOnInit(): void {
    this.clientId$.pipe(filter(id => !!id)).subscribe(id => {
      this.store.dispatch(loadRacm({ clientId: id! }));
    });
  }

  applyFilter(): void {
    this.clientId$.pipe(filter(id => !!id)).subscribe(id => {
      this.store.dispatch(loadRacm({
        clientId: id!,
        taxType: this.filterTaxType || undefined,
        rating: this.filterRating || undefined
      }));
    }).unsubscribe();
  }

  selectRisk(riskId: string): void {
    this.store.dispatch(selectRisk({ riskId }));
  }
}
