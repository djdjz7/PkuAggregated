export interface SearchSourceInfo {
  name: string;
  url: string;
}

export interface SearchResult {
  results: SearchResultItem[];
  sourceInfo: SearchSourceInfo;
  isSuccess: boolean;
  errorMessage: string | null;
}

export interface SearchResultItem {
  title: string;
  description: string;
  url: string;
}
