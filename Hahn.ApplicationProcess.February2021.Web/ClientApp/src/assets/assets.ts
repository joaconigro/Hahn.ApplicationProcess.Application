import { HttpService } from 'resources/http-service';
import { autoinject } from 'aurelia-framework';
import { IAsset } from '../interfaces/asset';
import { Router } from 'aurelia-router';
import { dateToUtcString, mapDepartment } from 'resources/utilities';
import { DialogService } from 'aurelia-dialog';
import { I18NService } from '../resources/i18n-service';
import { Confirm } from '../dialogs/confirm';

@autoinject
export class Assets {
  assets: IAsset[];
  baseUrl = 'api/Asset/';

  constructor(private http: HttpService, private router: Router,
    private i18n: I18NService, private dialogService: DialogService) { }


  async activate() {
    await this.loadData();
  }

  private async loadData() {
    this.assets = await this.http.getItems<IAsset[]>(`${this.baseUrl}all`);
    this.assets.forEach(a => {
      a.purchaseDate = dateToUtcString(a.purchaseDate);
    });
  }

  displayDepartmentText(value: number): string {
    return mapDepartment(value, this.i18n);
  }

  goToDetails(id: number) {
    this.router.navigate(`asset/${id}`);
  }

  createAsset() {
    this.router.navigate(`asset/new`);
  }

  deleteAsset(id: number) {
    this.dialogService.open({ viewModel: Confirm, model: this.i18n.tr('RemoveAssetMessage'), lock: true }).whenClosed(async response => {
      if (response.wasCancelled) return;

      const asset = await this.http.deleteItem<IAsset>(`${this.baseUrl}${id}`);
      if (asset.id === id) {
        this.loadData();
      }
    });
  }
}


