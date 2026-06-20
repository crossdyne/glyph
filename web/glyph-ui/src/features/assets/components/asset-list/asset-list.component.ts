import { Component, input } from "@angular/core";
import { AssetResponse } from "../../../../core/contracts/responses/asset.response";

@Component({
    selector: 'asset-list',
    templateUrl: './asset-list.component.html',
    styleUrls: ['./asset-list.component.scss'],
    standalone: true
})
export class AssetListComponent {
    assets = input<AssetResponse[]>([]);
}