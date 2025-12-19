import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectService } from '../../services/project/project.service';
import { ReleaseNotesResponseModel } from '../../services/project/models/release-notes/release-notes-response-model';
import { LoaderComponent } from "../../components/loader/loader.component";
import { DatePipe, NgClass, NgFor, NgIf } from '@angular/common';
import { ReleaseService } from '../../services/release/release.service';
import { NewReleaseComponent } from "./new-release/new-release.component";
import { NewNoteComponent } from "./new-note/new-note.component";
import { NoteEntryTypePipe } from '../../pipes/note-entry-type.pipe';
import { NgIcon } from '@ng-icons/core';
import { DeleteReleaseComponent } from "./delete-release/delete-release.component";
import { ReleaseNoteReleaseResponseModel } from '../../services/project/models/release-notes/release-note-release-response-model';
import { EditReleaseComponent } from './edit-release/edit-release.component';
import { ReleaseNotesReleaseNoteEntryResponseModel } from '../../services/project/models/release-notes/release-notes-release-note-entry-response-model';
import { EditNoteComponent } from "./edit-note/edit-note.component";
import { DeleteNoteComponent } from './delete-note/delete-note.component';
import { FormsModule } from '@angular/forms';
import { NoteService } from '../../services/note/note.service';

@Component({
    selector: 'app-release-notes',
    imports: [LoaderComponent, NgIf, NewReleaseComponent, NewNoteComponent, NoteEntryTypePipe, DatePipe, NgIcon, DeleteReleaseComponent, EditReleaseComponent, EditNoteComponent, DeleteNoteComponent, FormsModule, NgClass],
    templateUrl: './release-notes.component.html',
    styleUrl: './release-notes.component.scss'
})
export class ReleaseNotesComponent implements OnInit {
  projectId: string = '';
  project: ReleaseNotesResponseModel | undefined;

  selectedRelease: ReleaseNoteReleaseResponseModel | undefined;
  selectedNote: ReleaseNotesReleaseNoteEntryResponseModel | undefined;

  includeUnpublished = false;

  @ViewChild(EditReleaseComponent) editReleaseModal!: EditReleaseComponent;
  @ViewChild(DeleteReleaseComponent) deleteReleaseModal!: DeleteReleaseComponent;
  @ViewChild(EditNoteComponent) editNoteModal!: EditNoteComponent;
  @ViewChild(DeleteNoteComponent) deleteNoteModal!: DeleteNoteComponent;

  constructor(private route: ActivatedRoute, private projectService: ProjectService, private releaseService: ReleaseService, private noteService: NoteService, private router: Router) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.projectId = params.get('id') ?? '';
      this.getReleaseNotes();
    });
  }

  publish(releaseId: string) {
    this.releaseService.publishRelease(releaseId).subscribe({
      next: () => {
        this.getReleaseNotes();
      }
    });
  }

  unpublish(releaseId: string) {
    this.releaseService.unpublishRelease(releaseId).subscribe({
      next: () => {
        this.getReleaseNotes();
      }
    });

  }

  getReleaseNotes() {
    this.projectService.getReleaseNotes(this.projectId).subscribe({
      next: x => {
        this.project = x
      },
      error: () => {
        this.router.navigate(['/']);
      }
    });
  }

  navigateHome() {
    this.router.navigate(['/']);
  }

  scrollToVersion(version: string) {
    const element = document.getElementById(version);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth' });
    }
  }

  download() {
    this.projectService.downloadPdf(this.projectId, this.includeUnpublished).subscribe({
      next: blob => {
        // ??
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'ReleaseNotes_'+this.project?.name+'.pdf';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
      },
      error: () => {}
    
    });
  }

  preview() {
    this.projectService.downloadPdf(this.projectId, this.includeUnpublished).subscribe({
      next: blob => {
        const url = window.URL.createObjectURL(blob);
        window.open(url, '_blank');
      },
      error: () => {}
    });
  }

  editRelease(event: Event, release: ReleaseNoteReleaseResponseModel) { 
    event.stopPropagation();
    this.selectedRelease = this.project?.releases.find(x => x.id === release.id);
    this.editReleaseModal.modal.openModal(event);
  }

  deleteRelease(event: Event, release: ReleaseNoteReleaseResponseModel) {
    event.stopPropagation();
    this.selectedRelease = this.project?.releases.find(x => x.id === release.id);
    this.deleteReleaseModal.isChecked = false;
    this.deleteReleaseModal.modal.openModal(event);
  }

  editNote(event: Event, note: ReleaseNotesReleaseNoteEntryResponseModel) {
    event.stopPropagation();
    this.selectedNote = this.project?.releases.find(x => x.noteEntries.some(y => y.id === note.id))?.noteEntries.find(x => x.id === note.id);
    this.editNoteModal.modal.openModal(event);    
  }

  deleteNote(event: Event, note: ReleaseNotesReleaseNoteEntryResponseModel) {
    event.stopPropagation();
    this.selectedNote = this.project?.releases.find(x => x.noteEntries.some(y => y.id === note.id))?.noteEntries.find(x => x.id === note.id);
    this.deleteNoteModal.isChecked = false;
    this.deleteNoteModal.modal.openModal(event);
  }

  onDragStart(event: DragEvent, note: ReleaseNotesReleaseNoteEntryResponseModel) {
    if (note.releasePublishedOn !== null) {
      event.preventDefault();
      return;
    }
    event.dataTransfer?.setData('text/plain', JSON.stringify(note));
  }

  onDrop(event: DragEvent, release: ReleaseNoteReleaseResponseModel) {
    event.preventDefault();
    const note = JSON.parse(event.dataTransfer?.getData('text/plain') || '{}');
    this.moveNoteToRelease(note, release);
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
  }

  moveNoteToRelease(note: ReleaseNotesReleaseNoteEntryResponseModel, release: ReleaseNoteReleaseResponseModel) {
    if (note.releasePublishedOn !== null) {
      return;
    }
    
    if (release.publishedOn !== null) {
      return;
    }

    const currentRelease = this.project!.releases.find((r: any) => r.noteEntries.some((n: any) => n.id === note.id));
    if (currentRelease) {
      currentRelease.noteEntries = currentRelease.noteEntries.filter((n: any) => n.id !== note.id);
    }

    release.noteEntries.push(note);

    this.noteService.moveNote(note.id, release.id).subscribe({
      next: () => {
        this.getReleaseNotes();
      }
    })
  }
}
