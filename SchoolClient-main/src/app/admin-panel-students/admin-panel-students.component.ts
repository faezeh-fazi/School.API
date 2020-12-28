import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import {
  IAddDepartment,
  IDepartment,
  IEditDepartment,
  IResponseDepartment,
} from '../Interfaces/Department-interface';
import { IPaging } from '../Interfaces/paging-interface';
import { IResponseStundet, IStudent } from '../Interfaces/Students-interface';
import { UserService } from '../user.service';

@Component({
  selector: 'app-admin-panel-students',
  templateUrl: './admin-panel-students.component.html',
  styleUrls: [
    './admin-panel-students.component.css',
    '../admin-panel/sb-admin-2.min.css',
  ],
})
export class AdminPanelStudentsComponent implements OnInit {
  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private userService: UserService,
    private toastr: ToastrService
  ) {}
  @ViewChild('edit_name') edit_name: ElementRef;
  addForm: FormGroup;
  departments: IStudent[];
  pagingInfop: IPaging;
  mode: boolean = true;
  deletItem: string = null;
  idtoken: string;
  editMode: boolean = false;
  editElement: number = null;

  base_url = 'https://localhost:5001/';
  ngOnInit(): void {
    this.s();
    this.formAddInit();

    this.get_Authorize();
  }

  public s() {
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
        .get(this.base_url + 'GetAllDepartmentStudents', { params: paramss })
        .subscribe(
          (response: IResponseStundet) => {
            console.log(response)
          },
          (error) => {
            this.toastr.error(error.error, 'Error');

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

  delete_item() {
    this.http
      .delete(this.base_url + 'deletedepartment/' + this.deletItem, {
        headers: { Authorization: 'Bearer ' + this.idtoken },
        responseType: 'text',
      })
      .subscribe({
        next: (data) => {
          console.log('Delete successful');
          this.s();
        },
        error: (error) => {
          console.log('There was an error!', error);
        },
      });
  }

  add_item() {
    const name: IAddDepartment = {
      name: this.addForm.get('name').value,
    };
    if (this.addForm.get('name').hasError) {
      this.http
        .post(this.base_url + 'AddDepartment', name, {
          headers: { Authorization: 'Bearer ' + this.idtoken },
          responseType: 'text',
        })
        .subscribe(
          (response) => {
            this.mode = true;
            this.toastr.success('Department has successfully added', 'added');
            this.addForm.reset();
          },
          (error) => {
            this.toastr.error(error.error, 'Error');

          }
        );
    } else {
      this.toastr.error('new value is not valid', 'Error');
    }
  }

  formAddInit() {
    this.addForm = new FormGroup({
      name: new FormControl('', [Validators.required]),
    });
  }

  get_Authorize() {
    this.userService.currentToken$.subscribe((res) => {
      this.idtoken = res.token;
    });
  }

  editelm() {
    const name: IEditDepartment = {
      name: this.edit_name.nativeElement.value,
    };
    this.http
      .put(this.base_url + 'updatedepartment/' + this.editElement, name, {
        headers: { Authorization: 'Bearer ' + this.idtoken },
      })
      .subscribe(
        (response) => {
          this.toastr.success('Department has successfully Edited', 'Edited');
          this.s();
        },
        (error) => {
          this.toastr.error(error.error, 'Error');
        }
      );
  }
}
