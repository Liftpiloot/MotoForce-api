namespace Interface.Models;

public class FriendModel
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public bool Accepted { get; set; }
    public DateTime CreatedAt { get; set; }
}