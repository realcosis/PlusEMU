namespace Plus.Communication.Interfaces;

public interface IServerPacket
{
    int Id { get; }
    byte[] GetBytes();
}