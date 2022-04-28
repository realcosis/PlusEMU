namespace Plus.HabboHotel.Users.Messenger;

public class MessengerRequest
{
    public string Username { get; set; }

    public int ToId { get; set; }

    public int FromId { get; set; }
}