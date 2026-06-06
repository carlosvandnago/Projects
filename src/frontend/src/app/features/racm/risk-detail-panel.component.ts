import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { Store } from '@ngrx/store';
import { filter, switchMap } from 'rxjs';
import { ApiService } from '../../core/services/api.service';
import { Control } from '../../core/models/control.model';
import { selectSelectedClientId } from '../../core/store/clients/clients.selectors';
import { selectSelectedRisk } from '../../core/store/risks/risks.selectors';
import { BowTieVisualComponent } from './bowtie-visual.component';

@Component({
  selector: 'app-risk-detail-panel',
  standalone: true,
  imports: [AsyncPipe, NgFor, NgIf, MatCardModule, MatTabsModule, MatChipsModule, MatExpansionModule, MatDividerModule, MatIconModule, MatButtonModule, BowTieVisualComponent],
  template: `
    <mat-card class="detail-card">
      <mat-card-header>
        <mat-card-title>{{ (risk$ | async)?.name }}</mat-card-title>
        <mat-card-subtitle>{{ (risk$ | async)?.taxType }} · {{ (risk$ | async)?.status }}</mat-card-subtitle>
      </mat-card-header>
      <mat-card-content>
        <mat-tab-group>
          <mat-tab label="Overview">
            <div class="tab-content" *ngIf="risk$ | async as risk">
              <p class="description">{{ risk.description }}</p>
              <div class="scores">
                <div class="score-block">
                  <div class="score-label">Gross Risk</div>
                  <div class="score-val">{{ risk.grossScore?.score }}/25</div>
                  <mat-chip [style.background]="getColour(risk.grossRiskRating)">{{ risk.grossRiskRating }}</mat-chip>
                </div>
                <div class="arrow">→</div>
                <div class="score-block">
                  <div class="score-label">Net Risk</div>
                  <div class="score-val">{{ risk.netScore?.score }}/25</div>
                  <mat-chip [style.background]="getColour(risk.netRiskRating)">{{ risk.netRiskRating }}</mat-chip>
                </div>
                <div class="score-block">
                  <div class="score-label">CE Delta</div>
                  <div class="score-val delta">-{{ risk.controlEffectivenessDelta }}</div>
                </div>
              </div>

              <mat-expansion-panel>
                <mat-expansion-panel-header>
                  <mat-panel-title>Causes ({{ risk.causes?.length }})</mat-panel-title>
                </mat-expansion-panel-header>
                <ul><li *ngFor="let c of risk.causes">{{ c }}</li></ul>
              </mat-expansion-panel>

              <mat-expansion-panel>
                <mat-expansion-panel-header>
                  <mat-panel-title>Consequences ({{ risk.consequences?.length }})</mat-panel-title>
                </mat-expansion-panel-header>
                <ul><li *ngFor="let c of risk.consequences">{{ c }}</li></ul>
              </mat-expansion-panel>
            </div>
          </mat-tab>

          <mat-tab label="Bow-Tie">
            <div class="tab-content">
              <app-bowtie-visual></app-bowtie-visual>
            </div>
          </mat-tab>

          <mat-tab label="Controls">
            <div class="tab-content">
              <div *ngFor="let ctrl of controls" class="control-row">
                <div class="ctrl-header">
                  <strong>{{ ctrl.name }}</strong>
                  <mat-chip>{{ ctrl.evidenceStatus }}</mat-chip>
                </div>
                <div class="ctrl-meta">{{ ctrl.controlType }} · {{ ctrl.frequency }}</div>
                <div class="ctrl-effectiveness">Effectiveness: {{ ctrl.effectiveness }}</div>
              </div>
              <p *ngIf="!controls.length" class="empty">No controls yet.</p>
            </div>
          </mat-tab>
        </mat-tab-group>
      </mat-card-content>
    </mat-card>
  `,
  styles: [`
    .detail-card { height: 100%; }
    .tab-content { padding: 16px 0; }
    .description { color: #555; font-size: 0.9rem; margin-bottom: 16px; }
    .scores { display: flex; align-items: center; gap: 16px; margin-bottom: 16px; }
    .score-block { text-align: center; }
    .score-label { font-size: 0.75rem; color: #888; }
    .score-val { font-size: 1.5rem; font-weight: 700; color: #1a237e; }
    .delta { color: #388e3c; }
    .arrow { font-size: 1.5rem; color: #888; }
    ul { margin: 8px 0; padding-left: 20px; }
    li { font-size: 0.85rem; margin: 4px 0; }
    .control-row { padding: 12px; border-bottom: 1px solid #eee; }
    .ctrl-header { display: flex; justify-content: space-between; align-items: center; }
    .ctrl-meta { font-size: 0.8rem; color: #888; margin: 4px 0; }
    .ctrl-effectiveness { font-size: 0.8rem; }
    .empty { color: #888; font-style: italic; text-align: center; padding: 16px; }
  `]
})
export class RiskDetailPanelComponent implements OnInit {
  private readonly store = inject(Store);
  private readonly api = inject(ApiService);

  risk$ = this.store.select(selectSelectedRisk);
  controls: Control[] = [];

  ngOnInit(): void {
    this.risk$.pipe(filter(r => !!r)).subscribe(risk => {
      this.api.getControlsByRisk(risk!.id).subscribe(controls => {
        this.controls = controls;
      });
    });
  }

  getColour(rating: string): string {
    const map: Record<string, string> = {
      Critical: '#b71c1c', High: '#e65100', Medium: '#f57f17', Low: '#388e3c', Minimal: '#90a4ae'
    };
    return map[rating] || '#90a4ae';
  }
}
