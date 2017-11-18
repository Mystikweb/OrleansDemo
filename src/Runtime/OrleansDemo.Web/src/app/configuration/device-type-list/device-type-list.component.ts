import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';

import { DeviceType, DeviceTypeService, DeviceTypeDataSource } from '../../services/device-type.service';

@Component({
  selector: 'app-device-type-list',
  templateUrl: './device-type-list.component.html',
  styleUrls: ['./device-type-list.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class DeviceTypeListComponent implements OnInit {
  displayedColumns = ['Name', 'Active'];
  dataSource: DeviceTypeDataSource | null;
  deviceTypes: DeviceType[];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private deviceTypeApi: DeviceTypeService) { }

  ngOnInit() {
    this.dataSource = new DeviceTypeDataSource(this.deviceTypeApi, this.paginator, this.sort);
  }

}
