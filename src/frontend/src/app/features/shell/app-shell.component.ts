import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { Store } from '@ngrx/store';
import { loadClients } from '../../core/store/clients/clients.actions';
import { selectAllClients, selectSelectedClientId } from '../../core/store/clients/clients.selectors';
import { selectClient } from '../../core/store/clients/clients.actions';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [AsyncPipe, NgFor, NgIf, MatSidenavModule, MatToolbarModule, MatListModule, MatIconModule, RouterLink, RouterLinkActive],
  template: `
    <mat-sidenav-container class="sidenav-container">
      <mat-sidenav mode="side" opened class="sidenav">
        <div class="sidenav-header">
          <span class="logo">TaxRACM</span>
          <span class="tagline">Tax Risk & Controls</span>
        </div>

        <div class="client-selector" *ngIf="(clients$ | async) as clients">
          <label>Client</label>
          <select (change)="onClientChange($event)">
            <option value="">— Select Client —</option>
            <option *ngFor="let c of clients" [value]="c.id" [selected]="c.id === (selectedClientId$ | async)">
              {{ c.name }}
            </option>
          </select>
        </div>

        <mat-nav-list>
          <a mat-list-item routerLink="/dashboard" routerLinkActive="active">
            <mat-icon matListItemIcon>dashboard</mat-icon>
            <span matListItemTitle>Dashboard</span>
          </a>
          <a mat-list-item routerLink="/racm" routerLinkActive="active">
            <mat-icon matListItemIcon>table_chart</mat-icon>
            <span matListItemTitle>RACM</span>
          </a>
          <a mat-list-item routerLink="/risk-bank" routerLinkActive="active">
            <mat-icon matListItemIcon>library_books</mat-icon>
            <span matListItemTitle>Risk Bank</span>
          </a>
          <a mat-list-item routerLink="/reports" routerLinkActive="active">
            <mat-icon matListItemIcon>description</mat-icon>
            <span matListItemTitle>Reports</span>
          </a>
          <a mat-list-item routerLink="/control-advisor" routerLinkActive="active">
            <mat-icon matListItemIcon>smart_toy</mat-icon>
            <span matListItemTitle>AI Advisor</span>
          </a>
          <a mat-list-item routerLink="/import" routerLinkActive="active">
            <mat-icon matListItemIcon>upload_file</mat-icon>
            <span matListItemTitle>Import</span>
          </a>
        </mat-nav-list>
      </mat-sidenav>

      <mat-sidenav-content class="main-content">
        <mat-toolbar class="toolbar">
          <span>TaxRACM — Tax Risk Assessment & Controls Matrix</span>
          <span class="spacer"></span>
          <span class="version">v1.0</span>
        </mat-toolbar>
        <div class="page-content">
          <ng-content></ng-content>
        </div>
      </mat-sidenav-content>
    </mat-sidenav-container>
  `,
  styles: [`
    .sidenav-container { height: 100vh; }
    .sidenav { width: 240px; background: #1a237e; color: white; }
    .sidenav-header { padding: 24px 16px 16px; border-bottom: 1px solid rgba(255,255,255,0.1); }
    .logo { display: block; font-size: 1.5rem; font-weight: 700; letter-spacing: 1px; }
    .tagline { display: block; font-size: 0.7rem; opacity: 0.7; margin-top: 4px; }
    .client-selector { padding: 16px; }
    .client-selector label { display: block; font-size: 0.75rem; opacity: 0.7; margin-bottom: 8px; }
    .client-selector select { width: 100%; padding: 6px; border-radius: 4px; border: none; font-size: 0.85rem; }
    .toolbar { background: #0d47a1; color: white; position: sticky; top: 0; z-index: 100; }
    .spacer { flex: 1; }
    .version { font-size: 0.75rem; opacity: 0.7; }
    .main-content { background: #f5f5f5; }
    .page-content { padding: 24px; min-height: calc(100vh - 64px); }
    mat-nav-list a { color: rgba(255,255,255,0.85) !important; }
    mat-nav-list a.active { background: rgba(255,255,255,0.15) !important; color: white !important; }
    mat-icon { color: rgba(255,255,255,0.85) !important; }
  `]
})
export class AppShellComponent implements OnInit {
  private readonly store = inject(Store);

  clients$ = this.store.select(selectAllClients);
  selectedClientId$ = this.store.select(selectSelectedClientId);

  ngOnInit(): void {
    this.store.dispatch(loadClients());
  }

  onClientChange(event: Event): void {
    const id = (event.target as HTMLSelectElement).value;
    if (id) this.store.dispatch(selectClient({ clientId: id }));
  }
}
