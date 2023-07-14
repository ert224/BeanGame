using UnityEngine;

[System.Serializable]
public struct CardsTemplate
{
    public string type;
    public int total;
    public GameObject prefab;
    public ushort value;
    public CardsTemplate(string type, int total, GameObject prefab, ushort value)
    {
        this.type = type;
        this.total = total;
        this.prefab = prefab;
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

    public GameObject GetPrefab()
    {
        return this.prefab;
    }
    public ushort GetValue()
    {
        return this.value;
    }
}
