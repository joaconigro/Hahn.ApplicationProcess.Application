import { I18N } from 'aurelia-i18n';
import { autoinject } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';

@autoinject
export class I18NService {
  constructor(private i18n: I18N, private ea: EventAggregator) {
    this.i18n.setLocale(i18n.getLocale());
  }

  tr(key: string | string[]): string {
    return this.i18n.tr(key);
  }

  setLocale(locale: string) {
    return this.i18n.setLocale(locale);
  }
}
