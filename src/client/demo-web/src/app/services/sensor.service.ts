import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable,  BehaviorSubject } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

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

  getSensorList(sort: string, direction: string, index: number, filter: string): Observable<SensorConfig[]> {
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
