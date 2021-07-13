import { Router } from 'aurelia-router';
import { PLATFORM } from 'aurelia-pal';

export class App {
    router: Router;

    configureRouter(config, router: Router) {
        config.title = 'Aurelia';
        config.options.pushState = true;
        config.map([
			{ route: ['', 'home'], name: 'home', moduleId: PLATFORM.moduleName('views/home'), nav: true, title: 'Home', settings: { name: "Aurelia" } },
			{ route: ['/success', 'success'], name: 'success', moduleId: PLATFORM.moduleName('views/success'), title: 'View Success' }
        ]);

        config.mapUnknownRoutes({ redirect: '' });

        this.router = router;
    }
}
