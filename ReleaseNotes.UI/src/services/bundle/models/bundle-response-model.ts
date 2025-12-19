import { ReleaseNotesResponseModel } from "../../project/models/release-notes/release-notes-response-model";
import { BundleReleaseTimeRangesResponseModel } from "./bundle-release-time-ranges-response-model";

export interface BundleResponseModel {
  id: string;
  name: string;
  CreatedOnUtc: Date;
  createdBy: string;
  projects: ReleaseNotesResponseModel[];
  releases: BundleReleaseTimeRangesResponseModel[];
}
