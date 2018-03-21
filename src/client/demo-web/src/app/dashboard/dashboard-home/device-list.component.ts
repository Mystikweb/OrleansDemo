import { AfterViewInit, Component } from '@angular/core';

import { Observable } from 'rxjs/Observable';

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
    this.deviceList$ = this.dashboardService.getRuntimeList();
  }

  close() {
    this.detailsHostService.closeItem();
  }
}
