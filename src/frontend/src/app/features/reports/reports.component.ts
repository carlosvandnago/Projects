import { AsyncPipe, DatePipe, NgFor, NgIf } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { Store } from '@ngrx/store';
import { filter, switchMap } from 'rxjs';
import { ApiService } from '../../core/services/api.service';
import { ReportDto, RacmSummaryDto } from '../../core/models/report.model';
import { selectSelectedClient } from '../../core/store/clients/clients.selectors';
import { selectAllRacm } from '../../core/store/risks/risks.selectors';
import { loadRacm } from '../../core/store/risks/risks.actions';
import { selectSelectedClientId } from '../../core/store/clients/clients.selectors';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [AsyncPipe, DatePipe, NgFor, NgIf, FormsModule, MatCardModule, MatButtonModule, MatSelectModule, MatProgressSpinnerModule],
  template: `
    <div class="page-header">
      <h2>AI-Powered Reports</h2>
      <p class="subtitle">Generate tailored reports using Claude AI for different audiences</p>
    </div>

    <div class="report-types">
      <mat-card *ngFor="let rt of reportTypes" class="report-type-card" [class.selected]="selectedReportType === rt.type" (click)="selectedReportType = rt.type">
        <mat-card-header>
          <mat-card-title>{{ rt.label }}</mat-card-title>
          <mat-card-subtitle>{{ rt.audience }}</mat-card-subtitle>
        </mat-card-header>
        <mat-card-content>
          <p>{{ rt.description }}</p>
        </mat-card-content>
      </mat-card>
    </div>

    <div class="generate-section">
      <button mat-raised-button color="primary" [disabled]="generating || !selectedReportType || !(clientId$ | async)" (click)="generate()">
        {{ generating ? 'Generating...' : 'Generate Report' }}
      </button>
      <mat-progress-spinner *ngIf="generating" mode="indeterminate" diameter="24"></mat-progress-spinner>
    </div>

    <mat-card *ngIf="report" class="report-output">
      <mat-card-header>
        <mat-card-title>{{ report.reportType }} Report</mat-card-title>
        <mat-card-subtitle>Generated {{ report.generatedAt | date:'medium' }} · {{ report.tokensUsed }} tokens</mat-card-subtitle>
      </mat-card-header>
      <mat-card-content>
        <pre class="report-content">{{ report.markdownContent }}</pre>
      </mat-card-content>
    </mat-card>
  `,
  styles: [`
    .page-header { margin-bottom: 16px; }
    .subtitle { color: #666; margin-top: -8px; }
    .report-types { display: grid; grid-template-columns: repeat(auto-fill, minmax(260px, 1fr)); gap: 16px; margin-bottom: 24px; }
    .report-type-card { cursor: pointer; border: 2px solid transparent; transition: border-color 0.2s; }
    .report-type-card:hover { border-color: #1565c0; }
    .report-type-card.selected { border-color: #1a237e; background: #e8eaf6; }
    .generate-section { display: flex; align-items: center; gap: 16px; margin-bottom: 24px; }
    .report-output { }
    .report-content { white-space: pre-wrap; font-family: monospace; font-size: 0.85rem; line-height: 1.6; max-height: 60vh; overflow-y: auto; }
  `]
})
export class ReportsComponent implements OnInit {
  private readonly store = inject(Store);
  private readonly api = inject(ApiService);

  clientId$ = this.store.select(selectSelectedClientId);
  client$ = this.store.select(selectSelectedClient);
  racm$ = this.store.select(selectAllRacm);

  selectedReportType = '';
  generating = false;
  report: ReportDto | null = null;

  reportTypes = [
    { type: 'LocalEntity', label: 'Local Entity Report', audience: 'Local Finance Director', description: 'Detailed risk and control report for individual entities.' },
    { type: 'GlobalHeadOfTax', label: 'Global Head of Tax', audience: 'Group Tax Director', description: 'Executive summary across all entities and risk categories.' },
    { type: 'BoardAuditCommittee', label: 'Board Audit Committee', audience: 'Non-Executive Directors', description: 'High-level risk profile and control assurance narrative.' },
    { type: 'KpmgAdvisory', label: 'KPMG Advisory Report', audience: 'Advisory Partners', description: 'Structured advisory findings and recommendations.' },
  ];

  ngOnInit(): void {
    this.clientId$.pipe(filter(id => !!id)).subscribe(id => {
      this.store.dispatch(loadRacm({ clientId: id! }));
    });
  }

  generate(): void {
    this.clientId$.pipe(filter(id => !!id)).subscribe(clientId => {
      this.client$.pipe(filter(c => !!c)).subscribe(client => {
        this.racm$.subscribe(racm => {
          const summaries: RacmSummaryDto[] = racm.map(r => ({
            name: r.name,
            taxType: r.taxType,
            grossScore: r.grossScore?.score || 0,
            netScore: r.netScore?.score || 0,
            netRating: r.netRiskRating,
            totalControls: 0,
            evidencedControls: 0,
            outstandingActions: 0,
            notes: r.notes
          }));

          this.generating = true;
          this.api.generateReport(clientId!, '00000000-0000-0000-0000-000000000001', client!.name, this.selectedReportType, null, summaries).subscribe({
            next: report => { this.report = report; this.generating = false; },
            error: () => { this.generating = false; }
          });
        }).unsubscribe();
      }).unsubscribe();
    }).unsubscribe();
  }
}
