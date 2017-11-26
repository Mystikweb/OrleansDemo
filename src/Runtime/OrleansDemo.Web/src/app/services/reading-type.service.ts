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

export class ReadingType {
  id: number;
  name: string;
  uom: string;
  dataType: string;
  assembly: string;
  class: string;
  method: string;
}

export class ReadingDeviceType {
  id: number;
  deviceTypeId: number;
  deviceType: string;
  active: boolean;
}

@Injectable()
export class ReadingTypeService {
  private readingTypeUrl = environment.rootUri + '/api/readingtype';

  dataChange: BehaviorSubject<ReadingType[]> = new BehaviorSubject<ReadingType[]>([]);
  get data(): ReadingType[] {
    return this.dataChange.value;
  }

  constructor(private http: HttpClient) {}

  getList(): Observable<ReadingType[]> {
    return this.http.get<ReadingType[]>(this.readingTypeUrl).pipe(
      tap(types => {
        let copiedData = this.data.slice();
        copiedData = types;
        this.dataChange.next(copiedData);
      })
    );
  }

  get(id: number): Observable<ReadingType> {
    const requestUrl = `${this.readingTypeUrl}/${id}`;
    return this.http.get<ReadingType>(requestUrl);
  }

  save(readingType: ReadingType): Observable<ReadingType> {
    return this.http
      .post<ReadingType>(this.readingTypeUrl, readingType, httpOptions)
      .pipe(
        tap(t => {
          const copiedData = this.data.slice();
          copiedData.push(t);
          this.dataChange.next(copiedData);
        })
      );
  }

  update(readingType: ReadingType): Observable<ReadingType> {
    const requestUrl = `${this.readingTypeUrl}/${readingType.id}`;
    return this.http
      .put<ReadingType>(requestUrl, readingType, httpOptions)
      .pipe(
        tap(t => {
          const copiedData = this.data.slice();
          const itemIndex = copiedData.findIndex(item => item.id === t.id);
          copiedData[itemIndex] = t;
          this.dataChange.next(copiedData);
        })
      );
  }

  delete(id: number): Observable<ReadingType> {
    const requestUrl = `${this.readingTypeUrl}/${id}`;
    return this.http.delete<ReadingType>(requestUrl, httpOptions).pipe(
      tap(t => {
        const copiedData = this.data.slice();
        const itemIndex = copiedData.findIndex(item => item.id === t.id);
        copiedData.splice(itemIndex, 1);
        this.dataChange.next(copiedData);
      })
    );
  }
}

export class ReadingTypeDataSource extends DataSource<ReadingType> {
  resultsLength = 0;
  isLoadingResults = false;

  filterChange: BehaviorSubject<string> = new BehaviorSubject<string>('');
  get filter(): string {
    return this.filterChange.value;
  }
  set filter(filter: string) {
    this.filterChange.next(filter);
  }

  filteredData: ReadingType[] = [];
  renderedData: ReadingType[] = [];

  constructor(
    private readingTypeService: ReadingTypeService,
    private paginator: MatPaginator,
    private sort: MatSort
  ) {
    super();
  }

  connect(): Observable<ReadingType[]> {
    const displayDataChanges = [
      this.readingTypeService.dataChange,
      this.filterChange,
      this.sort.sortChange,
      this.paginator.page
    ];

    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));

    return Observable.merge(...displayDataChanges)
      .startWith(null)
      .map(() => {
        this.isLoadingResults = true;

        // Filter data
        this.filteredData = this.readingTypeService.data
          .slice()
          .filter((item: ReadingType) => {
            const searchStr = (
              item.name +
              item.uom +
              item.dataType
            ).toLowerCase();
            return searchStr.indexOf(this.filter.toLowerCase()) !== -1;
          });

        // Sort filtered data
        const sortedData = this.sortData(this.filteredData.slice());

        // Grab the page's slice of the filtered sorted data.
        const startIndex = this.paginator.pageIndex * this.paginator.pageSize;
        this.renderedData = sortedData.splice(
          startIndex,
          this.paginator.pageSize
        );

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

  sortData(data: ReadingType[]): ReadingType[] {
    if (!this.sort.active || this.sort.direction === '') {
      return data;
    }

    return data.sort((a, b) => {
      let propertyA: number | string = '';
      let propertyB: number | string = '';

      switch (this.sort.active) {
        case 'name':
          [propertyA, propertyB] = [a.name, b.name];
          break;
        case 'uom':
          [propertyA, propertyB] = [a.uom, b.uom];
          break;
        case 'dataType':
          [propertyA, propertyB] = [a.dataType, b.dataType];
          break;
        case 'assembly':
          [propertyA, propertyB] = [a.assembly, b.assembly];
          break;
        case 'class':
          [propertyA, propertyB] = [a.class, b.class];
          break;
        case 'method':
          [propertyA, propertyB] = [a.method, b.method];
          break;
      }

      const valueA = isNaN(+propertyA) ? propertyA : +propertyA;
      const valueB = isNaN(+propertyB) ? propertyB : +propertyB;

      return (
        (valueA < valueB ? -1 : 1) * (this.sort.direction === 'asc' ? 1 : -1)
      );
    });
  }
}
