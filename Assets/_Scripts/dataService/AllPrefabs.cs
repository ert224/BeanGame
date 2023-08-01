using UnityEngine;

[CreateAssetMenu(fileName = "AllPrefabs", menuName = "ScriptableObjects/AllPrefabs", order = 1)]
public class AllPrefabs : ScriptableObject
{
    [SerializeField, Tooltip("Card prefab")] private GameObject coffee;
    [SerializeField, Tooltip("Card prefab")] private GameObject wax;
    [SerializeField, Tooltip("Card prefab")] private GameObject blue;
    [SerializeField, Tooltip("Card prefab")] private GameObject chili;
    [SerializeField, Tooltip("Card prefab")] private GameObject stink;
    [SerializeField, Tooltip("Card prefab")] private GameObject green;
    [SerializeField, Tooltip("Card prefab")] private GameObject soy;
    [SerializeField, Tooltip("Card prefab")] private GameObject black;
    [SerializeField, Tooltip("Card prefab")] private GameObject red;

    public GameObject ReturnPrefab(string popType)
    {
        return getCardType(popType);
    }
    private GameObject getCardType(string popType)
    {
        switch (popType)
        {
            case "coffee":
                return coffee;
            case "wax":
                return wax;
            case "blue":
                return blue;
            case "chili":
                return chili;
            case "stink":
                return stink;
            case "green":
                return green;
            case "soy":
                return soy;
            case "black":
                return black;
            case "red":
                return red;
            default:
                return null;
        }
    }
}
