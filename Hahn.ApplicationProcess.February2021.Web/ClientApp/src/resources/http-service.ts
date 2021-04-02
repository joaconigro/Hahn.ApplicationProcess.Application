import { DialogService } from 'aurelia-dialog';
import { HttpClient, Interceptor } from 'aurelia-fetch-client';
import { autoinject } from 'aurelia-framework';
import { IApiValidationResult } from '../interfaces/apiValidationResult';
import { Message } from '../dialogs/message';
import { I18NService } from './i18n-service';

@autoinject
export class HttpService {
  private http: HttpClient;

  constructor(private dialogService: DialogService, private i18n: I18NService) {
    this.http = new HttpClient().configure(config => {
      config.withInterceptor(new HttpInterceptors(dialogService, i18n));
    });
  }

  //Send a GET request.
  async get<T>(url: string) {
    return this.fetch<T>(url, 'GET');
  }

  //Validates a control and return a Promise<boolean> that indicates if is valid.
  async validate(url: string) {
    let result = await this.get<IApiValidationResult>(url);
    return new Promise<boolean>(resolve => { resolve(result.isValid) });
  }

  //Send a DELETE request.
  async delete<T>(url: string) {
    return this.fetch<T>(url, 'DELETE');
  }

  //Send a POST request.
  async post<T>(url: string, item) {
    return this.fetch<T>(url, 'POST', item);
  }

  //Send a PUT request.
  async put<T>(url: string, item) {
    return this.fetch<T>(url, 'PUT', item);
  }

  //Handle all the request with this private function.
  private async fetch<T>(url: string, method: string, item: any = null): Promise<T> {
    return await this.http.fetch(url, { method: method, body: item ? JSON.stringify(item) : null })
      .then(res => res?.json() as Promise<T>);
  }
}

@autoinject
class HttpInterceptors implements Interceptor {
  constructor(private dialogService: DialogService, private i18n: I18NService) { }

  //Handle the response
  response(response) {
    //If the result isn't successful (2xx), handle the error.
    if (response.status < 200 || response.status > 299) {
      return this.responseError(response);
    }
    return response;
  }

  //Handle the error, concatenating all the error messages and display a message.
  responseError(error) {
    error.clone().json().then(r => {
      let message = this.i18n.tr('CheckErrors');
      let errors = Object.keys(r).filter(k => this.i18n.hasKey(k)).map(k => this.i18n.tr(k));
      message += errors.join(", ")
      this.dialogService.open({
        viewModel: Message,
        model: {
          title: `${this.i18n.tr('Error')} ${error.status}`,
          message: message
        },
        lock: true
      });

    });
    return null;
  }
}
