import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NgxLoadingModule  } from 'ngx-loading';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import {ReactiveFormsModule,FormsModule  } from '@angular/forms';
import { ComponentsModule } from '../components/components.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { KeyboardannouncementComponent } from './keyboardannouncement.component';
import { KeyboardAnnouncementRoutes } from './keyboardannouncement.routes';

@NgModule({
    declarations: [KeyboardannouncementComponent],
    exports:[KeyboardannouncementComponent],
    imports: [
      RouterModule.forChild(KeyboardAnnouncementRoutes),
      CommonModule,
      NgxLoadingModule.forRoot({}),
      NgbModule,
      ComponentsModule,
      ReactiveFormsModule,
      FormsModule,
      NgMultiSelectDropDownModule.forRoot()
    ]
  })
  export class KeyboardAnnouncementModule { }