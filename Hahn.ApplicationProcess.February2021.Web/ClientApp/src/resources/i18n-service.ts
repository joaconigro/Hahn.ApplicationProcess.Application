import { I18N } from 'aurelia-i18n';
import { autoinject } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';

@autoinject
export class I18NService {
  constructor(private i18n: I18N, private ea: EventAggregator) {
    this.i18n.setLocale(i18n.getLocale());
  }

  //Get the translation value, if the key isn't found, returns an empty string.
  tr(key: string | string[]): string {
    return this.i18n.tr(key, {defaultValue: ''});
  }

  //A helper method for check if a key exists.
  hasKey(key: string) : boolean {
    return this.tr(key).length > 0;
  }

  //Update the locale translation.
  setLocale(locale: string) {
    return this.i18n.setLocale(locale);
  }
}
