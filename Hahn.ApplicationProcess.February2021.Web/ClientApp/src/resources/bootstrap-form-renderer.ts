import { RenderInstruction, ValidateResult } from 'aurelia-validation';

export class BootstrapFormRenderer {
  render(instruction: RenderInstruction) {
    for (let { result, elements } of instruction.unrender) {
      for (let element of elements) {
        this.remove(element, result);
      }
    }

    for (let { result, elements } of instruction.render) {
      for (let element of elements) {
        this.add(element, result);
      }
    }
  }

  add(element: Element, result: ValidateResult) {
    const formGroup = element.closest('.form-group');
    if (!formGroup) {
      return;
    }

    const internalElement = formGroup.querySelector(`#${result.propertyName}`);

    if (result.valid) {
      if (internalElement.classList.contains('is-invalid')) {
        internalElement.classList.remove('is-invalid');
      }
    } else {
      //Add the is-invalid class to the enclosing form-group div.
      if(!internalElement.classList.contains('is-invalid')) {
        internalElement.classList.add('is-invalid');
      }

      //Add invalid-feedback.
      const message = document.createElement('span');
      message.className = 'invalid-feedback';
      message.textContent = result.message;
      message.id = `validation-message-${result.id}`;
      formGroup.appendChild(message);
    }
  }

  remove(element: Element, result: ValidateResult) {
    const formGroup = element.closest('.form-group');
    if (!formGroup) {
      return;
    }

    const internalElement = formGroup.querySelector(`#${result.propertyName}`);
    if (internalElement.classList.contains('is-invalid')) {
      internalElement.classList.remove('is-invalid');
    }

    if (result.valid) {
      //Remove invalid-feedback
      const message = formGroup.querySelector(`#validation-message-${result.id}`);
      if (message) {
        formGroup.removeChild(message);

        // Remove the is-invalid class from the enclosing form-group div
        if (formGroup.querySelectorAll('.help-block.validation-message').length === 0) {
          formGroup.classList.remove('is-invalid');
        }
      }
    }

    //Remove invalid-feedback.
    const message = formGroup.querySelector(`#validation-message-${result.id}`);
    if (message) {
      formGroup.removeChild(message);

      //Remove the is-invalid class from the enclosing form-group div.
      if (formGroup.querySelectorAll('.help-block.validation-message').length === 0) {
        formGroup.classList.remove('is-invalid');
      }
    }
  }
}
