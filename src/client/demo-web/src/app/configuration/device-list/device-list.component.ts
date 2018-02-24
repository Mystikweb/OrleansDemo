import { AfterViewInit, Component, OnInit } from '@angular/core';

import { Observable } from 'rxjs/Observable';

import { DetailsHostItem, DetailsHostService } from '../../shared/details-host/details-host.service';
import { DeviceEditorComponent } from './device-editor.component';

import { DeviceConfig, DeviceService } from '../../services/device.service';

@Component({
  selector: 'app-device-list',
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.css']
})
export class DeviceListComponent implements OnInit, AfterViewInit {
  devices$: Observable<DeviceConfig[]>;

  constructor(private deviceService: DeviceService,
    private detailsService: DetailsHostService) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.loadData();
  }
  openEditor(device: DeviceConfig) {
    this.detailsService.openItem(new DetailsHostItem(DeviceEditorComponent, device === null ? new DeviceConfig() : device));
  }

  private loadData() {
    this.devices$ = this.deviceService.getDeviceList();
  }
}
