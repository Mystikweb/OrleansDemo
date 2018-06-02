import { AfterViewInit, Component, OnInit } from '@angular/core';

import { Observable } from 'rxjs';

import { DetailsHostItem, DetailsHostService } from '../../shared/details-host/details-host.service';
import { DeviceListComponent } from './device-list.component';

import { DeviceSummary, SensorSummary, DashboardService } from '../../services/dashboard.service';

@Component({
  selector: 'app-dashboard-home',
  templateUrl: './dashboard-home.component.html',
  styleUrls: ['./dashboard-home.component.css']
})
export class DashboardHomeComponent implements AfterViewInit, OnInit {
  dashboard$: Observable<DeviceSummary[]>;

  constructor(private dashboardService: DashboardService,
    private detailsHostService: DetailsHostService) { }

  ngOnInit() {
    this.detailsHostService.detailsOpen$.subscribe(opened => {
      if (opened === false) {
        this.loadData();
      }
    });
  }

  ngAfterViewInit() {
    this.loadData();
  }

  showDevices() {
    this.detailsHostService.openItem(new DetailsHostItem(DeviceListComponent, null));
  }

  private loadData() {
    this.dashboard$ = this.dashboardService.getDashboardSummary();
  }
}
