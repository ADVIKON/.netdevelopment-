import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { LoginComponent } from './login/login.component';

import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import {ConfigAPI} from './class/ConfigAPI';
import { NgxLoadingModule  } from 'ngx-loading';

import {ReactiveFormsModule,FormsModule  } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';

const routes: Routes = [
 // {path: '', loadChildren:'./login/login.module#LoginModule' } , 
 { 
   path: 'Dashboard', 
   loadChildren: () => import('./customer-dashboard/customer-dashboard.module').then(m => m.CustomerDashboardModule)
  },
  { 
    path: 'LicenseHolderControl', 
    loadChildren: () => import('./license-holder/license-holder.module').then(m => m.LicenseHolderModule)
   },
   { 
    path: 'Registration', 
    loadChildren: () => import('./customer-registration/customer-registration.module').then(m => m.CustomerRegistrationModule)
   },
   { 
    path: 'PlaylistLibrary', 
    loadChildren: () => import('./playlist-library/playlist-library.module').then(m => m.PlaylistLibraryModule)
   },
   { 
    path: 'StoreAndForward', 
    loadChildren: () => import('./store-and-forward/store-forward.module').then(m => m.StoreAndForwardModule)
   },
   { 
    path: 'CopyData', 
    loadChildren: () => import('./copy-data/copy-data.module').then(m => m.CopyDataModule)
   },
   { 
    path: 'Links', 
    loadChildren: () => import('./links/links.module').then(m => m.LinksModule)
   },
   { 
    path: 'AdminLogs', 
    loadChildren: () => import('./admin-logs/admin-logs.module').then(m => m.AdminLogsModule)
   },
   { 
    path: 'InstantPlay', 
    loadChildren: () => import('./instant-play/instant-play.module').then(m => m.InstantPlayModule)
   },
   { 
    path: 'Users', 
    loadChildren: () => import('./user/user.module').then(m => m.UserModule)
   },
   { 
    path: 'Upload', 
    loadChildren: () => import('./upload-content/upload-content.module').then(m => m.UploadContentModule)
   },
   { 
    path: 'Ads', 
    loadChildren: () => import('./ad/ad.module').then(m => m.AdModule)
   },
   { 
    path: 'PlaylistAds', 
    loadChildren: () => import('./ad-playlists/ad-playlists.module').then(m => m.AdPlaylistsModule)
   },
   { 
    path: 'Reports', 
    loadChildren: () => import('./report/report.module').then(m => m.ReportModule)
   },
   { 
    path: 'Streaming', 
    loadChildren: () => import('./streaming/streaming.module').then(m => m.StreamingModule)
   },
   {
    path: '',
    redirectTo: '',
    pathMatch: 'full',
    component:LoginComponent

  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes,{
    preloadingStrategy:PreloadAllModules
  }),
  
  
  CommonModule,
  HttpClientModule,
  NgxLoadingModule.forRoot({}),
  ReactiveFormsModule,
    FormsModule,
    ToastrModule.forRoot({positionClass: 'toast-top-center'}),
    
],

  exports: [RouterModule],

  declarations:[LoginComponent],

  providers: [ConfigAPI]
  
})
export class AppRoutingModule { }
