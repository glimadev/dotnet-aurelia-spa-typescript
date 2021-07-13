import 'bootstrap/dist/css/bootstrap.css';
import "font-awesome/css/font-awesome.css";
import './app.css';

import { Aurelia } from 'aurelia-framework';
import { PLATFORM } from 'aurelia-pal';
import { I18N, Backend, TCustomAttribute } from 'aurelia-i18n';

export async function configure(aurelia: Aurelia) {
	aurelia.use
		.standardConfiguration()
		.developmentLogging();

	aurelia.use
		.plugin(PLATFORM.moduleName("aurelia-validation"))
		.plugin(PLATFORM.moduleName("aurelia-dialog"))
		.plugin(PLATFORM.moduleName('resources'))
		.plugin(PLATFORM.moduleName('aurelia-i18n'), (instance) => {
			let aliases = ['t', 'i18n'];
			// add aliases for 't' attribute
			TCustomAttribute.configureAliases(aliases);

			// register backend plugin
			instance.i18next.use(Backend.with(aurelia.loader));

			// adapt options to your needs (see https://i18next.com/docs/options/)
			// make sure to return the promise of the setup method, in order to guarantee proper loading
			return instance.setup({
				backend: {                                  // <-- configure backend settings
					loadPath: './locales/{{lng}}/{{ns}}.json', // <-- XHR settings for where to get the files from
				},
				attributes: aliases,
				lng: 'en',
				fallbackLng: 'en',
				debug: false,
				ns: ['translation'],
				defaultNS: 'translation'
			});
		});;

	await aurelia.start();
	await aurelia.setRoot(PLATFORM.moduleName('app'));
}
