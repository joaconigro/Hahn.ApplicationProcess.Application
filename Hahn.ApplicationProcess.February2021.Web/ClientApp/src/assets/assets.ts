import { HttpService } from 'resources/http-service';
import { autoinject } from 'aurelia-framework';
import { IAsset } from '../interfaces/asset';
import { Router } from 'aurelia-router';
import { dateToUtcString } from 'resources/utilities';
import { DialogService } from 'aurelia-dialog';
import { I18NService } from '../resources/i18n-service';
import { Confirm } from '../dialogs/confirm';

@autoinject
export class Assets {
  constructor(private http: HttpService, private router: Router,
    private i18n: I18NService, private dialogService: DialogService) { }

	assets: IAsset[];

  async activate() {
    this.assets = await this.http.getItems<IAsset[]>('api/Asset/all');
    this.assets.forEach(a => {
      a.purchaseDate = dateToUtcString(a.purchaseDate);
    });
  }

  goToDetails(id: number) {
    this.router.navigate(`details/${id}`);
  }

  createAsset() {
    this.router.navigate(`details/new`);
  }

  deleteAsset(id: number) {
    this.dialogService.open({ viewModel: Confirm, model: this.i18n.tr('RemoveAssetMessage'), lock: true }).whenClosed(response => {
      if (response.wasCancelled) return;
    });
  }
}


