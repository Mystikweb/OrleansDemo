import { AfterViewInit, Component, ElementRef, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { MatPaginator, MatSnackBar, MatSort } from '@angular/material';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/debounceTime';

import { SensorConfig, SensorService, SensorDataSource } from '../../services/sensor.service';
import { DetailsHostItem, DetailsHostService } from '../../shared/details-host/details-host.service';
import { SensorEditorComponent } from './sensor-editor.component';

@Component({
  selector: 'app-sensor-list',
  templateUrl: './sensor-list.component.html',
  styleUrls: ['./sensor-list.component.css']
})
export class SensorListComponent implements OnInit, AfterViewInit {
  displayedColumns = ['edit', 'name', 'remove'];
  dataSource: SensorDataSource | null;
  sensorList: SensorConfig[];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('filter') filter: ElementRef;

  constructor(private sensorService: SensorService,
    private detailsService: DetailsHostService,
    private snackBar: MatSnackBar) { }

  ngOnInit() {
    this.dataSource = new SensorDataSource(this.sensorService, this.paginator, this.sort);
    Observable.fromEvent(this.filter.nativeElement, 'keyup')
      .debounceTime(150)
      .distinctUntilChanged()
      .subscribe(() => {
        if (!this.dataSource) { return; }
        this.dataSource.filter = this.filter.nativeElement.value;
      });
  }

  ngAfterViewInit() {
    this.refreshList();
  }

  refreshList() {
    this.sensorService.getSensorList().subscribe();
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
