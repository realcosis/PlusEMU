using System.Collections.Generic;

using Plus.HabboHotel.Games;

namespace Plus.Communication.Packets.Outgoing.GameCenter
{
    class GameListComposer : ServerPacket
    {
        public GameListComposer(ICollection<GameData> games)
            : base(ServerPacketHeader.GameListMessageComposer)
        {
            WriteInteger(PlusEnvironment.GetGame().GetGameDataManager().GetCount());//Game count
            foreach (GameData game in games)
            {
                WriteInteger(game.Id);
               WriteString(game.Name);
               WriteString(game.ColourOne);
               WriteString(game.ColourTwo);
               WriteString(game.ResourcePath);
               WriteString(game.StringThree);
            }
        }
    }
}
