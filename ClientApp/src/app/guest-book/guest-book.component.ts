import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

import { User, Post, Like } from '../models';
import { UserService, PostService, AuthenticationService, AlertService } from '../services'; import { DatePipe } from '@angular/common';


@Component({
	selector: 'app-guest-book-posts',
	templateUrl: './guest-book.component.html',
	styleUrls: ['./guest-book.component.css']
})
export class GuestBookComponent implements OnInit {
	public posts: Post[];
	public currentUser: User;

	editForm: FormGroup;
	loading = false;
	submitted = false;

	constructor(
		private formBuilder: FormBuilder,
		private authService: AuthenticationService,
		private postService: PostService,
		private alertService: AlertService
	) { }

	get f() { return this.editForm.controls; }

	ngOnInit() {
		this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
		this.postService.getPosts(this.currentUser)
			.subscribe(posts => {
				this.posts = posts;
				console.log(posts)
			}, err => {
				console.log(err)
				});

		this.editForm = this.formBuilder.group({
			id: 0,
			text: ['', [Validators.required, Validators.minLength(30)]],
			user: this.currentUser
		});
	}

	addPost(post: Post) {
		this.posts.unshift(post)
	}

	deletePost() {
		this.postService.delete(this.editForm.value.id)
			.pipe(first())
			.subscribe(
				data => {
					this.alertService.success('Message updated', true);
					this.hideModal();
					this.removePost(this.editForm.value.id);
				},
				error => {
					this.alertService.error(error);
					this.loading = false;
				});
	}

	updatePost(post: Post) {
		let updateIndex: number = this.posts.findIndex(function (p) { return p.id == post.id; })
		this.posts[updateIndex] = post;
	}

	removePost(id: number) {
		let removeIndex: number = this.posts.findIndex(function (p) { return p.id == id; })
		this.posts.splice(removeIndex, 1);
	}

	likePost(post: Post) {
		let like: Like = {
			id: 0,
			userId: this.currentUser.id,
			postId: post.id,
			name: `${this.currentUser.firstName} ${this.currentUser.lastName}`
		}
		this.postService.like(like)
			.pipe(first())
			.subscribe(data => {
				this.updatePost(data.post);
			}, err => {
				console.log(err)
			});
			
	}


	newEditForm(post: Post) {
		this.editForm = this.formBuilder.group({
			id: post.id,
			text: [post.text, [Validators.required, Validators.minLength(30)]],
			user: this.currentUser
		});

		document.getElementById('editPostModalOpen').click();
	}

	onSubmit() {
		this.submitted = true;

		if (this.editForm.invalid) {
			return;
		}

		this.loading = true;

		this.postService.update(this.editForm.value)
			.pipe(first())
			.subscribe(
				data => {
					let post: Post = data.post;
					this.alertService.success('Message updated', true);
					this.hideModal();
					this.updatePost(post);
				},
				error => {
					this.alertService.error(error);
					this.loading = false;
				});
	}

	editPost(post: Post) {
		this.newEditForm(post);
	}

	hideModal(): void {
		document.getElementById('editPostModalClose').click();
	}
}
