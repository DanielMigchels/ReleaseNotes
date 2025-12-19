import { Injectable } from '@angular/core';
import { BundleResponseModel } from './models/bundle-response-model';
import { CreateBundleRequestModel } from './models/create-bundle-request-model';
import { CreateBundleResponseModel } from './models/create-bundle-response-model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedList } from '../pagination/paginated-list';
import { EditBundleRequestModel } from './models/edit-bundle-request-model';
import { AddProjectsToBundleRequestModel } from './models/add-projects-to-bundle-request-model';
import { CreateReleaseBundleRequestModel } from './models/create-release-bundle-request-model';

@Injectable({
  providedIn: 'root'
})
export class BundleService {
   private apiUrl = '/api/bundle';

  constructor(private http: HttpClient) { }

  getBundles(pageSize = 2147483647, page = 0): Observable<PaginatedList<BundleResponseModel>> {
    return this.http.get<PaginatedList<BundleResponseModel>>(`${this.apiUrl}?pageSize=${pageSize}&page=${page}`);
  }

  getBundleDetails(bundleId: string): Observable<BundleResponseModel> {
    return this.http.get<BundleResponseModel>(`${this.apiUrl}/${bundleId}`);
  }

  addBundle(createBundleRequestModel: CreateBundleRequestModel): Observable<CreateBundleResponseModel> {
    return this.http.post<CreateBundleResponseModel>(`${this.apiUrl}`, createBundleRequestModel);
  }

  editBundle(bundleId: string, editBundleRequestModel: EditBundleRequestModel) {
    return this.http.put(`${this.apiUrl}/${bundleId}`, editBundleRequestModel);
  }

  deleteBundle(bundleId: string) {
    return this.http.delete(`${this.apiUrl}/${bundleId}`);
  }

  addProjectsToBundle(bundleId: string, requestModel: AddProjectsToBundleRequestModel) {    
    return this.http.put(`${this.apiUrl}/${bundleId}/projects`, requestModel);
  }

  createReleaseBundle(id: string, requestModel: CreateReleaseBundleRequestModel) {
    return this.http.post(`${this.apiUrl}/${id}/release`, requestModel);
  }

 updateReleaseBundle(id: string, requestModel: CreateReleaseBundleRequestModel) {
    return this.http.put(`${this.apiUrl}/release/${id}`, requestModel);
  }

  deleteRelease(id: string) {
    return this.http.delete(`${this.apiUrl}/release/${id}`);
  }

  downloadPdf(id: string): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/release/${id}/pdf`, { responseType: 'blob' });
  }
}
