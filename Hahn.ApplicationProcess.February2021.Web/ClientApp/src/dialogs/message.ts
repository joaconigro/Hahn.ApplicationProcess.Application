import { autoinject } from "aurelia-framework";
import { DialogController } from 'aurelia-dialog';

@autoinject
export class Message {
  message = "";
  title = "";

  constructor(public controller: DialogController) { }

  activate(data) {
    this.title = data.title;
    this.message = data.message;
  }
}
