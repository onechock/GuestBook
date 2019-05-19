/// <reference path = "app.routing.ts" />
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';


import { AppComponent } from './app.component';
import { routing } from './app.routing';

import { JwtInterceptor, ErrorInterceptor } from './helpers';
import { LoginComponent } from './login/login.component';
import { GuestBookComponent } from './guest-book/guest-book.component';
import { UserCreateComponent } from './user-create/user-create.component';
import { AddPostComponent } from './guest-book/add-post/add-post.component';

@NgModule({
	imports: [
		BrowserModule,
		ReactiveFormsModule,
		HttpClientModule,
		routing
	],
	declarations: [
		AppComponent,
		GuestBookComponent,
		LoginComponent,
		UserCreateComponent,
		AddPostComponent
	],
	providers: [
		{ provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
		{ provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
	],
	bootstrap: [AppComponent]
})

export class AppModule { }