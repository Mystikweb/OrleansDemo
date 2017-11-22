import { Component, ComponentFactoryResolver, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';

import { EditorHostDirective } from './editor-host.directive';
import { ConfigurationDrawerService, EditorHostComponent } from './configuration-drawer.service';

@Component({
  selector: 'app-editor-shell',
  templateUrl: './editor-shell.component.html',
  styleUrls: ['./editor-shell.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class EditorShellComponent implements OnInit {
  @ViewChild(EditorHostDirective) appEditorHost: EditorHostDirective;

  constructor(private drawerService: ConfigurationDrawerService,
    private componentFactoryResolver: ComponentFactoryResolver) { }

  ngOnInit() {
    this.drawerService.currentItem$.subscribe(item => {
      const componentFactory = this.componentFactoryResolver.resolveComponentFactory(item.component);
      const viewContainerRef = this.appEditorHost.viewContainerRef;
      viewContainerRef.clear();

      const componentRef = viewContainerRef.createComponent(componentFactory);
      (<EditorHostComponent>componentRef.instance).data = item.data;
    });
  }

}
