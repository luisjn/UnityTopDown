using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects/Item")]

public class ItemSO : ScriptableObject
{
        public float damage;
        public string itemName;
        public int requiredLevel;
        public Sprite icon;
}