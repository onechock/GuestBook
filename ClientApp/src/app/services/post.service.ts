import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { User } from '../models/user';
import { Post } from '../models/post';
import { NewPost } from '../models/new-post';
import { Like } from '../models/like';

@Injectable({ providedIn: 'root' })
export class PostService {
	constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

	getPosts(user: User) {
		if (user) {
			return this.http.get<Post[]>(`${this.baseUrl}api/GuestBook/Posts/${user.id}`, {});
		}
		else {
			return this.http.get<Post[]>(`${this.baseUrl}api/GuestBook/Posts`, {});
		}
	}

	add(newPost: NewPost) {
		return this.http.put<any>(`${this.baseUrl}api/GuestBook/NewPost`, newPost);
	}

	like(like: Like) {
		return this.http.put<any>(`${this.baseUrl}api/GuestBook/LikePost`, like);
	}

	update(newPost: NewPost) {
		return this.http.put<any>(`${this.baseUrl}api/GuestBook/EditPost`, newPost);
	}

	delete(id: number) {
		return this.http.put<any>(`${this.baseUrl}api/GuestBook/DeletePost/${id}`, {});
	}
}