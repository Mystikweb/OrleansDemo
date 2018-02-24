import { Injectable, Type } from '@angular/core';

import { Subject } from 'rxjs/Subject';

export interface IDetailsHostComponent {
  data: any;
}

export class DetailsHostItem {
  constructor(public component: Type<IDetailsHostComponent>, public data: any) { }
}

@Injectable()
export class DetailsHostService {
  detailsOpen$: Subject<boolean> = new Subject<boolean>();
  currentItem$: Subject<DetailsHostItem> = new Subject<DetailsHostItem>();

  constructor() {
    this.detailsOpen$.next(false);
   }

   openItem(item: DetailsHostItem) {
    this.detailsOpen$.next(true);
    this.currentItem$.next(item);
   }

   closeItem() {
     this.currentItem$.next(null);
     this.detailsOpen$.next(false);
   }
}
