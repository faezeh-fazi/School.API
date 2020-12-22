import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminPanelCoursesComponent } from './admin-panel-courses/admin-panel-courses.component';
import { AdminPanelDepartmentsComponent } from './admin-panel-departments/admin-panel-departments.component';
import { AdminPanelStudentsComponent } from './admin-panel-students/admin-panel-students.component';
import { AdminPanelTeachersComponent } from './admin-panel-teachers/admin-panel-teachers.component';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { ContainerComponent } from './container/container.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { RegisterComponent } from './register/register.component';
import { ShowTeachersComponent } from './show-teachers/show-teachers.component';
import { TaechersComponent } from './taechers/taechers.component';

const routes: Routes = [
  {
    path: '',
    component: ContainerComponent,
    children: [
      { path: '', component: HomeComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'login', component: LoginComponent },
      { path: 'Departments', component: TaechersComponent },
      { path: 'all-teachers', component: ShowTeachersComponent },
      { path: '404-error', component: NotFoundComponent },
    ],
  },
  {
    path: 'admin-panel',
    component: AdminPanelComponent,
    children: [
      { path: 'Departments', component: AdminPanelDepartmentsComponent },
      { path: 'Students', component: AdminPanelStudentsComponent },
      { path: 'Courses', component: AdminPanelCoursesComponent },
      { path: 'Teachers', component: AdminPanelTeachersComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
