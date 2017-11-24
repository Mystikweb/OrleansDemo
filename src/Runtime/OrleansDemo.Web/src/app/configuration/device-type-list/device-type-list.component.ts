import { Component, ElementRef, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/debounceTime';

import { DetailsHostItem, DetailsHostService } from '../../shared/details-host/details-host.service';
import { DeviceType, DeviceTypeService, DeviceTypeDataSource } from '../../services/device-type.service';
import { DeviceTypeEditorComponent } from './device-type-editor.component';

@Component({
  selector: 'app-device-type-list',
  templateUrl: './device-type-list.component.html',
  styleUrls: ['./device-type-list.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class DeviceTypeListComponent implements OnInit {
  displayedColumns = ['edit', 'name', 'active', 'readings', 'remove'];
  dataSource: DeviceTypeDataSource | null;
  deviceTypes: DeviceType[];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('filter') filter: ElementRef;

  constructor(private deviceTypeService: DeviceTypeService,
    private detailsService: DetailsHostService) { }

  ngOnInit() {
    this.dataSource = new DeviceTypeDataSource(this.deviceTypeService, this.paginator, this.sort);
    Observable.fromEvent(this.filter.nativeElement, 'keyup')
      .debounceTime(150)
      .distinctUntilChanged()
      .subscribe(() => {
        if (!this.dataSource) { return; }
        this.dataSource.filter = this.filter.nativeElement.value;
      });
    this.refreshList();
  }

  refreshList() {
    this.deviceTypeService.getList().subscribe();
  }

  openEditor(type: DeviceType) {
    this.detailsService.openItem(new DetailsHostItem(DeviceTypeEditorComponent, type === null ? new DeviceType() : type));
  }

  delete(type: DeviceType) {
    console.log(type);
  }
}
