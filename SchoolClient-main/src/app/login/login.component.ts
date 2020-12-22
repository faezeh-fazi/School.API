import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ILoginResp, IUSer, IUserLogin } from '../Interfaces/app-interface';
import { UserService } from '../user.service';
import jwt_decode from 'jwt-decode';
import jwtDecode from 'jwt-decode';
import { Token } from '@angular/compiler/src/ml_parser/lexer';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  currentUser$: Observable<IUSer>;
  baseurl: string = 'https://localhost:5001/';
  constructor(private http: HttpClient, private userService: UserService) {}

  ngOnInit(): void {
    this.formInit();
    this.currentUser$ = this.userService.currentUser$;
  }

  formInit() {
    this.loginForm = new FormGroup({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
    });
  }

  logout() {
    this.userService.logout();
  }

  login() {
    const user: IUserLogin = {
      username: this.loginForm.get('username').value,
      password: this.loginForm.get('password').value,
    };
    this.userService.login(user);
  }

  public decode_jwt() {
    this.userService.decode_jwt();
  }
}
