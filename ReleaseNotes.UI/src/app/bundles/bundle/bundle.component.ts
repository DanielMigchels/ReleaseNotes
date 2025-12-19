import { DatePipe, NgIf, NgClass } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgIcon } from '@ng-icons/core';
import { LoaderComponent } from '../../../components/loader/loader.component';
import { BundleService } from '../../../services/bundle/bundle.service';
import { BundleResponseModel } from '../../../services/bundle/models/bundle-response-model';
import { SelectProjectsComponent } from './select-projects/select-projects.component';
import { CreateReleaseComponent } from './create-release/create-release.component';
import { BundleReleaseTimeRangesResponseModel } from '../../../services/bundle/models/bundle-release-time-ranges-response-model';
import { BrasserTimePipe } from '../../../pipes/brasser-time.pipe';
import { NoteEntryTypePipe } from '../../../pipes/note-entry-type.pipe';
import { EditReleaseComponent } from './edit-release/edit-release.component';
import { DeleteReleaseComponent } from './delete-release/delete-release.component';

@Component({
  selector: 'app-bundle',
  imports: [LoaderComponent, NgIf, NgIcon, FormsModule, SelectProjectsComponent, CreateReleaseComponent, NgClass, BrasserTimePipe, NoteEntryTypePipe, DatePipe, EditReleaseComponent, DeleteReleaseComponent],
  templateUrl: './bundle.component.html',
  styleUrl: './bundle.component.scss'
})
export class BundleComponent {

  bundleId: string = '';
  bundle: BundleResponseModel | undefined;

  selectedRelease: BundleReleaseTimeRangesResponseModel | undefined;

  constructor(private route: ActivatedRoute, private bundleService: BundleService, private router: Router) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.bundleId = params.get('id') ?? '';
      this.fetchData();
    });
  }

  fetchData() {
    this.bundleService.getBundleDetails(this.bundleId).subscribe({
      next: (response) => {
        this.bundle = response;
        this.selectRelease(this.bundle.releases[0]);
      }
    });
  }

  navigateHome() {
    this.router.navigate(['/bundles']);
  }

  selectRelease(bundleReleaseTimeRangesResponseModel: BundleReleaseTimeRangesResponseModel) {
    this.selectedRelease = bundleReleaseTimeRangesResponseModel;
  }

  download() {
    this.bundleService.downloadPdf(this.selectedRelease!.id).subscribe({
      next: blob => {
        // ??
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'ReleaseNotes_'+this.bundle?.name+'.pdf';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
      },
      error: () => {}
    
    });
  }

  preview() {
    this.bundleService.downloadPdf(this.selectedRelease!.id).subscribe({
      next: blob => {
        const url = window.URL.createObjectURL(blob);
        window.open(url, '_blank');
      },
      error: () => {}
    });
  }
}
