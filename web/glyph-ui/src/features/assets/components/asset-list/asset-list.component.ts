import { Component, input, output } from "@angular/core";
import { AssetUrlResponse } from "../../../../core/contracts/responses/asset-urls.response";
import { DeleteButton } from "../../../../shared/ui/delete-button/delete-button";

@Component({
    selector: 'asset-list',
    templateUrl: './asset-list.component.html',
    styleUrls: ['./asset-list.component.scss'],
    standalone: true,
    imports: [DeleteButton]
})
export class AssetListComponent {
    assets = input<AssetUrlResponse[]>([]);
    assetSelected = output<AssetUrlResponse>();
    delete = output<string>();

    onDelete(id: string){
        this.delete.emit(id);
    }
}