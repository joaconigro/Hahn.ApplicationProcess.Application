import { Router } from "aurelia-router";
import { autoinject } from "aurelia-framework";

@autoinject
export class Navmenu {
  swagerLink: string;

  constructor(private router: Router) {
    const baseUrl = document.getElementsByTagName('base')[0].href;
    this.swagerLink = `${baseUrl}swagger`;
  }

  //public navigateToSwagger(): void {
  //  const baseUrl = document.getElementsByTagName('base')[0].href;
  //  this.router.navigateToRoute(`${baseUrl}swagger`);
  //}
}
