using UnityEngine;
using UnityEngine.UI;

public class CollectibleItems : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private int quantity = 1;
    [TextArea][SerializeField] private string itemDescription;

    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogError("Inventory Manager has not assigned to the scene");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inventoryManager.AddItem(itemName, quantity, itemDescription, itemIcon);
            Destroy(gameObject);
        }
    }
}