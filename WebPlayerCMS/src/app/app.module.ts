import { BrowserModule } from '@angular/platform-browser';
import { NgModule  } from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {ReactiveFormsModule,FormsModule  } from '@angular/forms';
import {ToastModule} from 'ng6-toastr';
import { NgxLoadingModule  } from 'ngx-loading';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap'; 


import { LoginComponent } from './login/login.component';
import { MenuListComponent } from './menu-list/menu-list.component';
import { CustomerRegistrationComponent } from './customer-registration/customer-registration.component';
import { LicenseHolderComponent } from './license-holder/license-holder.component';
import { TokenInfoComponent } from './token-info/token-info.component';
import { BestOfPlaylistComponent } from './best-of-playlist/best-of-playlist.component';
import { PlaylistLibraryComponent } from './playlist-library/playlist-library.component';
import { StoreAndForwardComponent } from './store-and-forward/store-and-forward.component';
import { AdComponent } from './ad/ad.component';
import { PrayerComponent } from './prayer/prayer.component';
import { CopyDataComponent } from './copy-data/copy-data.component';
import { LinksComponent } from './links/links.component';
import { OwlDateTimeModule, OwlNativeDateTimeModule,DateTimeAdapter, OWL_DATE_TIME_FORMATS, OWL_DATE_TIME_LOCALE } from 'ng-pick-datetime';
import { OwlMomentDateTimeModule,MomentDateTimeAdapter } from 'ng-pick-datetime-moment';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import {ConfigAPI} from './class/ConfigAPI';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { NgxUploaderModule  } from 'ngx-uploader';
import { from } from 'rxjs';
import { CustomerDashboardComponent } from './customer-dashboard/customer-dashboard.component';
import { InstantPlayComponent } from './instant-play/instant-play.component';
import { UserComponent } from './user/user.component';
import * as $ from 'jquery';
import { PlayerLogComponent } from './player-log/player-log.component';
import { UploadContentComponent } from './upload-content/upload-content.component';
import {FileUploadModule} from 'ng2-file-upload';
import { AdminLogsComponent } from './admin-logs/admin-logs.component';
import { NewPlaylistLibraryComponent } from './new-playlist-library/new-playlist-library.component';
import { AdPlaylistsComponent } from './ad-playlists/ad-playlists.component';
import { ReportComponent } from './report/report.component';
import { DataTablesModule } from 'angular-datatables';
import { RepTokenInfoComponent } from './rep-token-info/rep-token-info.component';
import { RepTokenPlayedSongComponent } from './rep-token-played-song/rep-token-played-song.component'; 
//FileSelectDirective
export const MY_CUSTOM_FORMATS = {
  parseInput: 'LL LT',
  fullPickerInput: 'LL LT',
  datePickerInput: 'DD-MMM-YYYY',
  timePickerInput: 'HH:mm',
  monthYearLabel: 'MMM YYYY',
  dateA11yLabel: 'LL',
  monthYearA11yLabel: 'MMM YYYY',
};

const appRoutes:Routes= [
  {path: '', component: LoginComponent } , 
  {path: 'MenuList',component: MenuListComponent},
  {path: 'Registration',component: CustomerRegistrationComponent},
  {path: 'LicenseHolderControl',component: LicenseHolderComponent},
  {path: 'BestOffPlaylist',component: BestOfPlaylistComponent},
  {path: 'PlaylistLibrary',component:PlaylistLibraryComponent },
  {path: 'StoreAndForward',component:StoreAndForwardComponent },
  {path: 'Ads',component:AdComponent },
  {path: 'Prayer',component:PrayerComponent },
  {path: 'CopyData',component:CopyDataComponent },
  {path: 'Reports',component:ReportComponent },
  {path: 'Links',component:LinksComponent },
  {path: 'InstantPlay',component: InstantPlayComponent},
  {path: 'Users',component: UserComponent},
  {path: 'Upload',component: UploadContentComponent},
  {path: 'AdminLogs',component: AdminLogsComponent},
  {path: 'PlaylistAds',component:AdPlaylistsComponent },
  {path: 'Dashboard',component:CustomerDashboardComponent },
  {path: '**',component: LoginComponent},
  {path: '',redirectTo:'',pathMatch:'full'},
]
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    MenuListComponent,
    CustomerRegistrationComponent,
    LicenseHolderComponent,
    TokenInfoComponent,
    BestOfPlaylistComponent,
    PlaylistLibraryComponent,
    StoreAndForwardComponent,
    AdComponent,
    PrayerComponent,
    CopyDataComponent,
    LinksComponent,
    CustomerDashboardComponent,
    InstantPlayComponent,
    UserComponent,
    PlayerLogComponent,
    UploadContentComponent,
    AdminLogsComponent,
    NewPlaylistLibraryComponent,
    AdPlaylistsComponent,
    ReportComponent,
    RepTokenInfoComponent,
    RepTokenPlayedSongComponent,
    //FileSelectDirective
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ToastModule.forRoot(),
    NgxLoadingModule.forRoot({}),
    
    NgbModule,
     
    RouterModule.forRoot(appRoutes),
    OwlDateTimeModule, 
    OwlNativeDateTimeModule,
    OwlMomentDateTimeModule,
    NgMultiSelectDropDownModule.forRoot(),
    FormsModule ,
    HttpClientModule,
    Ng2SearchPipeModule,
    NgxUploaderModule ,
     FileUploadModule,
     DataTablesModule,
  ],
  providers: [
    {provide: DateTimeAdapter, useClass: MomentDateTimeAdapter, deps: [OWL_DATE_TIME_LOCALE]},
    {provide: OWL_DATE_TIME_FORMATS, useValue: MY_CUSTOM_FORMATS},ConfigAPI,
     {provide: LocationStrategy, useClass: HashLocationStrategy},
    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
 