import { Component, ComponentFactoryResolver, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';

import { DetailsHostDirective } from './details-host.directive';
import { DetailsItemComponent, DetailsHostService } from './details-host.service';

@Component({
  selector: 'app-details-host',
  templateUrl: './details-host.component.html',
  styleUrls: ['./details-host.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class DetailsHostComponent implements OnInit {
  @ViewChild(DetailsHostDirective) appDetailsHost: DetailsHostDirective;

  constructor(private componentFactoryResolver: ComponentFactoryResolver,
    private detailsHost: DetailsHostService) { }

  ngOnInit() {
    this.detailsHost.currentItem$.subscribe(item => {
      const componentFactory = this.componentFactoryResolver.resolveComponentFactory(item.component);
      const viewContainerRef = this.appDetailsHost.viewContainerRef;
      viewContainerRef.clear();

      const componentRef = viewContainerRef.createComponent(componentFactory);
      (<DetailsItemComponent>componentRef.instance).data = item.data;
    });
  }

}
