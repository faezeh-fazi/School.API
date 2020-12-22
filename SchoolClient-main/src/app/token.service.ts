import { Injectable } from '@angular/core';
import jwtDecode from 'jwt-decode';
import { decode } from 'querystring';
import { ILoginResp, IUSer } from './Interfaces/app-interface';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  constructor() {}

  public tokenExpired(token: string) {
    const expiry = JSON.parse(atob(token.split('.')[1])).exp;
    return Math.floor(new Date().getTime() / 1000) >= expiry;
  }

  public getUserId(token: string) {
    return jwtDecode(token);
  }


  public toUser(response:ILoginResp):IUSer{
    let userdata = jwtDecode(response.token)
     const user:IUSer = {

       id:userdata["id"],
       role:userdata["role"][0],
       name:userdata["name"]
     }
    return(user)
  }
}
