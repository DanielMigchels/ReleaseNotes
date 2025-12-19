import { Component, Input, ViewChild } from '@angular/core';
import { GenericModalComponent } from '../../../components/generic-modal/generic-modal.component';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { NoteEntryType } from '../../../enums/note-entry-type';
import { CreateNoteRequestModel } from '../../../services/note/models/create-note-request-model';
import { NoteService } from '../../../services/note/note.service';
import { NgIf, NgFor } from '@angular/common';
import { ReleaseNotesReleaseNoteEntryResponseModel } from '../../../services/project/models/release-notes/release-notes-release-note-entry-response-model';

@Component({
    selector: 'app-edit-note',
    imports: [GenericModalComponent, ReactiveFormsModule, NgIf, NgFor],
    templateUrl: './edit-note.component.html',
    styleUrl: './edit-note.component.scss'
})
export class EditNoteComponent extends GenericModalComponent {
  private _note: ReleaseNotesReleaseNoteEntryResponseModel | undefined;

  @Input() 
  set note(value: ReleaseNotesReleaseNoteEntryResponseModel | undefined) {
    this._note = value;
    if (this._note) {
      this.formGroup.patchValue({
        text: this._note.text,
        type: this._note.type,
        url: this._note.url
      });
    }
  }
  get note(): ReleaseNotesReleaseNoteEntryResponseModel | undefined {
    return this._note;
  }

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

    this.noteService.editNote(this.note!.id, this.formGroup.value as CreateNoteRequestModel).subscribe({
      next: () => {
        this.modalClosed.emit();
        this.formGroup.reset();
        this.formGroup.get('type')?.setValue(NoteEntryType.Bugfix);
        this.modal.closeModal();
      }
    });
  }
}