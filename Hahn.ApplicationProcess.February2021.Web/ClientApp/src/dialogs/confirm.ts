import { autoinject } from "aurelia-framework";
import { DialogController } from 'aurelia-dialog';

@autoinject
export class Confirm {
  message = "";

  constructor(public controller: DialogController) { }

  activate(data) {
    this.message = data;
  }
}
