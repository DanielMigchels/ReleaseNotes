import { Component, Input, ViewChild } from '@angular/core';
import { GenericModalComponent } from '../../../components/generic-modal/generic-modal.component';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { NoteService } from '../../../services/note/note.service';
import { NgFor, NgIf } from '@angular/common';
import { CreateNoteRequestModel } from '../../../services/note/models/create-note-request-model';
import { NoteEntryType } from '../../../enums/note-entry-type';
import { NgIcon } from '@ng-icons/core';

@Component({
    selector: 'app-new-note',
    imports: [GenericModalComponent, ReactiveFormsModule, NgIf, NgFor, NgIcon],
    templateUrl: './new-note.component.html',
    styleUrl: './new-note.component.scss'
})
export class NewNoteComponent extends GenericModalComponent {
  @Input() releaseId: string = '';
  @ViewChild(GenericModalComponent) modal!: GenericModalComponent;

  formGroup = new FormGroup({
    text: new FormControl('', [Validators.required]),
    type: new FormControl(NoteEntryType.Bugfix, [Validators.required]),
    url: new FormControl('')
  });

  noteEntryTypes = [
    { value: NoteEntryType.Critical, label: 'Critical issue (!)' },
    { value: NoteEntryType.NewFeature, label: 'New functionality (+)' },
    { value: NoteEntryType.Bugfix, label: 'Task or bugfix (•)' },
    { value: NoteEntryType.Removal, label: 'Functionality removed (-)' }
  ];

  constructor(private noteService: NoteService) {
    super();
  }

  createNote() {
    if (!this.formGroup.valid) {
      this.formGroup.markAllAsTouched();
      return;
    }

    this.noteService.createNote(this.releaseId, this.formGroup.value as CreateNoteRequestModel).subscribe({
      next: () => {
        this.modalClosed.emit();
        this.formGroup.reset();
        this.formGroup.get('type')?.setValue(NoteEntryType.Bugfix);
        this.modal.closeModal();
      }
    });
  }
}