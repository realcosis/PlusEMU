namespace Plus.HabboHotel.GameClients
{
    public interface IIncomingPacket
    {
        int MessageId { get; set; }
        Memory<byte> Buffer { get; }
        byte ReadByte();
        short ReadShort();
        int ReadInt();
        bool ReadBool();
        string ReadString();
        bool HasDataRemaining();
        byte[] ReadFixedValue();
        void ReadBytes(Span<byte> destination);
    }
}