import { Pipe, PipeTransform } from '@angular/core';
import { NoteEntryType } from '../enums/note-entry-type';

@Pipe({
  name: 'noteEntryType',
  standalone: true
})
export class NoteEntryTypePipe implements PipeTransform {

  transform(value: NoteEntryType): string {
    switch (value) {
      case NoteEntryType.Critical:
        return '!';
      case NoteEntryType.NewFeature:
        return '+';
      case NoteEntryType.Bugfix:
        return '•';
      case NoteEntryType.Removal:
        return '-';
      default:
        return '•';
    }
  }

}