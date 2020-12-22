import { IPaging } from './paging-interface';

export interface ICourseDelete {
  courseId: number;
}

export interface IAddCourse {
  courseName: string;
  courseDescription: string;
  courseCredit: number;
  startDate: string;
  endDate: string;
  startTime: string;
  endTime: string;
  day: number;
  departmentId: number;
}
export interface IAddCourseToTimeTable {
  courseId: number;
  startTime: string;
  endTime: string;
  day: number;
}

export interface ICourse {
  courseName: string;
  courseDescription: string;
  startDate: string;
  endDate: string;
  department: string;
}
export interface IResponseCourse {
  courses: Array<ICourse>;
  pagingInfo: IPaging;
}

export interface IUpdateCourse {
  courseName: string;
  courseDescription: string;
  startDate: string;
  endDate: string;
}

export interface IEditTimeTable {
  TimeTableId: number;
  startTime: string;
  endTime: string;
  day: number;
}
export interface IDeleteTimeTable {
  TimeTableId: number;
}
