import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { ReadingType, ReadingTypeService } from '../../services/reading-type.service';

import { ReadingTypeListComponent } from './reading-type-list.component';

@Component({
  selector: 'app-reading-type-dialog',
  templateUrl: './reading-type-dialog.component.html',
  styleUrls: ['./reading-type-dialog.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ReadingTypeDialogComponent implements OnInit {
  readingType: ReadingType;

  constructor(private readingTypeService: ReadingTypeService,
    private dialogRef: MatDialogRef<ReadingTypeListComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ReadingType) { }

  ngOnInit() {
    this.readingType = this.data === null ? { id: null, name: null, uom: null, dataType: null } : this.data;
  }

  save() {
    this.readingTypeService.save(this.readingType)
      .subscribe(result => {
          this.dialogRef.close(result);
      });
  }
}
