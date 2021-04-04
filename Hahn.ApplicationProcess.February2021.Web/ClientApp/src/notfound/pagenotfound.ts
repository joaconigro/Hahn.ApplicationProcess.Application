import { autoinject } from 'aurelia-framework';
import { I18NService } from '../resources/i18n-service';

@autoinject
export class PageNotFound {
  title: string;
  message: string;

  constructor(public i18n: I18NService) {
    this.title = i18n.tr('PageNotFound');
    this.message = i18n.tr('PageNotFoundMessage');
  }
}
