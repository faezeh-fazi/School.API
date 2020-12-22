import { IPaging } from './paging-interface';

export interface IDepartmentDelete {
  departmentId: number;
}

export interface IAddDepartment {
  name: string;
}
export interface IEditDepartment {
  name: string;
}

export interface IDepartment {
  name: string;
  departmentId: number;
}
export interface IResponseDepartment {
  departments: IDepartment[];
  pagingInfo: IPaging;
}
