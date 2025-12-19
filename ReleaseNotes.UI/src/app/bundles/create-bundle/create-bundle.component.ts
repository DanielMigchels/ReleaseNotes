import { Component, OnInit, ViewChild } from '@angular/core';
import { GenericModalComponent } from '../../../components/generic-modal/generic-modal.component';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { LoaderComponent } from '../../../components/loader/loader.component';
import { NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { BundleService } from '../../../services/bundle/bundle.service';
import { CreateBundleRequestModel } from '../../../services/bundle/models/create-bundle-request-model';

@Component({
  selector: 'app-create-bundle',
  imports: [GenericModalComponent, ReactiveFormsModule, LoaderComponent, NgIf],
  templateUrl: './create-bundle.component.html',
  styleUrl: './create-bundle.component.scss'
})
export class CreateBundleComponent extends GenericModalComponent implements OnInit {

  @ViewChild(GenericModalComponent) modal!: GenericModalComponent;

  isSubmitting = false;

  formGroup = new FormGroup({
    name: new FormControl('', [Validators.required]),
  });

  constructor(private bundleService: BundleService, private router: Router) { super(); }

  ngOnInit(): void {

  }

  createBundle() {
    if (!this.formGroup.valid) {
      this.formGroup.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    this.bundleService.addBundle(this.formGroup.value as CreateBundleRequestModel).subscribe({
      next: x => {
        this.modalClosed.emit();
        this.formGroup.reset();
        this.modal.closeModal();
        this.router.navigate([`/bundles/${x.id}`]);
        this.isSubmitting = false;
      },
      error: () => {
        this.isSubmitting = false;
      }
    });
  }
}
