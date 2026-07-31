import { AssetUrlResponse } from "../../../core/contracts/responses/asset-urls.response";
import { CategoryResponse } from "../../../core/contracts/responses/category.response";

export class Sorting {

   static sortCategoriesAlphabet(categories: CategoryResponse[]) {
        return categories.sort((a, b) => {
                const nameA = a.name.toLocaleLowerCase();
                const nameB = b.name.toLocaleLowerCase();

                return nameA.localeCompare(nameB);
            });
    }

    static sortGroupedAssetsAlphabet(grouped: { title: string, assets: AssetUrlResponse[] }[]) {
        if (!grouped)
            return [{ title: '', assets: []}];

         return grouped?.sort((a, b) => {
            const titleA = a.title;
            const titleB = b.title;

            return titleA.localeCompare(titleB);
        });
    }
}