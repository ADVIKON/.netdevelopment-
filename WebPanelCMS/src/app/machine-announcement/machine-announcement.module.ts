import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NgxLoadingModule  } from 'ngx-loading';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import {ReactiveFormsModule,FormsModule  } from '@angular/forms';
import { ComponentsModule } from '../components/components.module';
import { MachineAnnouncementComponent } from './machine-announcement.component';
import { MachineAnnouncementRoutes } from './machine-announcement.routes';

@NgModule({
  declarations: [MachineAnnouncementComponent],
  exports:[MachineAnnouncementComponent],
  imports: [
    RouterModule.forChild(MachineAnnouncementRoutes),
    CommonModule,
    NgxLoadingModule.forRoot({}),
    NgbModule,
    ComponentsModule,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class MachineAnnouncementModule { }