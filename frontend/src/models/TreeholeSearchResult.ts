import { SearchSourceInfo } from "./SearchResult";

export interface TreeholeSearchResult {
  results: TreeholeSearchResultItem[];
  sourceInfo: SearchSourceInfo;
  isSuccess: boolean;
  errorMessage: string | null;
}

export interface TreeholeSearchResultItem {
  pid: number;
  text: string;
  time: string;
  imageId: number | null;
  commentCount: number;
  starCount: number;
}

export interface TreeholeComment {
  cid: number;
  pid: number;
  text: string;
  timestamp: string;
  tag: any;
  comment_id: any;
  name: string;
  quote: TreeholeCommentQuoteData | null;
}

export interface TreeholeCommentQuoteData {
  pid: number;
  name_tag: string;
  text: string;
}