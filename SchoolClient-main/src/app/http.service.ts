import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  constructor() {}
  base_url = 'https://localhost:5001/';
  departments: string[];

  public get_url_no_params(api_link: string) {

  }
}
