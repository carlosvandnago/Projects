import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Client, TaxEntity } from '../models/client.model';
import { Control } from '../models/control.model';
import { ReportDto, RacmSummaryDto } from '../models/report.model';
import { BowTieData, RacmEntry, RiskBankEntry } from '../models/risk.model';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly http = inject(HttpClient);
  private readonly base = environment.apiUrl;

  // ── Clients ────────────────────────────────────────────────────────────────
  getClients(): Observable<Client[]> {
    return this.http.get<Client[]>(`${this.base}/clients`);
  }

  getClient(id: string): Observable<Client> {
    return this.http.get<Client>(`${this.base}/clients/${id}`);
  }

  // ── RACM ───────────────────────────────────────────────────────────────────
  getRacm(clientId: string, taxType?: string, rating?: string): Observable<RacmEntry[]> {
    let params = new HttpParams();
    if (taxType) params = params.set('taxType', taxType);
    if (rating) params = params.set('rating', rating);
    return this.http.get<RacmEntry[]>(`${this.base}/racm/${clientId}`, { params });
  }

  getRisk(clientId: string, riskId: string): Observable<RacmEntry> {
    return this.http.get<RacmEntry>(`${this.base}/racm/${clientId}/${riskId}`);
  }

  getBowTie(clientId: string, riskId: string): Observable<BowTieData> {
    return this.http.get<BowTieData>(`${this.base}/racm/${clientId}/${riskId}/bowtie`);
  }

  toggleBankRisk(clientId: string, riskBankEntryId: string, requestedById: string): Observable<string> {
    return this.http.post<string>(`${this.base}/racm/${clientId}/toggle-bank-risk`, {
      riskBankEntryId,
      requestedById
    });
  }

  updateRiskScore(clientId: string, riskId: string, netLikelihood: number, netImpact: number): Observable<void> {
    return this.http.put<void>(`${this.base}/racm/${clientId}/${riskId}/score`, { netLikelihood, netImpact });
  }

  updateNarrative(clientId: string, riskId: string, causes: string[], consequences: string[], notes: string): Observable<void> {
    return this.http.put<void>(`${this.base}/racm/${clientId}/${riskId}/narrative`, { causes, consequences, notes });
  }

  // ── Risk Bank ──────────────────────────────────────────────────────────────
  getRiskBank(taxType?: string, country?: string, industry?: string): Observable<RiskBankEntry[]> {
    let params = new HttpParams();
    if (taxType) params = params.set('taxType', taxType);
    if (country) params = params.set('country', country);
    if (industry) params = params.set('industry', industry);
    return this.http.get<RiskBankEntry[]>(`${this.base}/risk-bank`, { params });
  }

  // ── Controls ───────────────────────────────────────────────────────────────
  getControlsByRisk(racmEntryId: string): Observable<Control[]> {
    return this.http.get<Control[]>(`${this.base}/controls/risk/${racmEntryId}`);
  }

  getOverdueControls(): Observable<Control[]> {
    return this.http.get<Control[]>(`${this.base}/controls/overdue`);
  }

  getUpcomingDeadlines(days = 30): Observable<Control[]> {
    return this.http.get<Control[]>(`${this.base}/controls/upcoming`, {
      params: new HttpParams().set('days', days.toString())
    });
  }

  addControl(racmEntryId: string, name: string, description: string, controlType: string, ownerId: string, frequency: string): Observable<Control> {
    return this.http.post<Control>(`${this.base}/controls`, { racmEntryId, name, description, controlType, ownerId, frequency });
  }

  // ── Intelligence ───────────────────────────────────────────────────────────
  analyseDocument(clientId: string, requestedById: string, taxType: string, entityContext: string, documentText: string): Observable<any> {
    return this.http.post<any>(`${this.base}/intelligence/analyse-document`, {
      clientId, requestedById, taxType, entityContext, documentText
    });
  }

  assessControl(clientId: string, requestedById: string, riskName: string, grossScore: number, netScore: number, existingControls: string[], controlToAssess: string): Observable<any> {
    return this.http.post<any>(`${this.base}/intelligence/assess-control`, {
      clientId, requestedById, riskName, grossScore, netScore, existingControls, controlToAssess
    });
  }

  // ── Reports ────────────────────────────────────────────────────────────────
  generateReport(clientId: string, requestedById: string, clientName: string, reportType: string, entityFilter: string | null, racmEntries: RacmSummaryDto[]): Observable<ReportDto> {
    return this.http.post<ReportDto>(`${this.base}/reports/${clientId}`, {
      requestedById, clientName, reportType, entityFilter, racmEntries
    });
  }

  // ── Dashboard ─────────────────────────────────────────────────────────────
  getDashboard(clientId: string): Observable<any> {
    return this.http.get<any>(`${this.base}/dashboard/${clientId}`);
  }
}
