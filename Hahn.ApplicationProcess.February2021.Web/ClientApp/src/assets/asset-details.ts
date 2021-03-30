import { autoinject } from 'aurelia-framework';
import { HttpService } from 'resources/http-service';
import { stringUtcToDate, dateToUtcString } from 'resources/utilities';
import { IAsset } from '../interfaces/asset';
import { departments, IDepartment } from '../interfaces/departments';
import { ValidationControllerFactory, ValidationController, ValidationRules } from 'aurelia-validation';

@autoinject
export class AssetDetails {
  controller: ValidationController;
  asset: IAsset;
  departments: IDepartment[];
  countries: string[];
  title: string;

  constructor(private http: HttpService, controllerFactory: ValidationControllerFactory) {
    this.departments = departments();
    this.controller = controllerFactory.createForCurrentScope();
  }

  async activate(params: any) {
    if (params.id === 'new') {
      this.title = 'New asset';
      this.asset = <IAsset> {
        broken: false
      };
    } else {
      this.asset = await this.http.getItems<IAsset>(`api/Asset/${params.id}`);
      this.asset.purchaseDate = dateToUtcString(this.asset.purchaseDate);
      this.title = this.asset.assetName;
    }
    let countries = await this.http.getItems<any>('https://restcountries.eu/rest/v2/all');
    this.countries = countries.map(c => c.name);
  }

  public bind() {
    ValidationRules
      .ensure((a: IAsset) => a.assetName)
        .required().then()
        .satisfies(v => this.http.validate(`api/Asset/validateName?assetName=${v}`))
          .withMessageKey('invalidAssetName').on(this.asset)
      .ensure((a: IAsset) => a.department)
        .required().then()
        .satisfies(v => this.http.validate(`api/Asset/validateDepartment?department=${v}`))
          .withMessageKey('invalidDepartment').on(this.asset)
      .ensure((a: IAsset) => a.countryOfDepartment)
        .required().then()
        .satisfies(v => this.http.validate(`api/Asset/validateCountry?country=${v}`))
          .withMessageKey('invalidCountryName').on(this.asset)
      .ensure((a: IAsset) => a.emailAdressOfDepartment)
        .required().then()
        .satisfies(v => this.http.validate(`api/Asset/validateEmail?email=${v}`))
          .withMessageKey('invalidEmailAddress').on(this.asset)
      .ensure((a: IAsset) => a.purchaseDate)
        .required().then()
        .matches(/^(\d{4}-\d{2}-\d{2} \d{2}:\d{2})$/).then()
        .satisfies(v => this.http.validate(`api/Asset/validateDate?date=${v}`))
          .withMessageKey('invalidPurchaseDate').on(this.asset);
  }
}
