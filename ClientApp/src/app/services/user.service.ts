import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { User } from '../models/user';

@Injectable({ providedIn: 'root' })
export class UserService {
	constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

    getAll() {
		return this.http.get<User[]>(`${this.baseUrl}api/Auth/GetUsers`);
	}

	getById(id: number) {
		return this.http.get(`${this.baseUrl}api/Auth/GetUser/${id}`);
	}

	register(user: User) {
		return this.http.post(`${this.baseUrl}api/Auth/CreateUser`, user);
	}

	update(user: User) {
		return this.http.put(`${this.baseUrl}api/Auth/UpdateUser/${user.id}`, user);
	}

	delete(id: number) {
		return this.http.delete(`${this.baseUrl}api/Auth/DeleteUser/${id}`);
	}
}