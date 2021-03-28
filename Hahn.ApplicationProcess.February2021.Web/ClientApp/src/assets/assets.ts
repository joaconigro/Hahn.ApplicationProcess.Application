import { HttpClient } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';
import { IAsset } from '../interfaces/asset';

@inject(HttpClient)
export class Assets {
	constructor(private http: HttpClient) { }

	assets: IAsset[];

	async activate() {
    this.assets = await this.http.fetch('api/Asset/all')
      .then(result => result.json() as Promise<IAsset[]>);
	}
}


