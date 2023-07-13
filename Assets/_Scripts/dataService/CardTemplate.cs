using UnityEngine;

[System.Serializable]
public struct CardsTemplate
{
    public string type;
    public int value;
    public GameObject prefab;

    public CardsTemplate(string type, int value, GameObject prefab)
    {
        this.type = type;
        this.value = value;
        this.prefab = prefab;
    }

    public string GetCardType()
    {
        return this.type;
    }

    public int GetValue()
    {
        return this.value;
    }

    public GameObject GetPrefab()
    {
        return this.prefab;
    }
}
