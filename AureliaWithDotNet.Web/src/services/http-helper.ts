import { HttpClient, json } from "aurelia-fetch-client";
import { DialogService } from "aurelia-dialog";
import { inject } from "aurelia-framework";
import { ApiDialog } from "views/dialog/apiDialog";
//import { api } from "../config/api";

@inject(HttpClient, DialogService)
export class HttpHelper {
	private http: HttpClient;
	private dialogService: DialogService;

	constructor(http: HttpClient, dialogService: DialogService) {
		http.configure((config) => {
			config
				.withBaseUrl("api/")
				.withDefaults({
					credentials: "same-origin",
					headers: {
						Accept: "application/json",
						"X-Requested-With": "Fetch",
					},
				})
				.withInterceptor({
					request(request) {
						console.log(`Requesting ${request.method} ${request.url}`);
						return request; // you can return a modified Request, or you can short-circuit the request by returning a Response
					},
					response(response) {
						console.log(`Received ${response.status} ${response.url}`);
						return response; // you can return a modified Response
					},
				});
		});
		this.http = http;
		this.dialogService = dialogService;
	}

	public postData(controller, body) {
		return this.http
			.fetch(controller, {
				method: "post",
				body: json(body),
			})
			.then((response) => response)
			.catch((error) => {
				console.log("Error saving applicant", error);
			});
	}

	displayMessages(response) {
		this.dialogService
			.open({
				viewModel: ApiDialog,
				model: response.json(),
			});
	}
}
