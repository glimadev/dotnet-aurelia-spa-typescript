import { autoinject } from "aurelia-framework";
import { DialogController } from "aurelia-dialog";

@autoinject
export class ApiDialog {
  constructor(public controller: DialogController) {
    controller.settings.centerHorizontalOnly = true;
  }
  response : any;
  activate(data) {
    this.response = data;
  }
  cancel(): void {
    this.controller.cancel();
  }
}
