import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import jwtDecode from 'jwt-decode';
import { ReplaySubject } from 'rxjs';
import { ILoginResp, IUSer, IUserLogin } from './Interfaces/app-interface';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  base_url = 'https://localhost:5001/';
  private Token = new ReplaySubject<ILoginResp>(1);
  private currentUserSource = new ReplaySubject<IUSer>(1);
  currentToken$ = this.Token.asObservable();
  currentUser$ = this.currentUserSource.asObservable();

  constructor(
    private http: HttpClient,
    private tokenService: TokenService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  public login(model: IUserLogin) {
    return this.http
      .post(this.base_url + 'api/login', model)
      .subscribe((response: ILoginResp) => {
        localStorage.setItem('user', JSON.stringify(response));
        this.currentUserSource.next(this.tokenService.toUser(response));
        this.Token.next(response);
      });
  }

  setCurrentUser(resp: ILoginResp) {
    this.Token.next(resp);
     this.currentUserSource.next(this.tokenService.toUser(resp));
  }

  logout() {
    this.Token.next(null);
    this.currentUserSource.next(null);
    localStorage.removeItem('user');
  }

  public decode_jwt() {
    this.currentToken$.subscribe((res: ILoginResp) => {
      console.log(res);
      if (this.tokenService.tokenExpired) {
        console.log('token is valid');
      } else {
        console.log('token has expired');
      }
    });

  }
}
