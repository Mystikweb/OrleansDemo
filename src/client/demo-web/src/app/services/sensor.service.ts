import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { DataSource, SelectionModel } from '@angular/cdk/collections';
import { MatPaginator, MatSort } from '@angular/material';

import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { catchError, tap } from 'rxjs/operators';
import 'rxjs/add/observable/merge';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/operator/switchMap';

import { environment } from '../../environments/environment';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

export class SensorConfig {
  sensorId: number;
  name: string;
  uom: string;
}

@Injectable()
export class SensorService {

  private sensorUri = environment.rootUri + '/api/sensor';

  dataChange: BehaviorSubject<SensorConfig[]> = new BehaviorSubject<SensorConfig[]>([]);
  get data(): SensorConfig[] { return this.dataChange.value; }

  constructor(private http: HttpClient) { }

  getSensorList(): Observable<SensorConfig[]> {
    return this.http.get<SensorConfig[]>(this.sensorUri)
      .pipe(
        tap(types => {
          let copiedData = this.data.slice();
          copiedData = types;
          this.dataChange.next(copiedData);
        })
      );
  }

  getSensor(sensorId): Observable<SensorConfig> {
    const request = `${this.sensorUri}/${sensorId}`;
    return this.http.get<SensorConfig>(request);
  }

  save(sensor: SensorConfig): Observable<SensorConfig> {
    return this.http.post<SensorConfig>(this.sensorUri, sensor, httpOptions)
      .pipe(
        tap(t => {
          const copiedData = this.data.slice();
          copiedData.push(t);
          this.dataChange.next(copiedData);
        })
      );
  }

  remove(sensorId: number) {
    const request = `${this.sensorUri}/${sensorId}`;
    return this.http.delete(request)
      .pipe(
        tap(t => {
          const copiedData = this.data.slice();
          const removed = copiedData.find(s => s.sensorId === sensorId);
          copiedData.splice(copiedData.indexOf(removed), 1);
          this.dataChange.next(copiedData);
        })
      );
  }
}

export class SensorDataSource extends DataSource<SensorConfig> {
  resultsLength = 0;
  isLoadingResults = false;

  filterChange: BehaviorSubject<string> = new BehaviorSubject<string>('');
  get filter(): string { return this.filterChange.value; }
  set filter(filter: string) { this.filterChange.next(filter); }

  filteredData: SensorConfig[] = [];
  renderedData: SensorConfig[] = [];

  constructor(private sensorService: SensorService,
              private paginator: MatPaginator,
              private sort: MatSort) {
    super();
  }

  connect(): Observable<SensorConfig[]> {
    const displayDataChanges = [
      this.sensorService.dataChange,
      this.filterChange,
      this.sort.sortChange,
      this.paginator.page
    ];

    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    return Observable.merge(...displayDataChanges)
      .startWith(null)
      .map(() => {
        this.isLoadingResults = true;

        // Filter data
        this.filteredData = this.sensorService.data.slice().filter((item: SensorConfig) => {
          const searchStr = (item.name).toLowerCase();
          return searchStr.indexOf(this.filter.toLowerCase()) !== -1;
        });

        // Sort filtered data
        const sortedData = this.sortData(this.filteredData.slice());

        // Grab the page's slice of the filtered sorted data.
        const startIndex = this.paginator.pageIndex * this.paginator.pageSize;
        this.renderedData = sortedData.splice(startIndex, this.paginator.pageSize);

        this.isLoadingResults = false;
        this.resultsLength = this.renderedData.length;

        return this.renderedData;
      })
      .catch(() => {
        this.isLoadingResults = false;
        return Observable.of([]);
      });
  }

  disconnect() {}

  /** Returns a sorted copy of the database data. */
  sortData(data: SensorConfig[]): SensorConfig[] {
    if (!this.sort.active || this.sort.direction === '') { return data; }

    return data.sort((a, b) => {
      let propertyA: number|string = '';
      let propertyB: number|string = '';

      switch (this.sort.active) {
        case 'name': [propertyA, propertyB] = [a.name, b.name]; break;
      }

      const valueA = isNaN(+propertyA) ? propertyA : +propertyA;
      const valueB = isNaN(+propertyB) ? propertyB : +propertyB;

      return (valueA < valueB ? -1 : 1) * (this.sort.direction === 'asc' ? 1 : -1);
    });
  }
}
