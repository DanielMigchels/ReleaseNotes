import { Component, Input, ViewChild } from '@angular/core';
import { GenericModalComponent } from '../../../components/generic-modal/generic-modal.component';
import { BundleService } from '../../../services/bundle/bundle.service';
import { BundleResponseModel } from '../../../services/bundle/models/bundle-response-model';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-delete-bundle',
  imports: [GenericModalComponent, NgIf, FormsModule],
  templateUrl: './delete-bundle.component.html',
  styleUrl: './delete-bundle.component.scss'
})
export class DeleteBundleComponent extends GenericModalComponent  {
  
  @Input() bundle: BundleResponseModel | undefined;
  @ViewChild(GenericModalComponent) modal!: GenericModalComponent;
  isChecked: boolean = false;

  constructor(private bundleService: BundleService) {
    super();
  }

  deleteBundle() {
    this.bundleService.deleteBundle(this.bundle!.id).subscribe({
      next: () => {
        this.modalClosed.emit();
        this.modal.closeModal();
      }
    });
  }
}
