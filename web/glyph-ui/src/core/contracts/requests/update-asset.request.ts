export interface UpdateAssetRequest {
    assetId: string;
    assetName: string;
    file: File | null;
    projectIds: string[];
    categoryId: string;
}