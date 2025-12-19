import { Component, Input, ViewChild } from '@angular/core';
import { GenericModalComponent } from "../../../components/generic-modal/generic-modal.component";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ReleaseService } from '../../../services/release/release.service';
import { CreateReleaseRequestModel } from '../../../services/release/models/create-release-request-model';
import { NgIf } from '@angular/common';
import { ReleaseNotesResponseModel } from '../../../services/project/models/release-notes/release-notes-response-model';

@Component({
    selector: 'app-new-release',
    imports: [GenericModalComponent, ReactiveFormsModule, NgIf],
    templateUrl: './new-release.component.html',
    styleUrl: './new-release.component.scss'
})
export class NewReleaseComponent extends GenericModalComponent {
  @Input() project: ReleaseNotesResponseModel | undefined;
  @ViewChild(GenericModalComponent) modal!: GenericModalComponent;

  formGroup = new FormGroup({
    version: new FormControl('', [Validators.required]),
  });

  constructor(private releaseService: ReleaseService) { super(); }

  createRelease() {
    if (!this.formGroup.valid) {
      this.formGroup.markAllAsTouched();
      return;
    }
    this.releaseService.createRelease(this.project!.id, this.formGroup.value as CreateReleaseRequestModel).subscribe({
      next:() => {
        this.modalClosed.emit();
        this.formGroup.reset();
        this.modal.closeModal();
      }
    });
  }
}
