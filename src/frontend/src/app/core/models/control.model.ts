export interface Evidence {
  id: string;
  fileName: string;
  fileUrl: string;
  uploadedById: string;
  notes: string;
  uploadedAt: string;
}

export interface Control {
  id: string;
  racmEntryId: string;
  name: string;
  description: string;
  controlType: string;
  ownerId: string;
  frequency: string;
  lastTested?: string;
  nextDue?: string;
  evidenceStatus: string;
  requiresReview: boolean;
  reviewerId?: string;
  reviewStatus: string;
  effectiveness: string;
  evidence: Evidence[];
}
