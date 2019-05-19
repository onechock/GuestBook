import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { AuthGuard } from './guards/auth.guard';
import { GuestBookComponent } from './guest-book/guest-book.component';
import { UserCreateComponent } from './user-create/user-create.component';

const appRoutes: Routes = [
    { path: '', component: GuestBookComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent},
	{ path: 'user-create', component: UserCreateComponent },

    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

export const routing = RouterModule.forRoot(appRoutes);