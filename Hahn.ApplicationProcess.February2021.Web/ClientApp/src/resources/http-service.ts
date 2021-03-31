import { HttpClient } from 'aurelia-fetch-client';
import { autoinject } from 'aurelia-framework';
import { IApiValidationResult } from '../interfaces/apiValidationResult';

@autoinject
export class HttpService {
  constructor(private http: HttpClient) { }


  async getItems<T>(url: string) {
    let items = await this.http.fetch(url)
      .then(result => result.json() as Promise<T>);
    return items;
  }

  async validate(url: string) {
    let result = await this.getItems<IApiValidationResult>(url);
    return new Promise<boolean>((resolve, reject) => { resolve(result.isValid) });
  }

  async deletItem<T>(url: string) {
    let items = await this.http.delete(url)
      .then(result => result.json() as Promise<T>);
    return items;
  }
}
