import { Router } from "aurelia-router";
import { autoinject } from "aurelia-framework";
import { I18NService } from "../resources/i18n-service";

@autoinject
export class Navmenu {
  swagerLink: string;

  constructor(private router: Router, private i18n: I18NService) {
    const baseUrl = document.getElementsByTagName('base')[0].href;
    this.swagerLink = `${baseUrl}swagger`;
  }

  //Update the locale with the one choosen by the user.
  setLocale(locale: string) {
    this.i18n.setLocale(locale);
  }
}
