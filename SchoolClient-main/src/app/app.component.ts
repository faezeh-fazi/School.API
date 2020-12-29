import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ILoginResp, IUSer } from './Interfaces/app-interface';
import { TokenService } from './token.service';
import { UserService } from './user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  currentUser$: Observable<IUSer>;

  constructor(
    private userService: UserService,
    private tokenService: TokenService
  ) {}
  ngOnInit(): void {
    this.setCurrentUser();
  }
  setCurrentUser() {
    if (localStorage.getItem('user') != null) {
      const user: ILoginResp = JSON.parse(localStorage.getItem('user'));
      console.log(this.tokenService.tokenExpired(user.token));

      this.userService.setCurrentUser(user);
      this.currentUser$ = this.userService.currentUser$;
    }
  }
}
