import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountRoutningModule } from './account-routning.module';
import { SharedModule } from '../shared/shared.module';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';



@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    CommonModule,
    AccountRoutningModule,
    SharedModule
    ]
})
export class AccountModule { }
