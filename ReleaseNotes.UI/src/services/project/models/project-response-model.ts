export interface ProjectResponseModel {
  id: string;
  name: string;
  latestVersion: string;
  createdOnUtc: Date;
  createdBy: string;
  latestVersionCreatedOnUtc: Date;
}
