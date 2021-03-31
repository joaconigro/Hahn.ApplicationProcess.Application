import { Aurelia } from 'aurelia-framework';
import { Router, RouterConfiguration } from 'aurelia-router';
import { PLATFORM } from 'aurelia-pal';
import i18next from 'i18next';

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
      title: i18next.t('Assets')
    },
      {
        route: ['asset/:id'],
        name: 'asset',
        moduleId: PLATFORM.moduleName('./assets/asset-details'),
        title: i18next.t('Details')
      }
    ]);
    this.router = router;
  }
}
