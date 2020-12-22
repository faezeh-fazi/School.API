export interface IPaging {
  totalCount: number;
  pageSize: number;
  totalPages: number;
  currentPages: number;
  prevLink: string;
  nextLink: string;
}
