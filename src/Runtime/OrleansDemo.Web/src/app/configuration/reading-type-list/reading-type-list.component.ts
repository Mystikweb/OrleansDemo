import { Component, ElementRef, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/debounceTime';

import { DetailsHostItem, DetailsHostService } from '../../shared/details-host/details-host.service';
import { ReadingType, ReadingTypeService, ReadingTypeDataSource } from '../../services/reading-type.service';
import { ReadingTypeEditorComponent } from './reading-type-editor.component';

@Component({
  selector: 'app-reading-type-list',
  templateUrl: './reading-type-list.component.html',
  styleUrls: ['./reading-type-list.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ReadingTypeListComponent implements OnInit {
  displayedColumns = ['edit', 'name', 'uom', 'dataType', 'remove'];
  dataSource: ReadingTypeDataSource | null;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('filter') filter: ElementRef;

  constructor(private readingTypeService: ReadingTypeService,
    private detailsHost: DetailsHostService) { }

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

  openEditor(type: ReadingType) {
    this.detailsHost.openItem(new DetailsHostItem(ReadingTypeEditorComponent, type === null ? new ReadingType() : type));
  }

  delete(type: ReadingType) {
    console.log(type);
  }
}
