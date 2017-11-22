import { Component, ElementRef, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MatPaginator, MatSort } from '@angular/material';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/observable/merge';
import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/debounceTime';

import { DeviceType, DeviceTypeService, DeviceTypeDataSource } from '../../services/device-type.service';

import { DeviceTypeDialogComponent } from './device-type-dialog.component';

@Component({
  selector: 'app-device-type-list',
  templateUrl: './device-type-list.component.html',
  styleUrls: ['./device-type-list.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class DeviceTypeListComponent implements OnInit {
  displayedColumns = ['name', 'active'];
  dataSource: DeviceTypeDataSource | null;
  deviceTypes: DeviceType[];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('filter') filter: ElementRef;

  constructor(private deviceTypeService: DeviceTypeService,
              private dialog: MatDialog) { }

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

  openDialog(type: DeviceType) {
    const dialogRef = this.dialog.open(DeviceTypeDialogComponent, {
      disableClose: true,
      data: type
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(result);
    });
  }
}
