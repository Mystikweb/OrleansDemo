import { Injectable, Type } from '@angular/core';

import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';

export interface EditorHostComponent {
  data: any;
}

export class EditorHostItem {
  constructor(public component: Type<EditorHostComponent>, public data: any) { }
}

@Injectable()
export class ConfigurationDrawerService {
  drawerOpen$: Subject<boolean> = new Subject<boolean>();
  currentItem$: Subject<EditorHostItem> = new Subject<EditorHostItem>();

  constructor() {
    this.drawerOpen$.next(false);
   }

   openComponent(item: EditorHostItem) {
     this.drawerOpen$.next(true);
     this.currentItem$.next(item);
   }
}
