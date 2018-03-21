import { AfterViewInit, Component } from '@angular/core';

import { Observable } from 'rxjs/Observable';

import { DetailsHostItem, DetailsHostService } from '../../shared/details-host/details-host.service';
import { DeviceListComponent } from './device-list.component';

import { DeviceSummary, SensorSummary, DashboardService } from '../../services/dashboard.service';

@Component({
  selector: 'app-dashboard-home',
  templateUrl: './dashboard-home.component.html',
  styleUrls: ['./dashboard-home.component.css']
})
export class DashboardHomeComponent implements AfterViewInit {
  dashboard$: Observable<DeviceSummary[]>;

  constructor(private dashboardService: DashboardService,
    private detailsHostService: DetailsHostService) { }

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
