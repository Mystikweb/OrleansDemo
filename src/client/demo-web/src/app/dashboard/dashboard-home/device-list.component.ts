import { AfterViewInit, Component } from '@angular/core';

import { Observable } from 'rxjs';

import { IDetailsHostComponent, DetailsHostService } from '../../shared/details-host/details-host.service';
import { DeviceStatus, DashboardService } from '../../services/dashboard.service';

@Component({
  selector: 'app-device-list',
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.css']
})
export class DeviceListComponent implements IDetailsHostComponent, AfterViewInit {
  data: any;
  deviceList$: Observable<DeviceStatus[]>;
  displayColumns: String[] = ['name', 'isRunning'];

  constructor(private dashboardService: DashboardService,
    private detailsHostService: DetailsHostService) { }

  ngAfterViewInit() {
    this.loadData();
  }

  startDevice(device: DeviceStatus) {
    this.dashboardService.startDevice(device)
      .subscribe(() => this.loadData());
  }

  stopDevice(device: DeviceStatus) {
    this.dashboardService.stopDevice(device)
      .subscribe(() => this.loadData());
  }

  close() {
    this.detailsHostService.closeItem();
  }

  private loadData() {
    this.deviceList$ = this.dashboardService.getRuntimeList();
  }
}
