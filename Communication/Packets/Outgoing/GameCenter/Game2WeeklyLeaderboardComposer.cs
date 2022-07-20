using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Games;
using Plus.HabboHotel.Users;

namespace Plus.Communication.Packets.Outgoing.GameCenter;

public class Game2WeeklyLeaderboardComposer : IServerPacket
{
    private readonly GameData _gameData;
    private readonly ICollection<Habbo> _habbos;

    public Game2WeeklyLeaderboardComposer(GameData gameData, ICollection<Habbo> habbos)
    {
        _gameData = gameData;
        _habbos = habbos;
    }

    public uint MessageId => ServerPacketHeader.Game2WeeklyLeaderboardComposer;

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(2014);
        packet.WriteInteger(41);
        packet.WriteInteger(0);
        packet.WriteInteger(1);
        packet.WriteInteger(1581);

        //Used to generate the ranking numbers.
        var num = 0;
        packet.WriteInteger(_habbos.Count); //Count
        foreach (var habbo in _habbos.ToList())
        {
            num++;
            packet.WriteInteger(habbo.Id); //Id
            packet.WriteInteger(habbo.FastfoodScore); //Score
            packet.WriteInteger(num); //Rank
            packet.WriteString(habbo.Username); //Username
            packet.WriteString(habbo.Look); //Figure
            packet.WriteString(habbo.Gender.ToLower()); //Gender .ToLower()
        }
        packet.WriteInteger(0); //
        packet.WriteInteger(_gameData.Id); //Game Id?
    }
}