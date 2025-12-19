import { Component, Input, ViewChild } from '@angular/core';
import { GenericModalComponent } from '../../../components/generic-modal/generic-modal.component';
import { ReleaseService } from '../../../services/release/release.service';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReleaseNoteReleaseResponseModel } from '../../../services/project/models/release-notes/release-note-release-response-model';

@Component({
    selector: 'app-delete-release',
    imports: [GenericModalComponent, NgIf, FormsModule],
    templateUrl: './delete-release.component.html',
    styleUrl: './delete-release.component.scss'
})
export class DeleteReleaseComponent extends GenericModalComponent {
  @Input() release: ReleaseNoteReleaseResponseModel | undefined;
  @ViewChild(GenericModalComponent) modal!: GenericModalComponent;
  isChecked: boolean = false;

  constructor(private releaseService: ReleaseService) {
    super();
  }

  deleteRelease() {
    this.releaseService.deleteRelease(this.release!.id).subscribe({
      next: () => {
        this.modalClosed.emit();
        this.isChecked = false;
        this.modal.closeModal();
      }
    });
  }
}
