import { Component, Input, ViewChild } from '@angular/core';
import { GenericModalComponent } from '../../../components/generic-modal/generic-modal.component';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { BundleService } from '../../../services/bundle/bundle.service';
import { BundleResponseModel } from '../../../services/bundle/models/bundle-response-model';
import { EditBundleRequestModel } from '../../../services/bundle/models/edit-bundle-request-model';
import { NgIf } from '@angular/common';
import { LoaderComponent } from '../../../components/loader/loader.component';

@Component({
  selector: 'app-edit-bundle',
  imports: [GenericModalComponent, NgIf, ReactiveFormsModule, LoaderComponent],
  templateUrl: './edit-bundle.component.html',
  styleUrl: './edit-bundle.component.scss'
})
export class EditBundleComponent extends GenericModalComponent {

  private _bundle: BundleResponseModel | undefined;

  @Input() 
  set bundle(value: BundleResponseModel | undefined) {
    this._bundle = value;
    if (this._bundle) {
      this.formGroup.patchValue({
        name: this._bundle.name,
      });
    }
  }
  
  get bundle(): BundleResponseModel | undefined {
    return this._bundle;
  }
  
  @ViewChild(GenericModalComponent) modal!: GenericModalComponent;
  isSubmitting: boolean = false;

  formGroup = new FormGroup({
    name: new FormControl('', [Validators.required]),
  });

  constructor(private bundleService: BundleService) {
    super();
  }

  editBundle() {
    if (!this.formGroup.valid) {
      this.formGroup.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    this.bundleService.editBundle(this.bundle!.id, this.formGroup.value as EditBundleRequestModel).subscribe({
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
