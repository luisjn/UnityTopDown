using UnityEngine;

public class Item : MonoBehaviour, Interactive
{
    [SerializeField] private ItemSO data;
    [SerializeField] private GameManagerSO gameManager;
    
    public void Interact()
    {
        gameManager.Inventory.NewItem(data);
        Destroy(this.gameObject);
    }
}