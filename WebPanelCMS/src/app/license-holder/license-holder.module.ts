import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LicenseHolderComponent, NgbdSortableHeader } from './license-holder.component';
import { LicenseHolderRoutes } from './license-holder.routes';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxLoadingModule  } from 'ngx-loading';
import { Ng2SearchPipeModule } from 'ng2-search-filter';

import {ReactiveFormsModule,FormsModule  } from '@angular/forms';
import { ComponentsModule } from '../components/components.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { DataTablesModule } from 'angular-datatables';


@NgModule({
  declarations: [LicenseHolderComponent,NgbdSortableHeader],
  exports:[LicenseHolderComponent],
  imports: [
    RouterModule.forChild(LicenseHolderRoutes),
    CommonModule,
    NgxLoadingModule.forRoot({}),
    NgbModule,
    Ng2SearchPipeModule,
    NgMultiSelectDropDownModule.forRoot(),
    //MenuListModule, 
    //TokenInfoModule
    //PlayerLogModule
     
    ComponentsModule,
    ReactiveFormsModule,
    FormsModule,
    DataTablesModule
  ]
})
export class LicenseHolderModule { }
