import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FooterComponent } from './footer/footer.component';
import { RegisterComponent } from './register/register.component';
import { ShowCoursesComponent } from './show-courses/show-courses.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { TaechersComponent } from './taechers/taechers.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ShowTeachersComponent } from './show-teachers/show-teachers.component';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { ContainerComponent } from './container/container.component';
import { AdminheaderComponent } from './adminheader/adminheader.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { AdminPanelDepartmentsComponent } from './admin-panel-departments/admin-panel-departments.component';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { EditFComponent } from './edit-f/edit-f.component';
import { AdminPanelStudentsComponent } from './admin-panel-students/admin-panel-students.component';
import { AdminPanelTeachersComponent } from './admin-panel-teachers/admin-panel-teachers.component';
import { AdminPanelCoursesComponent } from './admin-panel-courses/admin-panel-courses.component';
import { ProfileComponent } from './profile/profile.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    RegisterComponent,
    ShowCoursesComponent,
    NotFoundComponent,
    HomeComponent,
    LoginComponent,
    TaechersComponent,
    ShowTeachersComponent,
    AdminPanelComponent,
    ContainerComponent,
    AdminheaderComponent,
    AdminPanelDepartmentsComponent,
    EditFComponent,
    AdminPanelStudentsComponent,
    AdminPanelTeachersComponent,
    AdminPanelCoursesComponent,
    ProfileComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserModule,
    HttpClientModule,
    FontAwesomeModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
