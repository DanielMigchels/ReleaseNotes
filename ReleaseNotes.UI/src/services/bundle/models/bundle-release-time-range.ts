export interface BundleReleaseTimeRange {
  id: string;
  version: string;
  startTimeUtc: Date | null;
  endTimeUtc: Date | null;
}
