using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;
    public InputAction OpenInventory;
    public bool menuActivated;
    public GameObject InventoryMenu;

    public GameObject itemSlotPrefab;
    public Transform inventorySlotsParent;

    public static InventoryManager instance;
    private List<ItemSlot> itemSlots = new List<ItemSlot>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        Debug.Log("item count: " + instance.itemSlots.Count);
    }
    void Awake()
    {
        OpenInventory = new InputAction("OpenInventory", binding: "<Keyboard>/i");
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        OpenInventory.Enable();
        OpenInventory.performed += ToggleInventory;
    }

    void OnDisable()
    {
        OpenInventory.performed -= ToggleInventory;
        OpenInventory.Disable();
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        menuActivated = !menuActivated;
        InventoryMenu.SetActive(menuActivated);
        Time.timeScale = menuActivated ? 0f : 1f;
    }

    public void AddItem(string newItemName, int Quantity, string itemDescription, Sprite ItemSprite)
    {
        Debug.Log($"[AddItem] Request masuk: {newItemName}, qty: {Quantity}");

        foreach (var slot in instance.itemSlots)
        {
            if (!string.IsNullOrEmpty(slot.itemName) && slot.itemName.Equals(newItemName))
            {
                slot.AddItem(newItemName, Quantity, itemDescription, ItemSprite);
                Debug.Log($"stacked: {newItemName} x{Quantity}");
                return;
            }
            
        }
        foreach (var slot in instance.itemSlots)
        {
            if (string.IsNullOrEmpty(slot.itemName))
            {
                slot.AddItem(newItemName, Quantity, itemDescription, ItemSprite);
                Debug.Log($"Placed in an empty slot: {newItemName} x{Quantity}");
                return;
            }
            
        }

        Debug.Log("Slot full");
        GameObject newSlotObj = Instantiate(instance.itemSlotPrefab, instance.inventorySlotsParent);
        ItemSlot newSlot = newSlotObj.GetComponent<ItemSlot>();
        newSlot.AddItem(newItemName, Quantity, itemDescription, ItemSprite);
        instance.itemSlots.Add(newSlot);
    }
    public void DeselectAllSlots()
    {
        for(int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].selectedShader.SetActive(false);
            itemSlots[i].thisItemSelected = false;
        }
    }
}