import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';

import { Observable } from 'rxjs/Observable';

import { DetailsHostItem, DetailsHostService } from '../../shared/details-host/details-host.service';

import { EditorHostItem, ConfigurationDrawerService } from '../configuration-drawer.service';
import { Device, DeviceService } from '../../services/device.service';
import { DeviceEditorComponent } from './device-editor.component';

@Component({
  selector: 'app-device-list',
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class DeviceListComponent implements OnInit {
  devices$: Observable<Device[]>;

  constructor(private deviceService: DeviceService,
    private detailsService: DetailsHostService) { }

  ngOnInit() {
    this.devices$ = this.deviceService.getDevices();
  }

  openEditor(device: Device) {
    this.detailsService.openItem(new DetailsHostItem(DeviceEditorComponent, device === null ? new Device() : device));
  }
}
