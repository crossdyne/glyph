export interface UpdateAssetRequest {
    assetId: string;
    assetName: string;
    file: File;
    projectIds: string[];
    categoryId: string;
}