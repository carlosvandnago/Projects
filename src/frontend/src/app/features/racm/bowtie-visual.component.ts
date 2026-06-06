import { AfterViewInit, Component, ElementRef, inject, ViewChild } from '@angular/core';
import { AsyncPipe, NgIf } from '@angular/common';
import { Store } from '@ngrx/store';
import * as d3 from 'd3';
import { filter, switchMap } from 'rxjs';
import { selectSelectedClientId } from '../../core/store/clients/clients.selectors';
import { loadBowTie } from '../../core/store/risks/risks.actions';
import { selectBowTie, selectSelectedRiskId } from '../../core/store/risks/risks.selectors';

@Component({
  selector: 'app-bowtie-visual',
  standalone: true,
  imports: [AsyncPipe, NgIf],
  template: `
    <div class="bowtie-wrapper">
      <h4 *ngIf="!(bowTie$ | async)">Select a risk and it will load the bow-tie diagram.</h4>
      <svg #svgEl width="100%" height="460"></svg>
    </div>
  `,
  styles: [`
    .bowtie-wrapper { padding: 8px; }
    svg { background: #fafafa; border-radius: 8px; }
  `]
})
export class BowTieVisualComponent implements AfterViewInit {
  @ViewChild('svgEl', { static: true }) svgEl!: ElementRef<SVGSVGElement>;

  private readonly store = inject(Store);

  bowTie$ = this.store.select(selectBowTie);

  ngAfterViewInit(): void {
    const clientId$ = this.store.select(selectSelectedClientId);
    const riskId$ = this.store.select(selectSelectedRiskId);

    clientId$.pipe(filter(c => !!c)).subscribe(clientId => {
      riskId$.pipe(filter(r => !!r)).subscribe(riskId => {
        this.store.dispatch(loadBowTie({ clientId: clientId!, riskId: riskId! }));
      });
    });

    this.bowTie$.pipe(filter(d => !!d)).subscribe(data => {
      if (data) this.renderBowTie(data);
    });
  }

  private renderBowTie(data: any): void {
    const el = this.svgEl.nativeElement;
    const svg = d3.select(el);
    svg.selectAll('*').remove();

    const width = el.clientWidth || 800;
    const height = 460;
    const cx = width / 2;
    const cy = height / 2;

    const colours: Record<string, string> = {
      Preventive: '#1565c0', Mitigating: '#6a1b9a', Detective: '#00695c'
    };

    svg.append('circle').attr('cx', cx).attr('cy', cy).attr('r', 60).attr('fill', '#e53935').attr('opacity', 0.9);
    svg.append('text').attr('x', cx).attr('y', cy - 8).attr('text-anchor', 'middle').attr('fill', 'white').attr('font-size', 11).attr('font-weight', 700).text(data.riskName?.substring(0, 18) + '…');
    svg.append('text').attr('x', cx).attr('y', cy + 8).attr('text-anchor', 'middle').attr('fill', 'white').attr('font-size', 10).text(`Gross:${data.grossScore} Net:${data.netScore}`);

    const causes: string[] = data.causes || [];
    causes.forEach((cause: string, i: number) => {
      const y = cy - ((causes.length - 1) / 2 - i) * 60;
      svg.append('line').attr('x1', 100).attr('y1', y).attr('x2', cx - 60).attr('y2', cy).attr('stroke', '#ef9a9a').attr('stroke-width', 1.5);
      const g = svg.append('g').attr('transform', `translate(80,${y})`);
      g.append('rect').attr('x', -75).attr('y', -15).attr('width', 150).attr('height', 30).attr('rx', 6).attr('fill', '#ffcdd2');
      g.append('text').attr('text-anchor', 'middle').attr('dominant-baseline', 'middle').attr('font-size', 9).text(cause.substring(0, 20));
    });

    const consequences: string[] = data.consequences || [];
    consequences.forEach((cons: string, i: number) => {
      const y = cy - ((consequences.length - 1) / 2 - i) * 60;
      svg.append('line').attr('x1', cx + 60).attr('y1', cy).attr('x2', width - 100).attr('y2', y).attr('stroke', '#ce93d8').attr('stroke-width', 1.5);
      const g = svg.append('g').attr('transform', `translate(${width - 80},${y})`);
      g.append('rect').attr('x', -75).attr('y', -15).attr('width', 150).attr('height', 30).attr('rx', 6).attr('fill', '#e1bee7');
      g.append('text').attr('text-anchor', 'middle').attr('dominant-baseline', 'middle').attr('font-size', 9).text(cons.substring(0, 20));
    });

    const allControls = [
      ...(data.preventiveControls || []).map((c: any) => ({ ...c, type: 'Preventive' })),
      ...(data.mitigatingControls || []).map((c: any) => ({ ...c, type: 'Mitigating' })),
      ...(data.detectiveControls || []).map((c: any) => ({ ...c, type: 'Detective' }))
    ];

    const controlsPerSide = Math.ceil(allControls.length / 2);
    allControls.forEach((ctrl: any, i: number) => {
      const side = i < controlsPerSide ? -1 : 1;
      const idx = i < controlsPerSide ? i : i - controlsPerSide;
      const x = cx + side * (130 + Math.floor(idx / 3) * 90);
      const y = cy + (idx % 3 - 1) * 55;
      const g = svg.append('g').attr('transform', `translate(${x},${y})`);
      g.append('rect').attr('x', -50).attr('y', -15).attr('width', 100).attr('height', 30).attr('rx', 6)
        .attr('fill', colours[ctrl.type] || '#78909c').attr('opacity', 0.85);
      g.append('text').attr('text-anchor', 'middle').attr('dominant-baseline', 'middle').attr('fill', 'white').attr('font-size', 8).text(ctrl.name?.substring(0, 14));
    });
  }
}
