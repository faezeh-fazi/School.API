import { relative } from '@angular/compiler-cli/src/ngtsc/file_system';
import { Route } from '@angular/compiler/src/core';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ILoginResp, IUSer, IUserLogin } from '../Interfaces/app-interface';
import { UserService } from '../user.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  constructor(private userService: UserService) {}
  currentUser$: Observable<IUSer>;

  ngOnInit(): void {
    this.currentUser$ = this.userService.currentUser$;
  }

  logout() {
    this.userService.logout();
  }

}
