import { ReleaseNotesResponseModel } from "../../project/models/release-notes/release-notes-response-model";

export interface BundleReleaseTimeRangesResponseModel {
  id: string;
  CreatedOnUtc: Date;
  version: string;
  startTimeUtc: string;
  endTimeUtc: string;
  releaseNote:  ReleaseNotesResponseModel[];
}
