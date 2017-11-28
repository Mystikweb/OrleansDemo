import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';

import { environment } from '../../environments/environment';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

export class Device {
  id: string;
  name: string;
  deviceTypeId: number;
  deviceType: string;
  enabled: boolean;
  runOnStartup: boolean;
  createdAt: Date;
  createdBy: string;
  updatedAt: Date;
  updatedBy: string;
  readings: Reading[];

  constructor() {
    this.readings = new Array<Reading>();
  }
}

export class Reading {
    id: string;
    readingTypeId: number;
    readingType: string;
    readingUom: string;
    readingDataType: string;
    enabled: boolean;
}

@Injectable()
export class DeviceService {

  private deviceUrl = environment.rootUri + '/api/device';

  constructor(private http: HttpClient) { }

  getDevices(): Observable<Device[]> {
    return this.http.get<Device[]>(this.deviceUrl);
  }

  get(id: string): Observable<Device> {
    const requestUrl = `${this.deviceUrl}/${id}`;
    return this.http.get<Device>(requestUrl);
  }

  save(device: Device): Observable<Device> {
    return this.http.post<Device>(this.deviceUrl, device, httpOptions);
  }

  update(device: Device) {
    const requestUrl = `${this.deviceUrl}/${device.id}`;
    return this.http.put(requestUrl, device, httpOptions);
  }

  delete(id: string): Observable<Device> {
    const requestUrl = `${this.deviceUrl}/${id}`;
    return this.http.delete<Device>(requestUrl, httpOptions);
  }
}
