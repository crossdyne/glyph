import { AssetUrlResponse } from "../../../core/contracts/responses/asset-urls.response";
import { CategoryResponse } from "../../../core/contracts/responses/category.response";

export class Grouping {
    static groupedAssets(categories: CategoryResponse[], assets: AssetUrlResponse[]) {
        const groups = new Map<string, AssetUrlResponse[]>();

        for (const asset of assets ) {
            const key = categories.find(x => x.categoryId === asset.categoryId)?.name ?? 'Без категории';

            if (!groups.has(key))
                groups.set(key, []);

            groups.get(key)!.push(asset);
        }

        return Array.from(groups.entries()).map(([title, assets]) => ({ title, assets }));
    }
}