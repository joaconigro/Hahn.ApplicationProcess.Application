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
    public i18n: I18NService, private dialogService: DialogService) { }


  async activate() {
    await this.loadData();
  }

  //Loads the data from the server.
  private async loadData() {
    this.assets = await this.http.get<IAsset[]>(`${this.baseUrl}all`);
    if (this.assets) {
      //Format the date.
      this.assets.forEach(a => {
        a.purchaseDate = dateToUtcString(a.purchaseDate);
      });
    }
  }

  //Update the Department enum value with the translation.
  displayDepartmentText(value: number): string {
    return mapDepartment(value, this.i18n);
  }

  //Navigate to the selected asset.
  goToDetails(id: number) {
    this.router.navigate(`asset/${id}`);
  }

  //Navigate to the asset creation page.
  createAsset() {
    this.router.navigate(`asset/new`);
  }

  //Deletes an asset.
  deleteAsset(id: number) {
    //First, ask the user if really wants to remove the asset.
    this.dialogService.open({ viewModel: Confirm, model: this.i18n.tr('RemoveAssetMessage'), lock: true }).whenClosed(async response => {
      if (response.wasCancelled) return;

      //If the anwser is yes, will remove the asset and reload the assets.
      const asset = await this.http.delete<IAsset>(`${this.baseUrl}${id}`);
      if (asset?.id === id) {
        this.loadData();
      }
    });
  }
}
