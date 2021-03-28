import { Aurelia } from 'aurelia-framework';
import { Router, RouterConfiguration } from 'aurelia-router';
import { PLATFORM } from 'aurelia-pal';

export class App {
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = 'Hahn Demo App';
    config.titleSeparator = ' | ';
    config.map([{
      route: ['', 'home', 'assets'],
      name: 'assets',
      settings: { icon: 'fa fa-list' },
      moduleId: PLATFORM.moduleName('./assets/assets'),
      nav: true,
      title: 'Assets'
    }, {
        route: "swagger",
        name: "swagger",
        redirect: 'swagger',
        settings: { icon: "fa fa-plus" },
        nav: true,
        title: "Swagger"
      }
    ]);
    this.router = router;
  }
}

///swagger