import { HttpClient, json } from "aurelia-fetch-client";
import { inject } from "aurelia-framework";
//import { api } from "../config/api";

@inject(HttpClient)
export class CountryService {
	private http: HttpClient;
	public countries: any;

	constructor(http: HttpClient) {
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
	}

	public getCountries() {
		var countries = sessionStorage.getItem("countries");

		if (countries !== null) {
			this.countries = JSON.parse(countries);
			if (Array.isArray(this.countries) && this.countries.length > 0)
				return;
		}

		return this.http
			.fetch('country', {
				method: 'get',
			})
			.then((response) => response.json())
			.then((countries) => {
				sessionStorage.setItem("countries", JSON.stringify(countries.data));
				this.countries = countries.data;
			})
			.catch((error) => {
				console.log("Error retrieving countires.", error);
				this.countries = [];
			});
	}

	//getCountry() {
	//  return this.http
	//    .fetch("applicant/GetCountry", {
	//      method: "get",
	//    })
	//    .then((response) => response.json())
	//    .then((countries) => {
	//      debugger;
	//      console.log("Fetching... ", countries);
	//      localStorage.setItem("countries", JSON.stringify(countries));

	//      return countries;
	//    })

	//    .catch((error) => {
	//      console.log("Error retrieving countires.", error);
	//      return [];
	//    });
	//}
}
