import { autoinject  } from 'aurelia-framework';
import { Router, RouterConfiguration, NavigationInstruction, RouteConfig } from 'aurelia-router';
import { PLATFORM } from 'aurelia-pal';
import { I18NService } from './resources/i18n-service';

@autoinject
export class App {
  constructor(private i18: I18NService) {}
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
      title: this.i18.tr('Assets')
    },
      {
        route: ['asset/:id'],
        name: 'asset',
        moduleId: PLATFORM.moduleName('./assets/asset-details'),
        title: this.i18.tr('Details')
      }
    ]);

    const handleUnknownRoutes = (instruction: NavigationInstruction): RouteConfig => {
      return { route: 'not-found', moduleId: PLATFORM.moduleName('./notfound/pagenotfound') };
    }

    config.mapUnknownRoutes(handleUnknownRoutes);
    this.router = router;
  }
}
