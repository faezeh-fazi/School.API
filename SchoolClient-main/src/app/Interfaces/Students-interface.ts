import { IPaging } from './paging-interface';

export interface IAddStudent {
  userName: string;
  studentName: string;
  email: string;
  password: string;
  departmentId: number;
}
export interface IUpdateSrudent {
  name: string;
}

export interface IStudent {
  studentName: string;
  departmentName: string;
}
export interface IResponseStundet {
  departments: IStudent[];
}
