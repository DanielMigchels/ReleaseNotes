import { ReleaseNoteReleaseResponseModel } from "./release-note-release-response-model";

export interface ReleaseNotesResponseModel {
  id: string;
  name: string;
  latestVersion: string;
  createdOnUtc: Date;
  createdBy: string;
  releases: ReleaseNoteReleaseResponseModel[];
}
