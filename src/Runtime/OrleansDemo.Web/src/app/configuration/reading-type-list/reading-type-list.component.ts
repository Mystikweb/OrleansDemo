import { Component, ElementRef, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MatPaginator, MatSort } from '@angular/material';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/observable/merge';
import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/debounceTime';

import { ReadingType, ReadingTypeService, ReadingTypeDataSource } from '../../services/reading-type.service';

import { ReadingTypeDialogComponent } from './reading-type-dialog.component';

@Component({
  selector: 'app-reading-type-list',
  templateUrl: './reading-type-list.component.html',
  styleUrls: ['./reading-type-list.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ReadingTypeListComponent implements OnInit {
  displayedColumns = ['name', 'uom', 'dataType'];
  dataSource: ReadingTypeDataSource | null;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('filter') filter: ElementRef;

  constructor(private readingTypeService: ReadingTypeService,
              private dialog: MatDialog) { }

  ngOnInit() {
    this.dataSource = new ReadingTypeDataSource(this.readingTypeService, this.paginator, this.sort);
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
    this.readingTypeService.getList().subscribe();
  }

  openDialog(type: ReadingType) {
    const dialogRef = this.dialog.open(ReadingTypeDialogComponent, {
      disableClose: true,
      data: type
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(result);
    });
  }
}
