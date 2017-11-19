import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';

import { Device, DeviceService, DeviceDataSource } from '../../services/device.service';

@Component({
  selector: 'app-device-list',
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class DeviceListComponent implements OnInit {
  displayColumns = [
    'Name',
    'DeviceTypeId',
    'Enabled',
    'RunOnStartup',
    'CreatedAt',
    'CreatedBy',
    'UpdatedAt',
    'UpdatedBy'
  ];
  dataSource: DeviceDataSource | null;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private deviceService: DeviceService) { }

  ngOnInit() {
    this.dataSource = new DeviceDataSource(this.deviceService, this.paginator, this.sort);
  }

}
