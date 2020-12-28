import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ITeacherView } from '../Interfaces/app-interface';
import { UserService } from '../user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-show-teachers',
  templateUrl: './show-teachers.component.html',
  styleUrls: ['./show-teachers.component.css'],
})
export class ShowTeachersComponent implements OnInit {
  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private toaster: ToasterService
  ) {}
  base_url = 'https://localhost:5001/';
  teachers: Array<ITeacherView>;
  departmentId: string;
  ngOnInit(): void {
    this.get_teachers();
  }

  get_teachers() {
    this.route.queryParams.subscribe((obj) => {
      this.departmentId = obj['DepartmentId'];
    });
    const paramss = new HttpParams().set('DepartmentId', this.departmentId);
    return this.http
      .get(this.base_url + 'GetAllDepartmentTeachers', { params: paramss })
      .subscribe(
        (response: Array<ITeacherView>) => {
          this.teachers = response;
        },
        (error) => {
          this.toaster.error(error.error, 'Error');
        }
      );
  }
}
