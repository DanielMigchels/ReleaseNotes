import { Component, OnInit, ViewChild } from '@angular/core';
import { CreateBundleComponent } from './create-bundle/create-bundle.component';
import { NgIconComponent } from '@ng-icons/core';
import { BundleResponseModel } from '../../services/bundle/models/bundle-response-model';
import { PaginatedList } from '../../services/pagination/paginated-list';
import { Router } from '@angular/router';
import { BundleService } from '../../services/bundle/bundle.service';
import { NgIf } from '@angular/common';
import { LoaderComponent } from '../../components/loader/loader.component';
import { EditBundleComponent } from './edit-bundle/edit-bundle.component';
import { DeleteBundleComponent } from './delete-bundle/delete-bundle.component';

@Component({
  selector: 'app-bundles',
  imports: [CreateBundleComponent, NgIconComponent, NgIf, LoaderComponent, EditBundleComponent, DeleteBundleComponent],
  templateUrl: './bundles.component.html',
  styleUrl: './bundles.component.scss'
})
export class BundlesComponent implements OnInit {
  bundles: PaginatedList<BundleResponseModel> | undefined;

  selectedBundle: BundleResponseModel | undefined;

  constructor(private bundleService: BundleService, private router: Router) { }

  pageSize = 2147483647;
  page = 0;

  @ViewChild(DeleteBundleComponent) deleteBundleModal!: DeleteBundleComponent;
  @ViewChild(EditBundleComponent) editBundleModal!: EditBundleComponent;
  
  ngOnInit(): void {
    this.fetchData();
  }

  fetchData() {
    this.bundles = undefined;
    this.bundleService.getBundles(this.pageSize, this.page).subscribe({
      next: x => this.bundles = x
    });
  }

  openBundle(bundleId: string) {
    this.router.navigate([`/bundles/${bundleId}`]);
  }

  editBundle(event: Event, bundle: BundleResponseModel) {
    event.stopPropagation();
    this.selectedBundle = bundle;
    this.editBundleModal.modal.openModal(event);
  }

  deleteBundle(event: Event, bundle: BundleResponseModel) {
    event.stopPropagation();
    this.selectedBundle = bundle;
    this.deleteBundleModal.isChecked = false;
    this.deleteBundleModal.modal.openModal(event);
  }

  previousBundle() {
    this.page--;
    this.fetchData();
  }

  nextBundle() {
    this.page++;
    this.fetchData();
  }
}
