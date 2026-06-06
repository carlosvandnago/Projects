export interface ReportDto {
  aiRequestId: string;
  reportType: string;
  markdownContent: string;
  tokensUsed: number;
  generatedAt: string;
}

export interface RacmSummaryDto {
  name: string;
  taxType: string;
  grossScore: number;
  netScore: number;
  netRating: string;
  totalControls: number;
  evidencedControls: number;
  outstandingActions: number;
  notes?: string;
}
