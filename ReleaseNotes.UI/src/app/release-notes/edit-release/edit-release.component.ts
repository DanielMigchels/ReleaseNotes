import { Component, Input, ViewChild } from '@angular/core';
import { GenericModalComponent } from '../../../components/generic-modal/generic-modal.component';
import { NgIf } from '@angular/common';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ReleaseNoteReleaseResponseModel } from '../../../services/project/models/release-notes/release-note-release-response-model';
import { ReleaseService } from '../../../services/release/release.service';
import { EditReleaseRequestModel } from '../../../services/release/edit-release-request-model';

@Component({
    selector: 'app-edit-release',
    imports: [GenericModalComponent, NgIf, FormsModule, ReactiveFormsModule],
    templateUrl: './edit-release.component.html',
    styleUrl: './edit-release.component.scss'
})
export class EditReleaseComponent extends GenericModalComponent {
  private _release: ReleaseNoteReleaseResponseModel | undefined;

  @Input() 
  set release(value: ReleaseNoteReleaseResponseModel | undefined) {
    this._release = value;
    if (this._release) {
      const createdOnUtc = new Date(this._release.createdOnUtc);
      this.formGroup.patchValue({
        version: this._release.version,
        createdOnUtc: createdOnUtc.toISOString().slice(0, 16)
      });
    }
  }
  get release(): ReleaseNoteReleaseResponseModel | undefined {
    return this._release;
  }

  @ViewChild(GenericModalComponent) modal!: GenericModalComponent;
  isSubmitting: boolean = false;

  formGroup = new FormGroup({
    version: new FormControl('', [Validators.required]),
    createdOnUtc: new FormControl('', [Validators.required]),
  });

  constructor(private releaseService: ReleaseService) {
    super();
  }
  
  createRelease() {
    if (!this.formGroup.valid) {
      this.formGroup.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    this.releaseService.editRelease(this.release!.id, this.formGroup.value as EditReleaseRequestModel).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.modalClosed.emit();
        this.modal.closeModal();
      },
      error: () => {
        this.isSubmitting = false;
      }
    });
  }
}