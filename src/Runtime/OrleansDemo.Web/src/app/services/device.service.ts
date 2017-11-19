import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { DataSource, SelectionModel } from '@angular/cdk/collections';
import { MatPaginator, MatSort } from '@angular/material';

import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import 'rxjs/add/observable/merge';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/operator/switchMap';

import { environment } from '../../environments/environment';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

export interface Device {
  Id: string;
  Name: string;
  DeviceTypeId: number;
  Enabled: boolean;
  RunOnStartup: boolean;
  CreatedAt: Date;
  CreatedBy: string;
  UpdatedAt: Date;
  UpdatedBy: string;
}

@Injectable()
export class DeviceService {

  private deviceUrl = environment.rootUri + '/api/device';

  constructor(private http: HttpClient) { }

  getDevices(sort: string, order: string, page: number): Observable<Device[]> {
    const requestUrl = `${this.deviceUrl}?sort=${sort}&order=${order}&page=${page + 1}`;
    return this.http.get<Device[]>(requestUrl);
  }

}

export class DeviceDataSource extends DataSource<Device> {
  resultsLength = 0;
  isLoadingResults = false;
  isRateLimitReached = false;

  constructor(private deviceApi: DeviceService,
              private paginator: MatPaginator,
              private sort: MatSort) {
    super();
  }

  connect(): Observable<Device[]> {
    const displayDataChanges = [
      this.sort.sortChange,
      this.paginator.page
    ];

    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    return Observable.merge(...displayDataChanges)
      .startWith(null)
      .switchMap(() => {
        this.isLoadingResults = true;
        return this.deviceApi.getDevices(this.sort.active,
          this.sort.direction,
          this.paginator.pageIndex);
      })
      .map(data => {
        this.isLoadingResults = false;
        this.isRateLimitReached = false;
        this.resultsLength = data.length;

        return data;
      })
      .catch(() => {
        this.isLoadingResults = false;
        this.isRateLimitReached = true;
        return Observable.of([]);
      });
  }

  disconnect() {}
}
