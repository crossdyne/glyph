import { Component, computed, input, output } from "@angular/core";
import { AssetUrlResponse } from "../../../../core/contracts/responses/asset-urls.response";
import { DeleteButton } from "../../../../shared/ui/delete-button/delete-button";
import { Sorting } from "../../utils/sorting.utils";

@Component({
    selector: 'asset-list',
    templateUrl: './asset-list.component.html',
    styleUrls: ['./asset-list.component.scss'],
    standalone: true,
    imports: [DeleteButton]
})
export class AssetListComponent {
    assets = input<{ title: string, assets: AssetUrlResponse[] }[] | null>(null);
    sortingAssets = computed(() => Sorting.sortGroupedAssetsAlphabet(this.assets()!));
    assetSelected = output<AssetUrlResponse>();
    delete = output<string>();

    onDelete(id: string){
        this.delete.emit(id);
    }
}