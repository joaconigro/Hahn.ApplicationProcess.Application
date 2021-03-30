import { HttpService } from 'resources/http-service';
import { autoinject } from 'aurelia-framework';
import { IAsset } from '../interfaces/asset';
import { Router } from 'aurelia-router';
import { dateToUtcString } from 'resources/utilities';

@autoinject
export class Assets {
	constructor(private http: HttpService, private router: Router) { }

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
}


