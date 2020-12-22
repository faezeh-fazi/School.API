import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ITeacherView } from '../Interfaces/app-interface';
import { IDepartment } from '../Interfaces/Department-interface';

@Component({
  selector: 'app-taechers',
  templateUrl: './taechers.component.html',
  styleUrls: ['./taechers.component.css'],
})
export class TaechersComponent implements OnInit {
  base_url = 'https://localhost:5001/';
  departments: IDepartment;
  constructor(private http: HttpClient) {}
  ngOnInit(): void {
    this.get_Teachers();
  }

  show() {
    console.log(this.departments);
  }

  get_Teachers() {
    this.http.get(this.base_url + 'GetAlldepartments').subscribe(
      (response: IDepartment) => {
        this.departments = response['departments'];
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
