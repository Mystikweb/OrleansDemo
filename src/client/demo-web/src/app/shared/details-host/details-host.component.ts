import { Component, ComponentFactoryResolver, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';

import { DetailsHostDirective } from './details-host.directive';
import { IDetailsHostComponent, DetailsHostService } from './details-host.service';

@Component({
  selector: 'app-details-host',
  templateUrl: './details-host.component.html',
  styleUrls: ['./details-host.component.css']
})
export class DetailsHostComponent implements OnInit {
  @ViewChild(DetailsHostDirective) appDetailsHost: DetailsHostDirective;

  constructor(private componentFactoryResolver: ComponentFactoryResolver,
    private detailsHost: DetailsHostService) { }

  ngOnInit() {
    this.detailsHost.currentItem$.subscribe(item => {
      const viewContainerRef = this.appDetailsHost.viewContainerRef;
      viewContainerRef.clear();

      if (item !== null) {
        const componentFactory = this.componentFactoryResolver.resolveComponentFactory(item.component);
        const componentRef = viewContainerRef.createComponent(componentFactory);
        (<IDetailsHostComponent>componentRef.instance).data = item.data;
      }
    });
  }

}
