import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { DetailsItemComponent, DetailsHostService } from '../../shared/details-host/details-host.service';
import { ReadingType, ReadingTypeService } from '../../services/reading-type.service';

@Component({
  selector: 'app-reading-type-editor',
  templateUrl: './reading-type-editor.component.html',
  styleUrls: ['./reading-type-editor.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ReadingTypeEditorComponent implements OnInit, DetailsItemComponent {
  data: any;
  readingType: ReadingType;

  constructor(private detailsHost: DetailsHostService,
    private readingTypeService: ReadingTypeService) { }

  ngOnInit() {
    this.readingType = this.data as ReadingType;
  }

  save() {
    if (this.readingType.id === undefined) {
      this.readingTypeService.save(this.readingType).subscribe(result => {
        this.detailsHost.closeItem();
      });
    } else {
      this.readingTypeService.update(this.readingType).subscribe(result => {
        this.detailsHost.closeItem();
      });
    }
  }

  cancel() {
    this.detailsHost.closeItem();
  }
}
