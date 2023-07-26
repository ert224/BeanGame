using UnityEngine;
using Unity.Netcode;

public struct SerlializedCard : INetworkSerializable
{
    public string type;
    public int total;
    public ushort value;

    public SerlializedCard(string type, int total, ushort value)
    {
        this.type = type; 
        this.total = total;
      this.value = value;
    }

    public string GetCardType()
    {
        return this.type;
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
        serializer.SerializeValue(ref total);
        serializer.SerializeValue(ref value);
    }
}
