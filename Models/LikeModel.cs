namespace GuestBookCodeCase.Models
{
	public class LikeModel
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int PostId { get; set; }
		public string Name { get; set; }
	}
}