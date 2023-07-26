using UnityEngine;
using Unity.Netcode;

public struct SerializedCard : INetworkSerializable
{
    public string type;
    public string cardTag; 
    public int total;
    public ushort value;
    public SerializedCard(string type, string cardTag, int total, ushort value)
    {
        this.type = type; 
        this.total = total;
        this.value = value;
        this.cardTag = cardTag;
    }

    public string GetCardType()
    {
        return this.type;
    }
    public string GetCardTag()
    {
        return this.cardTag;
    }

    public int GetTotal()
    {
        return this.total;
    }


    public ushort GetValue()
    {
        return this.value;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref type);
        serializer.SerializeValue(ref cardTag);
        serializer.SerializeValue(ref total);
        serializer.SerializeValue(ref value);
    }
}
