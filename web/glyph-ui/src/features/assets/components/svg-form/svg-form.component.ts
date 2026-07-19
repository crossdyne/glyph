import { Component, effect, input, output, signal } from "@angular/core";
import { AssetUrlResponse } from "../../../../core/contracts/responses/asset-urls.response";
import { UpdateAssetRequest } from "../../../../core/contracts/requests/update-asset.request";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { Combobox } from "../../../../shared/ui/combobox/combobox";
import { ProjectResponse } from "../../../../core/contracts/responses/project.response";
import { CreateAssetRequest } from "../../../../core/contracts/requests/create-asset.request";
import { CategoryResponse } from "../../../../core/contracts/responses/category.response";

@Component({
    selector: 'svg-form',
    templateUrl: './svg-form.component.html',
    styleUrls: ['./svg-form.component.scss'],
    standalone: true,
    imports: [ReactiveFormsModule, Combobox],
})
export class SvgFormComponent {
  
  asset = input<AssetUrlResponse | null>(null);
  selectedFile = input<File | null>(null);
  projects = input<ProjectResponse[]>([]);
  categories = input<CategoryResponse[]>([]); 

  created = output<CreateAssetRequest>(); 
  updated = output<UpdateAssetRequest>(); 
  cancelled = output<void>();

  previewUrl = signal<string | null>(null);
  saving = signal(false);
  uploadError = signal<string | null>(null);

  form: FormGroup = new FormGroup({
      assetName: new FormControl('', Validators.required),
      selectedProject: new FormControl<ProjectResponse | null>(null, Validators.required),
      selectedCategory: new FormControl<CategoryResponse | null>(null, Validators.required),
  });

  constructor() {
      effect(() => {
        const assetData = this.asset();
        const file = this.selectedFile();
        const projectsData = this.projects();
        const categoriesData = this.categories();

        if (!assetData && !file) {
          this.previewUrl.set(null);
          this.uploadError.set(null);
          this.form.reset(undefined, { emitEvent: false });
          return;
        }

        if (file) {
          this.previewUrl.set(URL.createObjectURL(file));
        } else if (assetData) {
          this.previewUrl.set(assetData.url);

          let projectToSelect: ProjectResponse | null = null;
          let categoryToSelect: CategoryResponse | null = null;

          if (projectsData.length > 0 && assetData.projectIds?.length > 0) {
            const targetId = String(assetData.projectIds[0]).trim().toLowerCase();

            projectToSelect = projectsData.find(p => {
               const projectId = String(p.id).trim().toLowerCase();
               const isMatch = projectId === targetId;

               return isMatch;
            }) || null;

          if (categoriesData.length > 0 && assetData.categoryId) {
            const targetCategoryId = String(assetData.categoryId).trim().toLowerCase();

            categoryToSelect = categoriesData.find(c => {
               const categoryId = String(c.categoryId).trim().toLowerCase();
               return categoryId === targetCategoryId;
            }) || null;
          }

          this.form.patchValue({
            assetName: assetData.assetName,
            selectedProject: projectToSelect,
            selectedCategory: categoryToSelect
          }, { emitEvent: false });
        }
      }
    });
  }

  onSubmit() {
    if (this.form.invalid || this.saving()) return;

    const assetName = this.form.get('assetName')?.value as string;
    const selectedProject = this.form.get('selectedProject')?.value as ProjectResponse;
    const selectedCategory = this.form.get('selectedCategory')?.value as CategoryResponse;
    const file = this.selectedFile();
    const existing = this.asset();

    if (!existing && !file) {
      this.uploadError.set('Файл не был выбран');
      return;
    }

    if (existing) {
      this.updated.emit({
        assetId: existing.assetId,
        assetName,
        file,
        projectIds: [selectedProject.id],
        categoryId: selectedCategory.categoryId
      });
    } else {
      this.created.emit({
        assetName,
        file: file!,
        categoryId: selectedCategory.categoryId,
        projectIdsJson: [selectedProject.id]
      });
    }
  }

  onCancel() {
    this.form.reset({
      assetName: '',
      selectedProject: null,
      selectedCategory: null
    });
    this.cancelled.emit();
  }
}