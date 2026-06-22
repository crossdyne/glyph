import { Component, input } from "@angular/core";
import { AssetUrlResponse } from "../../../../core/contracts/responses/asset-urls.response";

@Component({
    selector: 'asset-list',
    templateUrl: './asset-list.component.html',
    styleUrls: ['./asset-list.component.scss'],
    standalone: true
})
export class AssetListComponent {
    assets = input<AssetUrlResponse[]>([]);
}