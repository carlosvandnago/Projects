import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
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
import { loadRacm } from '../../core/store/risks/risks.actions';
import { selectAllRacm } from '../../core/store/risks/risks.selectors';
import { RacmEntry } from '../../core/models/risk.model';

@Component({
  selector: 'app-control-advisor',
  standalone: true,
  imports: [AsyncPipe, NgFor, NgIf, FormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatSelectModule, MatProgressSpinnerModule],
  template: `
    <div class="page-header">
      <h2>AI Control Advisor</h2>
      <p class="subtitle">Use Claude AI to assess the effectiveness of proposed controls</p>
    </div>

    <mat-card class="advisor-card">
      <mat-card-content>
        <mat-form-field class="full-width">
          <mat-label>Select Risk</mat-label>
          <mat-select [(ngModel)]="selectedRisk">
            <mat-option *ngFor="let r of racm$ | async" [value]="r">{{ r.name }}</mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field class="full-width">
          <mat-label>Control to Assess</mat-label>
          <textarea matInput [(ngModel)]="controlDescription" rows="4"
            placeholder="Describe the control you want assessed..."></textarea>
        </mat-form-field>

        <div class="actions">
          <button mat-raised-button color="primary" [disabled]="loading || !selectedRisk || !controlDescription" (click)="assess()">
            {{ loading ? 'Assessing...' : 'Assess Control' }}
          </button>
          <mat-progress-spinner *ngIf="loading" mode="indeterminate" diameter="24"></mat-progress-spinner>
        </div>
      </mat-card-content>
    </mat-card>

    <mat-card *ngIf="assessment" class="assessment-result">
      <mat-card-header><mat-card-title>Assessment Result</mat-card-title></mat-card-header>
      <mat-card-content>
        <div class="effectiveness">
          <strong>Effectiveness:</strong> {{ assessment.effectivenessRating }}
          <div class="effectiveness-score" [style.width.%]="assessment.confidenceScore * 10"
               [style.background]="getEffectivenessColour(assessment.effectivenessRating)"></div>
        </div>
        <p class="rationale">{{ assessment.rationale }}</p>
        <div *ngIf="assessment.gaps?.length" class="gaps">
          <strong>Gaps Identified:</strong>
          <ul><li *ngFor="let g of assessment.gaps">{{ g }}</li></ul>
        </div>
        <div *ngIf="assessment.recommendations?.length" class="recommendations">
          <strong>Recommendations:</strong>
          <ul><li *ngFor="let r of assessment.recommendations">{{ r }}</li></ul>
        </div>
      </mat-card-content>
    </mat-card>
  `,
  styles: [`
    .page-header { margin-bottom: 16px; }
    .subtitle { color: #666; margin-top: -8px; }
    .advisor-card { margin-bottom: 24px; }
    .full-width { width: 100%; margin-bottom: 16px; }
    .actions { display: flex; align-items: center; gap: 16px; }
    .assessment-result { }
    .effectiveness { margin-bottom: 16px; }
    .effectiveness-score { height: 8px; border-radius: 4px; margin-top: 8px; transition: width 0.5s; }
    .rationale { color: #555; font-size: 0.9rem; line-height: 1.6; }
    .gaps, .recommendations { margin-top: 16px; }
    ul { padding-left: 20px; }
    li { font-size: 0.85rem; margin: 4px 0; }
  `]
})
export class ControlAdvisorComponent implements OnInit {
  private readonly store = inject(Store);
  private readonly api = inject(ApiService);

  clientId$ = this.store.select(selectSelectedClientId);
  racm$ = this.store.select(selectAllRacm);

  selectedRisk: RacmEntry | null = null;
  controlDescription = '';
  loading = false;
  assessment: any = null;

  ngOnInit(): void {
    this.clientId$.pipe(filter(id => !!id)).subscribe(id => {
      this.store.dispatch(loadRacm({ clientId: id! }));
    });
  }

  assess(): void {
    if (!this.selectedRisk) return;
    this.clientId$.pipe(filter(id => !!id)).subscribe(clientId => {
      this.loading = true;
      this.api.assessControl(
        clientId!, '00000000-0000-0000-0000-000000000001',
        this.selectedRisk!.name,
        this.selectedRisk!.grossScore?.score || 0,
        this.selectedRisk!.netScore?.score || 0,
        [],
        this.controlDescription
      ).subscribe({
        next: result => { this.assessment = result; this.loading = false; },
        error: () => { this.loading = false; }
      });
    }).unsubscribe();
  }

  getEffectivenessColour(rating: string): string {
    const map: Record<string, string> = { Effective: '#388e3c', PartiallyEffective: '#f57f17', Inadequate: '#b71c1c' };
    return map[rating] || '#90a4ae';
  }
}
