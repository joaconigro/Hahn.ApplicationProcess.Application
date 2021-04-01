import { autoinject } from 'aurelia-framework';
import { HttpService } from 'resources/http-service';
import { stringUtcToDate, dateToUtcString, mapDepartment } from 'resources/utilities';
import { IAsset } from '../interfaces/asset';
import { IDepartment } from '../interfaces/departments';
import { ValidationControllerFactory, ValidationController, ValidationRules } from 'aurelia-validation';
import { BootstrapFormRenderer } from '../resources/bootstrap-form-renderer';
import { I18NService } from '../resources/i18n-service';
import { Router } from 'aurelia-router';

@autoinject
export class AssetDetails {
  controller: ValidationController;
  asset: IAsset;
  countries: string[];
  title: string;
  isEditing: boolean;
  baseUrl = 'api/Asset/';

  constructor(private http: HttpService, controllerFactory: ValidationControllerFactory,
    readonly i18n: I18NService, private router: Router)
  {
    this.controller = controllerFactory.createForCurrentScope();
    this.controller.addRenderer(new BootstrapFormRenderer());
  }

  get departments(): IDepartment[] {
  const deps = Array<IDepartment>();
  for (let i = 0; i < 5; i++) {
    deps.push({ id: i, name: mapDepartment(i, this.i18n) });
  }
  return deps;
}

  async activate(params: any) {
    if (params.id === 'new') {
      this.title = this.i18n.tr('NewAsset');
      this.isEditing = false;
      this.asset = <IAsset> {
        broken: false
      };
    } else {
      this.asset = await this.http.get<IAsset>(`${this.baseUrl}${params.id}`);
      this.asset.purchaseDate = dateToUtcString(this.asset.purchaseDate);
      this.title = this.asset.assetName;
      this.isEditing = true;
    }
    await this.getCountries();
  }

  async getCountries() {
    let countries = localStorage.getItem('countries');
    if (!countries) {
      const response = await this.http.get<any>('https://restcountries.eu/rest/v2/all');
      this.countries = response.map(c => c.name);
      localStorage.setItem('countries', JSON.stringify(this.countries));
    } else {
      this.countries = JSON.parse(countries);
    }
  }

  async send() {
    this.asset.purchaseDate = (stringUtcToDate(this.asset.purchaseDate) as Date)?.toJSON();
    if (this.isEditing) {
      const item = await this.http.put<IAsset>(`${this.baseUrl}`, this.asset);
      if (item) {
        this.router.navigateToRoute('assets');
      }
    } else {
      const item = await this.http.post<IAsset>(`${this.baseUrl}`, this.asset);
      if (item) {
        this.router.navigateToRoute('asset', { id: item.id });
      }
    }
  }

  public bind() {
    ValidationRules
      .ensure((a: IAsset) => a.assetName)
        .required().withMessageKey('FieldRequired').then()
        .satisfies(v => this.http.validate(`api/Asset/validateName?assetName=${v}`))
          .withMessageKey('InvalidAssetName').on(this.asset)
      .ensure((a: IAsset) => a.department)
        .required().withMessageKey('FieldRequired').then()
        .satisfies(v => this.http.validate(`api/Asset/validateDepartment?department=${v}`))
          .withMessageKey('InvalidDepartment').on(this.asset)
      .ensure((a: IAsset) => a.countryOfDepartment)
        .required().withMessageKey('FieldRequired').then()
        .satisfies(v => this.http.validate(`api/Asset/validateCountry?country=${v}`))
          .withMessageKey('InvalidCountryName').on(this.asset)
      .ensure((a: IAsset) => a.emailAdressOfDepartment)
        .required().withMessageKey('FieldRequired').then()
        .satisfies(v => this.http.validate(`api/Asset/validateEmail?email=${v}`))
          .withMessageKey('InvalidEmailAddress').on(this.asset)
      .ensure((a: IAsset) => a.purchaseDate)
        .required().withMessageKey('FieldRequired').then()
        .satisfies(v => this.http.validate(`api/Asset/validateDate?date=${(stringUtcToDate(v) as Date).toJSON()}`))
        .withMessageKey('InvalidPurchaseDate').on(this.asset);
  }

  private checkValue(str: string, max: number, length: number, addZero: boolean): string {
    if (str.charAt(0) !== '0' || str == '00') {
      let num = parseInt(str);
      if (isNaN(num) || num <= 0) {
        num = 1;
      } else if (num > max) {
        num = max;
      }
      str = addZero && num.toString().length < length ? '0' + num : num.toString();
    };
    return str;
  }

  onTextChanged(value: string) {
    if (/[\d -:]{4,}/.test(value)) {
      const values = value.split(/ |-|:/);
      if (values[0]) values[0] = this.checkValue(values[0], new Date().getFullYear(), 4, false);
      if (values[1]) values[1] = this.checkValue(values[1], 12, 2, values.length > 2);
      if (values[2]) values[2] = this.checkValue(values[2], 31, 2, values.length > 3);
      if (values[3]) values[3] = this.checkValue(values[3], 23, 2, values.length > 4);
      if (values[4]) values[4] = this.checkValue(values[4], 59, 2, false);

      const output = values.map((v, i) => {
        switch (i) {
          case 0:
          case 1:
            return v.length > 1 ? `${v}-` : v;
          case 2:
            return v.length > 1 ?`${v} ` : v;
          case 3:
            return v.length > 1 ? `${v}:` : v;
          default:
            return v;
        }
      });

      this.asset.purchaseDate = output.join('');
    }
  }
}
