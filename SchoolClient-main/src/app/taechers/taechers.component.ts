import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
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
    this.get_departments();
  }

  show() {
    console.log(this.departments);
  }

  get_departments() {
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
