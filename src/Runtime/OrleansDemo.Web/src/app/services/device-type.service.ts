import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { DataSource, SelectionModel } from '@angular/cdk/collections';
import { MatPaginator, MatSort } from '@angular/material';

import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { catchError, map, tap } from 'rxjs/operators';
import 'rxjs/add/observable/merge';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/operator/switchMap';

import { environment } from '../../environments/environment';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

export class DeviceType {
  id: number;
  name: string;
  active: boolean;
  readingTypes: DeviceReadingType[];
}

export class DeviceReadingType {
  id: number;
  readingTypeId: number;
  readingType: string;
  readingTypeUom: string;
  readingTypeDataType: string;
  active: boolean;
}

@Injectable()
export class DeviceTypeService {
  private deviceTypeUrl = environment.rootUri + '/api/devicetype';

  dataChange: BehaviorSubject<DeviceType[]> = new BehaviorSubject<DeviceType[]>([]);
  get data(): DeviceType[] { return this.dataChange.value; }

  constructor(private http: HttpClient) { }

  getList(): Observable<DeviceType[]> {
    return this.http.get<DeviceType[]>(this.deviceTypeUrl)
      .pipe(
        tap(types => {
          let copiedData = this.data.slice();
          copiedData = types;
          this.dataChange.next(copiedData);
        })
      );
  }

  get(id: number) {
    const requestUrl = `${this.deviceTypeUrl}/${id}`;
    return this.http.get<DeviceType>(requestUrl);
  }

  save(deviceType: DeviceType): Observable<DeviceType> {
    return this.http.post<DeviceType>(this.deviceTypeUrl, deviceType, httpOptions)
      .pipe(
        tap(t => {
          const copiedData = this.data.slice();
          copiedData.push(t);
          this.dataChange.next(copiedData);
        })
      );
  }

  update(deviceType: DeviceType) {
    const requestUrl = `${this.deviceTypeUrl}/${deviceType.id}`;
    return this.http.put(requestUrl, deviceType, httpOptions);
  }

  delete(id: number): Observable<DeviceType> {
    const requestUrl = `${this.deviceTypeUrl}/${id}`;
    return this.http.delete<DeviceType>(requestUrl, httpOptions)
      .pipe(
        tap(t => {
          const copiedData = this.data.slice();
          const itemIndex = copiedData.findIndex(item => item.id === t.id);
          copiedData.splice(itemIndex, 1);
          this.dataChange.next(copiedData);
        })
      );
  }
}

export class DeviceTypeDataSource extends DataSource<DeviceType> {
  resultsLength = 0;
  isLoadingResults = false;

  filterChange: BehaviorSubject<string> = new BehaviorSubject<string>('');
  get filter(): string { return this.filterChange.value; }
  set filter(filter: string) { this.filterChange.next(filter); }

  filteredData: DeviceType[] = [];
  renderedData: DeviceType[] = [];

  constructor(private deviceTypeService: DeviceTypeService,
              private paginator: MatPaginator,
              private sort: MatSort) {
    super();
  }

  connect(): Observable<DeviceType[]> {
    const displayDataChanges = [
      this.deviceTypeService.dataChange,
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
        this.filteredData = this.deviceTypeService.data.slice().filter((item: DeviceType) => {
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
  sortData(data: DeviceType[]): DeviceType[] {
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
