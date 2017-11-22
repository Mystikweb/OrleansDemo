import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { EditorHostComponent } from '../configuration-drawer.service';

@Component({
  selector: 'app-device-editor',
  templateUrl: './device-editor.component.html',
  styleUrls: ['./device-editor.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class DeviceEditorComponent implements OnInit, EditorHostComponent {
  data: any;

  constructor() { }

  ngOnInit() {
  }

}
