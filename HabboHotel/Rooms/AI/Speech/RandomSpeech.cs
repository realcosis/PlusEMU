namespace Plus.HabboHotel.Rooms.AI.Speech
{
    public class RandomSpeech
    {
        public int BotId;
        public string Message;

        public RandomSpeech(string message, int botId)
        {
            this.BotId = botId;
            this.Message = message;
        }
    }
}