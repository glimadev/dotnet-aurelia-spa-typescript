import { inject } from "aurelia-framework";
import { ValidationControllerFactory, ValidationRules } from "aurelia-validation";
import { BootstrapFormRenderer } from "../resources/bootstrap-form-renderer";
import { HttpHelper } from "../services/http-helper";
import { CountryService } from "../services/country-service";
import { DialogService } from "aurelia-dialog";
import { Dialog } from "./dialog/dialog";
import { Router } from "aurelia-router";
import { Asset } from "models/asset";
import { Department } from "models/department";

@inject(
	ValidationControllerFactory,
	DialogService,
	Router,
	HttpHelper,
	CountryService
)
export class Home {
	controllerFactory: ValidationControllerFactory;
	dialogService: DialogService;
	countryService: CountryService;
	httpHelper: HttpHelper;
	router: Router;
	controller = null;
	asset: Asset;
	departments: Department;

	constructor(controllerFactory, dialogService, router, httpHelper, countryService) {
		this.controller = controllerFactory.createForCurrentScope();
		this.controller.addRenderer(new BootstrapFormRenderer());
		this.dialogService = dialogService;
		this.countryService = countryService;
		this.httpHelper = httpHelper;
		this.router = router;
		this.asset = new Asset();
		this.departments = new Department();
	}

	activate = async () => {
		this.countryService.getCountries();
		await this.setupValidation();
	};

	setupValidation() {

		ValidationRules.customRule(
			"dateGreaterThan",
			(value, obj, maxYear) => {
				var dateSelected = new Date(value);
				var ageDifMs = Date.now() - dateSelected.getTime();
				var ageDate = new Date(ageDifMs); // miliseconds from epochv
				var diff = Math.abs(ageDate.getUTCFullYear() - 1970);
				return (
					diff < maxYear
				);
			},
			"${$displayName} must be an date greater than ${$config.maxYear}",
			(maxYear) => ({ maxYear })
		);

		//validation rules starts from here
		ValidationRules.ensure("assetName")
			.displayName("Asset Name")
			.required()
			.minLength(5)
			.withMessage("Asset Name at least 5 characters")

			.ensure("eMailAdressOfDepartment")
			.displayName("E-mail Address of Department")
			.required()
			.email()
			.withMessage("E-mail Address of Department - must be an valid e-mail")

			.ensure("purchaseDate")
			.required()
			.satisfiesRule("dateGreaterThan", 1)
			.withMessage("PurchaseDate - must be an date greater than 1 year")

			.ensure("department")
			.required()
			.satisfies((val) => this.departments.listNumbers.indexOf(val) > -1)
			.withMessage("Please select valid department")

			.ensure("countryOfDepartment")
			.required()
			.satisfies((val) => this.countryService.countries.filter((c) => c.name == val).length > 0)
			.withMessage("Please select valid country")

			.on(this.asset);
	}

	resetDialog() {
		this.dialogService
			.open({ viewModel: Dialog, model: "are you really sure to reset all the data" })
			.whenClosed(response => {
				if (!response.wasCancelled) {
					this.asset = null;
				}
			});
	}

	get listCountries() {
		return this.countryService.countries;
	}

	get listDepartments() {
		return this.departments.list;
	}

	//enable send button when form validation is done
	get canSave() {
		return (
			this.asset.assetName
			&& this.asset.department
			&& this.asset.countryOfDepartment
			&& this.asset.eMailAdressOfDepartment
			&& this.asset.purchaseDate
		);;
	}

	//enable reset button when user type something
	get canReset() {
		return (
			this.asset.assetName
			|| this.asset.department
			|| this.asset.countryOfDepartment
			|| this.asset.eMailAdressOfDepartment
			|| this.asset.purchaseDate
			|| this.asset.broken
		);
	}

	execute = async () => {
		try {
			var res = await this.controller.validate();
			if (res.valid) {
				await this.httpHelper
					.postData('asset', this.asset)
					.then((response: Response) => {
						if (response.status == 201) {
							this.router.navigateToRoute("success");
						} else {
							this.httpHelper.displayMessages(response);
						}

						return null;
					});
			}
		} catch (error) {
			console.log(error);
		}
	};
}
