import { Component, Input, ViewChild } from '@angular/core';
import { GenericModalComponent } from '../../../components/generic-modal/generic-modal.component';
import { NoteService } from '../../../services/note/note.service';
import { ReleaseNotesReleaseNoteEntryResponseModel } from '../../../services/project/models/release-notes/release-notes-release-note-entry-response-model';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-delete-note',
    imports: [GenericModalComponent, NgIf, FormsModule],
    templateUrl: './delete-note.component.html',
    styleUrl: './delete-note.component.scss'
})
export class DeleteNoteComponent extends GenericModalComponent {
  @Input() note: ReleaseNotesReleaseNoteEntryResponseModel | undefined;
  @ViewChild(GenericModalComponent) modal!: GenericModalComponent;
  isChecked: boolean = false;

  constructor(private noteService: NoteService) {
    super();
  }

  deleteNote() {
    this.noteService.deleteNote(this.note!.id).subscribe({
      next: () => {
        this.modalClosed.emit();
        this.modal.closeModal();
      }
    });
  }
}
