import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[appDetailsHost]'
})
export class DetailsHostDirective {
  constructor(public viewContainerRef: ViewContainerRef) { }
}
