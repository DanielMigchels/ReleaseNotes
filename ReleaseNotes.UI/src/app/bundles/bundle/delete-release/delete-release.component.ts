import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { GenericModalComponent } from '../../../../components/generic-modal/generic-modal.component';
import { NgIconComponent } from '@ng-icons/core';
import { BundleReleaseTimeRangesResponseModel } from '../../../../services/bundle/models/bundle-release-time-ranges-response-model';
import { ReleaseService } from '../../../../services/release/release.service';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BundleService } from '../../../../services/bundle/bundle.service';

@Component({
  selector: 'app-delete-release',
  imports: [GenericModalComponent, NgIconComponent, NgIf, FormsModule],
  templateUrl: './delete-release.component.html',
  styleUrl: './delete-release.component.scss'
})
export class DeleteReleaseComponent extends GenericModalComponent implements OnInit {

  private _release: BundleReleaseTimeRangesResponseModel | undefined;

  @Input()
  set release(value: BundleReleaseTimeRangesResponseModel | undefined) {
    this._release = value;
  }

  get release(): BundleReleaseTimeRangesResponseModel | undefined {
    return this._release;
  }
  
  @ViewChild(GenericModalComponent) modal!: GenericModalComponent;

  ngOnInit(): void {

  }

  isChecked: boolean = false;

  constructor(private bundleService: BundleService) {
    super();
  }

  deleteRelease() {
    this.bundleService.deleteRelease(this.release!.id).subscribe({
      next: () => {
        this.modalClosed.emit();
        this.isChecked = false;
        this.modal.closeModal();
      }
    });
  }
}
