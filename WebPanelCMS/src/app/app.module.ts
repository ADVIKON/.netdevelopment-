import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';


import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { AuthGuard } from './auth/auth.guard';



@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({positionClass: 'toast-top-center'}),
    NgbModule,
    AppRoutingModule ,
      ],
  providers: [AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
