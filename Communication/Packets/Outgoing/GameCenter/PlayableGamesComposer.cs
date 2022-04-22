namespace Plus.Communication.Packets.Outgoing.GameCenter
{
    class PlayableGamesComposer : ServerPacket
    {
        public PlayableGamesComposer(int gameId)
            : base(ServerPacketHeader.PlayableGamesMessageComposer)
        {
            WriteInteger(gameId);
            WriteInteger(0);
        }
    }
}
