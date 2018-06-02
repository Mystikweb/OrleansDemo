import { AfterViewInit, Component, ViewChild, ElementRef } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource, MatSnackBar } from '@angular/material';
import { merge, Observable, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';

import { SensorConfig, SensorService } from '../../services/sensor.service';
import { DetailsHostItem, DetailsHostService } from '../../shared/details-host/details-host.service';
import { SensorEditorComponent } from './sensor-editor.component';

@Component({
  selector: 'app-sensor-list',
  templateUrl: './sensor-list.component.html',
  styleUrls: ['./sensor-list.component.css']
})
export class SensorListComponent implements AfterViewInit {
  displayedColumns = ['edit', 'name', 'uom', 'remove'];
  dataSource: MatTableDataSource<SensorConfig>;
  sensorList: SensorConfig[];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('filter') filter: ElementRef;

  constructor(private sensorService: SensorService,
    private detailsService: DetailsHostService,
    private snackBar: MatSnackBar) { }

  ngAfterViewInit() {
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        startWith({}),
        switchMap(() => {
          return this.sensorService.getSensorList(this.sort.active, this.sort.direction, this.paginator.pageIndex, '');
        }),
        map(data => {
          return data;
        }),
        catchError(() => {
          return observableOf([]);
        })
      ).subscribe(data => this.dataSource.data = data);
  }

  openEditor(sensor: SensorConfig) {
    this.detailsService.openItem(new DetailsHostItem(SensorEditorComponent, sensor === null ? new SensorConfig() : sensor));
  }

  delete(sensor: SensorConfig) {
    this.sensorService.remove(sensor.sensorId).subscribe(result => {
      this.snackBar.open('Removed successfully', 'Close', { duration: 1500 });
    });
  }
}
