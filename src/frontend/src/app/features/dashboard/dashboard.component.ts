import { AsyncPipe, KeyValuePipe, NgFor, NgIf } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterLink } from '@angular/router';
import { Store } from '@ngrx/store';
import { BehaviorSubject, combineLatest, switchMap, filter } from 'rxjs';
import { ApiService } from '../../core/services/api.service';
import { selectSelectedClientId } from '../../core/store/clients/clients.selectors';
import { loadRacm } from '../../core/store/risks/risks.actions';
import { selectAllRacm, selectRisksLoading } from '../../core/store/risks/risks.selectors';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [AsyncPipe, NgFor, NgIf, KeyValuePipe, MatCardModule, MatChipsModule, MatProgressSpinnerModule, RouterLink],
  template: `
    <h2>Dashboard</h2>

    <ng-container *ngIf="!(loading$ | async); else spinner">
      <ng-container *ngIf="dashboardData$ | async as data">
        <div class="stats-grid">
          <mat-card class="stat-card">
            <mat-card-content>
              <div class="stat-value">{{ (racm$ | async)?.length || 0 }}</div>
              <div class="stat-label">Total Risks</div>
            </mat-card-content>
          </mat-card>

          <mat-card class="stat-card critical" *ngIf="data.ratingBreakdown?.['Critical'] as count">
            <mat-card-content>
              <div class="stat-value">{{ count }}</div>
              <div class="stat-label">Critical Risks</div>
            </mat-card-content>
          </mat-card>

          <mat-card class="stat-card high" *ngIf="data.ratingBreakdown?.['High'] as count">
            <mat-card-content>
              <div class="stat-value">{{ count }}</div>
              <div class="stat-label">High Risks</div>
            </mat-card-content>
          </mat-card>

          <mat-card class="stat-card warn">
            <mat-card-content>
              <div class="stat-value">{{ data.overdueControls || 0 }}</div>
              <div class="stat-label">Overdue Controls</div>
            </mat-card-content>
          </mat-card>

          <mat-card class="stat-card">
            <mat-card-content>
              <div class="stat-value">{{ data.upcomingDeadlines || 0 }}</div>
              <div class="stat-label">Upcoming (30 days)</div>
            </mat-card-content>
          </mat-card>
        </div>

        <div class="breakdown-section">
          <mat-card>
            <mat-card-header><mat-card-title>Risk Rating Breakdown</mat-card-title></mat-card-header>
            <mat-card-content>
              <div class="rating-bars" *ngIf="data.ratingBreakdown">
                <div *ngFor="let r of data.ratingBreakdown | keyvalue" class="rating-row">
                  <span class="rating-label">{{ r.key }}</span>
                  <div class="rating-bar" [style.width.%]="(asNum(r.value) / ((racm$ | async)?.length || 1)) * 100"
                       [class]="'bar-' + asStr(r.key).toLowerCase()"></div>
                  <span class="rating-count">{{ r.value }}</span>
                </div>
              </div>
            </mat-card-content>
          </mat-card>

          <mat-card>
            <mat-card-header><mat-card-title>Top Risks</mat-card-title></mat-card-header>
            <mat-card-content>
              <div *ngFor="let risk of data.topRisks" class="top-risk-row" [routerLink]="['/racm']">
                <span class="risk-name">{{ risk.name }}</span>
                <mat-chip [style.background]="getRatingColour(risk.netRiskRating)">
                  {{ risk.netRiskRating }} ({{ risk.netScore }})
                </mat-chip>
              </div>
            </mat-card-content>
          </mat-card>
        </div>
      </ng-container>
    </ng-container>

    <ng-template #spinner>
      <mat-progress-spinner mode="indeterminate"></mat-progress-spinner>
    </ng-template>

    <div *ngIf="!(selectedClientId$ | async)" class="no-client">
      <mat-card>
        <mat-card-content>
          <p>Please select a client from the sidebar to view the dashboard.</p>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .stats-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(180px,1fr)); gap: 16px; margin-bottom: 24px; }
    .stat-card { text-align: center; }
    .stat-value { font-size: 2.5rem; font-weight: 700; color: #1a237e; }
    .stat-label { font-size: 0.85rem; color: #666; margin-top: 4px; }
    .critical .stat-value { color: #b71c1c; }
    .high .stat-value { color: #e65100; }
    .warn .stat-value { color: #f57f17; }
    .breakdown-section { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
    .rating-row { display: flex; align-items: center; gap: 12px; margin: 8px 0; }
    .rating-label { width: 80px; font-size: 0.85rem; }
    .rating-bar { height: 20px; border-radius: 4px; background: #1a237e; transition: width 0.5s; }
    .bar-critical { background: #b71c1c; }
    .bar-high { background: #e65100; }
    .bar-medium { background: #f57f17; }
    .bar-low { background: #388e3c; }
    .bar-minimal { background: #90a4ae; }
    .rating-count { font-weight: 600; }
    .top-risk-row { display: flex; justify-content: space-between; align-items: center; padding: 8px 0; border-bottom: 1px solid #eee; cursor: pointer; }
    .top-risk-row:hover { background: #f5f5f5; }
    .risk-name { font-size: 0.9rem; }
    .no-client { margin-top: 40px; text-align: center; }
  `]
})
export class DashboardComponent implements OnInit {
  private readonly store = inject(Store);
  private readonly api = inject(ApiService);

  selectedClientId$ = this.store.select(selectSelectedClientId);
  racm$ = this.store.select(selectAllRacm);
  loading$ = this.store.select(selectRisksLoading);

  dashboardData$ = this.selectedClientId$.pipe(
    filter(id => !!id),
    switchMap(id => this.api.getDashboard(id!))
  );

  ngOnInit(): void {
    this.selectedClientId$.pipe(filter(id => !!id)).subscribe(id => {
      this.store.dispatch(loadRacm({ clientId: id! }));
    });
  }

  getRatingColour(rating: string): string {
    const map: Record<string, string> = {
      Critical: '#b71c1c', High: '#e65100', Medium: '#f57f17', Low: '#388e3c', Minimal: '#90a4ae'
    };
    return map[rating] || '#90a4ae';
  }

  asNum(v: unknown): number { return +(v as number); }
  asStr(v: unknown): string { return v as string; }
}
