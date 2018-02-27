import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { environment } from '../../environments/environment';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

export class DeviceConfig {
  deviceId: string;
  name: string;
  sensors: Array<DeveiceSensorConfig>;
  events: Array<DeviceEventConfig>;

  constructor() {
    this.sensors = new Array<DeveiceSensorConfig>();
    this.events = new Array<DeviceEventConfig>();
  }
}

export class DeveiceSensorConfig {
  deviceSensorId: number;
  sensorId: number;
  name: string;
  isEnabled: boolean;
}

export class DeviceEventConfig {
  deviceEventTypeId: number;
  eventTypeId: number;
  name: string;
  isEnabled: boolean;
}

@Injectable()
export class DeviceService {

  private deviceUri = environment.rootUri + '/api/device';

  constructor(private http: HttpClient) { }

  getDeviceList(): Observable<DeviceConfig[]> {
    return this.http.get<DeviceConfig[]>(this.deviceUri);
  }
}
