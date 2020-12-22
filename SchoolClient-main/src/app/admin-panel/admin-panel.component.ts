import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { faLaughWink, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { UserService } from '../user.service';
@Component({
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css', './sb-admin-2.min.css'],
})
export class AdminPanelComponent implements OnInit, OnDestroy {
  constructor(
    private router: ActivatedRoute,
    private userService: UserService
  ) {}
  ngOnDestroy(): void {}
  admin_image: string = 'assets/undraw_profile.svg';
  wink_icon: IconDefinition;
  title: string;
  name: string;
  ngOnInit(): void {
    this.wink_icon = faLaughWink;
    this.userService.currentUser$.subscribe((resp) => {
      this.name = resp.name;
    });
  }
  logout() {
    this.userService.logout()
  }
}
