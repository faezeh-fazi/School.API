import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-edit-f',
  templateUrl: './edit-f.component.html',
  styleUrls: ['./edit-f.component.css'],
})
export class EditFComponent implements OnInit {
  constructor() {}
  @Input() formname: FormGroup;
  ngOnInit(): void {}
}
