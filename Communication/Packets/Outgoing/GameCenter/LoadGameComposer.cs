using Plus.HabboHotel.Games;

namespace Plus.Communication.Packets.Outgoing.GameCenter;

internal class LoadGameComposer : ServerPacket
{
    public LoadGameComposer(GameData gameData, string ssoTicket)
        : base(ServerPacketHeader.LoadGameMessageComposer)
    {
        WriteInteger(gameData.Id);
        WriteString("1365260055982");
        WriteString(gameData.ResourcePath + gameData.Swf);
        WriteString("best");
        WriteString("showAll");
        WriteInteger(60); //FPS?
        WriteInteger(10);
        WriteInteger(8);
        WriteInteger(6); //Asset count
        WriteString("assetUrl");
        WriteString(gameData.ResourcePath + gameData.Assets);
        WriteString("habboHost");
        WriteString("http://fuseus-private-httpd-fe-1");
        WriteString("accessToken");
        WriteString(ssoTicket);
        WriteString("gameServerHost");
        WriteString(gameData.ServerHost);
        WriteString("gameServerPort");
        WriteString(gameData.ServerPort);
        WriteString("socketPolicyPort");
        WriteString(gameData.ServerHost);
    }
}