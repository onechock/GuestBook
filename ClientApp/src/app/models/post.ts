import { Like } from "./like";

export class Post {
	id: number;
	date: Date;
	userId: number;
	publisher: string;
	text: string;
	likes: Like[];
	liked: boolean;
	isOwner: boolean;
}