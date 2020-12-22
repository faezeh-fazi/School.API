import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, ElementRef, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import {
  IAddCourse,
  ICourse,
  IResponseCourse,
} from '../Interfaces/Course-interface';
import { IPaging } from '../Interfaces/paging-interface';
import { UserService } from '../user.service';

@Component({
  selector: 'app-admin-panel-courses',
  templateUrl: './admin-panel-courses.component.html',
  styleUrls: [
    './admin-panel-courses.component.css',
    '../admin-panel/sb-admin-2.min.css',
  ],
})
export class AdminPanelCoursesComponent implements OnInit {
  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private toastr: ToastrService
  ) {}

  base_url = 'https://localhost:5001/';
  pagingInfop: IPaging;
  courses: ICourse[];
  mode: boolean = false;
  addForm: FormGroup;
  idtoken: string;
  ngOnInit(): void {
    this.GetallDepartments();
    this.formAddInit();
    this.get_Authorize();
  }

  formAddInit() {
    this.addForm = new FormGroup({
      courseName: new FormControl('', [Validators.required]),
      courseDescription: new FormControl('', [Validators.required]),
      courseCredit: new FormControl('', [Validators.required]),
      day: new FormControl('', [Validators.required]),
      departmentId: new FormControl('', [Validators.required]),
      startDate: new FormControl('', [Validators.required]),
      endDate: new FormControl('', [Validators.required]),
      startTime: new FormControl('', [Validators.required]),
      endTime: new FormControl('', [Validators.required]),
    });
  }
  get_Authorize() {
    this.userService.currentToken$.subscribe((res) => {
      this.idtoken = res.token;
    });
  }

  add_item() {
    const temp: IAddCourse = {
      courseName: this.addForm.get('courseName').value,
      courseDescription: this.addForm.get('courseDescription').value,
      courseCredit: this.addForm.get('courseCredit').value,
      day: this.addForm.get('day').value,
      departmentId: this.addForm.get('departmentId').value,
      startDate: this.addForm.get('startDate').value,
      endDate: this.addForm.get('endDate').value,
      startTime: this.addForm.get('startTime').value,
      endTime: this.addForm.get('endTime').value,
    };
    temp.startTime = temp.startDate + 'T' + temp.startTime + ':42.346Z';
    temp.endTime = temp.endDate + 'T' + temp.endTime + ':42.346Z';
    console.log(JSON.stringify(temp));
    if (this.addForm.hasError) {
      this.http
        .post(this.base_url + 'AddCourse', temp, {
          headers: { Authorization: 'Bearer ' + this.idtoken },
        })
        .subscribe(
          (response) => {
            console.log(response);
            this.mode = false;
            this.addForm.reset();
            this.toastr.success('Course has successfully added', 'added');
          },
          (error) => {
            this.toastr.error(error.error, 'Error');
            console.log(error.error);
          }
        );
    } else {
      console.log(this.addForm.errors);
      this.toastr.error('form is not valid', 'Error');
    }
  }

  public GetallDepartments() {
    this.route.queryParams.subscribe((obj) => {
      this.pagingInfop = {
        currentPages: !!obj['PageNumber'] ? +obj['PageNumber'] : 1,
        pageSize: !!obj['PageSize'] ? +obj['PageSize'] : 10,
        nextLink: '',
        prevLink: '',
        totalCount: 0,
        totalPages: 0,
      };
      const paramss = new HttpParams()
        .set('PageNumber', this.pagingInfop.currentPages.toString())
        .set('PageSize', this.pagingInfop.pageSize.toString());
      this.http
        .get(this.base_url + 'GetAllCourses', { params: paramss })
        .subscribe(
          (response: IResponseCourse) => {
            this.courses = response['courses'];
            this.pagingInfop = response['pagingInfo'];
            if (this.pagingInfop.totalPages < this.pagingInfop.currentPages) {
              paramss.set('PageNumber', this.pagingInfop.totalPages.toString());
              this.router.navigate([], {
                relativeTo: this.route,
                queryParams: {
                  PageNumber: this.pagingInfop.totalPages,
                  PageSize: this.pagingInfop.pageSize,
                },
              });
            }
          },
          (error) => {
            console.log(error);
          }
        );
    });
  }

  counter(i: number) {
    let list = [];
    if (i - 3 > 0) {
      list.push('..');
    }
    for (let index = i - 2; index <= i; index++) {
      if (index > 0) list.push(index);
    }
    for (let index = i + 1; index < i + 3; index++) {
      if (index <= this.pagingInfop.totalPages) list.push(index);
    }
    if (i + 2 < this.pagingInfop.totalPages) {
      list.push('..');
    }
    return list;
  }
}
