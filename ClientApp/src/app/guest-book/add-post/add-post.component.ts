import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

import { AlertService, PostService } from '../../services';
import { User, Post } from '../../models';
import { GuestBookComponent } from '../guest-book.component';

declare var $: any;

@Component({
  selector: 'app-add-post',
  templateUrl: './add-post.component.html'
})

export class AddPostComponent implements OnInit {
	addForm: FormGroup;
	currentUser: User;
	loading = false;
	submitted = false;

	constructor(
		private formBuilder: FormBuilder,
		private alertService: AlertService,
		private postService: PostService,
		private guestBook: GuestBookComponent
	) {
		this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
	}

	get f() { return this.addForm.controls; }

	ngOnInit() {
		this.newAddForm()
	}

	newAddForm() {
		this.addForm = this.formBuilder.group({
			id: 0,
			text: ['', [Validators.required, Validators.minLength(30)]],
			user: this.currentUser
		});
	}

	onSubmit() {
		this.submitted = true;

		if (this.addForm.invalid) {
			return;
		}

		this.loading = true;

		this.postService.add(this.addForm.value)
			.pipe(first())
			.subscribe(
				data => {
					let post: Post = data.post;
					this.alertService.success('Message added', true);
					this.hideModal();
					this.newAddForm();
					this.guestBook.addPost(post);
				},
				error => {
					this.alertService.error(error);
					this.loading = false;
				});
	}

	hideModal(): void {
		document.getElementById('addPostModalClose').click();
	}

}
