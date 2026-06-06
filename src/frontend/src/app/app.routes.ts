import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent) },
  { path: 'racm', loadComponent: () => import('./features/racm/racm.component').then(m => m.RacmComponent) },
  { path: 'risk-bank', loadComponent: () => import('./features/risk-bank/risk-bank.component').then(m => m.RiskBankComponent) },
  { path: 'reports', loadComponent: () => import('./features/reports/reports.component').then(m => m.ReportsComponent) },
  { path: 'control-advisor', loadComponent: () => import('./features/control-advisor/control-advisor.component').then(m => m.ControlAdvisorComponent) },
  { path: 'import', loadComponent: () => import('./features/import/import.component').then(m => m.ImportComponent) },
];
