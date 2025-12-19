import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { GenericModalComponent } from '../../../../components/generic-modal/generic-modal.component';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoaderComponent } from '../../../../components/loader/loader.component';
import { NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { BundleService } from '../../../../services/bundle/bundle.service';
import { BundleResponseModel } from '../../../../services/bundle/models/bundle-response-model';
import { CreateReleaseBundleRequestModel } from '../../../../services/bundle/models/create-release-bundle-request-model';

@Component({
  selector: 'app-create-release',
  imports: [GenericModalComponent, ReactiveFormsModule, LoaderComponent, NgIf],
  templateUrl: './create-release.component.html',
  styleUrl: './create-release.component.scss'
})
export class CreateReleaseComponent extends GenericModalComponent implements OnInit {

  private _bundle: BundleResponseModel | undefined;

  @Input() 
  set bundle(value: BundleResponseModel | undefined) {
    this._bundle = value;
  }
  
  get bundle(): BundleResponseModel | undefined {
    return this._bundle;
  }

  @ViewChild(GenericModalComponent) modal!: GenericModalComponent;

  isSubmitting = false;

  constructor(private bundleService: BundleService, private router: Router) { super(); }

  formGroup = new FormGroup({
    version: new FormControl('', [Validators.required]),
    startTimeUtc: new FormControl(),
    endTimeUtc: new FormControl(),
  });

  ngOnInit(): void {

  }

  createReleaseBundle() {
    if (!this.formGroup.valid) {
      this.formGroup.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    this.bundleService.createReleaseBundle(this.bundle!.id, this.formGroup.value as CreateReleaseBundleRequestModel).subscribe({
      next: () => {
        this.modalClosed.emit();
        this.formGroup.reset();
        this.modal.closeModal();
        this.isSubmitting = false;
      },
      error: () => {
        this.isSubmitting = false;
      }
    });
  }
}
