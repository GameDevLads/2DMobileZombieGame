using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Crate Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public int weight;
    public int index;
}
