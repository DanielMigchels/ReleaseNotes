import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { GenericModalComponent } from '../../../../components/generic-modal/generic-modal.component';
import { NgIconComponent } from '@ng-icons/core';
import { BundleReleaseTimeRangesResponseModel } from '../../../../services/bundle/models/bundle-release-time-ranges-response-model';
import { NgIf } from '@angular/common';
import { LoaderComponent } from '../../../../components/loader/loader.component';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BundleService } from '../../../../services/bundle/bundle.service';
import { CreateReleaseBundleRequestModel } from '../../../../services/bundle/models/create-release-bundle-request-model';

@Component({
  selector: 'app-edit-release',
  imports: [GenericModalComponent, NgIconComponent, NgIf, LoaderComponent, ReactiveFormsModule],
  templateUrl: './edit-release.component.html',
  styleUrl: './edit-release.component.scss'
})
export class EditReleaseComponent extends GenericModalComponent implements OnInit {

  isSubmitting: boolean = false;

  private _release: BundleReleaseTimeRangesResponseModel | undefined;

  formGroup = new FormGroup({
    version: new FormControl('', [Validators.required]),
    startTimeUtc: new FormControl(),
    endTimeUtc: new FormControl(),
  });

  @Input()
  set release(value: BundleReleaseTimeRangesResponseModel | undefined) {
    this._release = value;

    if (this._release) {
      const formatDateForInput = (isoString: string | undefined) => isoString ? isoString.split('T')[0] : null;
      this.formGroup.patchValue({
        version: this._release.version,
        startTimeUtc: formatDateForInput(this._release.startTimeUtc),
        endTimeUtc: formatDateForInput(this._release.endTimeUtc)
      });
    }
  }

  get release(): BundleReleaseTimeRangesResponseModel | undefined {
    return this._release;
  }

  @ViewChild(GenericModalComponent) modal!: GenericModalComponent;

  constructor(private bundleService: BundleService) {
    super();
  }

  ngOnInit(): void {

  }

  updateReleaseBundle() {
    if (!this.formGroup.valid) {
      this.formGroup.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    this.bundleService.updateReleaseBundle(this.release!.id, this.formGroup.value as CreateReleaseBundleRequestModel).subscribe({
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
