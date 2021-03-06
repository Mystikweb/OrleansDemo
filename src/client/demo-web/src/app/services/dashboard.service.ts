import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

export class DeviceStatus {
  deviceId: string;
  name: string;
  isRunning: boolean;
}

export class DeviceSummary {
  deviceId: string;
  name: string;
  isEnabled: boolean;
  sensorSummaries: Array<SensorSummary>;

  constructor() {
    this.sensorSummaries = new Array<SensorSummary>();
  }
}

export class SensorSummary {
  name: string;
  average: number;
  uom: string;
}

@Injectable()
export class DashboardService {

  private dashboardUri = environment.rootUri + '/api/dashboard';
  private runtimeUri = environment.rootUri + '/api/runtime';

  constructor(private http: HttpClient) { }

  getDashboardSummary(): Observable<DeviceSummary[]> {
    return this.http.get<DeviceSummary[]>(this.dashboardUri);
  }

  getRuntimeList(): Observable<DeviceStatus[]> {
    return this.http.get<DeviceStatus[]>(this.runtimeUri);
  }

  startDevice(device: DeviceStatus) {
    const requestUri = `${this.runtimeUri}/start`;
    return this.http.post(requestUri, { deviceId: device.deviceId, isRunning: true }, httpOptions);
  }

  stopDevice(device: DeviceStatus) {
    const requestUri = `${this.runtimeUri}/stop`;
    return this.http.post(requestUri, { deviceId: device.deviceId, isRunning: false }, httpOptions);
  }
}
