using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public Image icon;

    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public GameObject selectedShader;
    public bool thisItemSelected;
    public InventoryManager inventoryManager;

    [SerializeField]
    private TMP_Text quantityText;

    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;
    public Sprite emptySprite;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        itemDescriptionImage = inventoryManager.itemDescriptionImage;
        itemDescriptionNameText = inventoryManager.itemDescriptionNameText;
        itemDescriptionText = inventoryManager.itemDescriptionText;

    }
    public void AddItem(string newItemName, int Quantity, string itemDescription, Sprite ItemSprite)
    {
        Debug.Log($"[addItem] Slot sekarang: {itemName}, item baru: {newItemName}");

        if (string.IsNullOrEmpty(itemName))
        {
            itemName = newItemName;
            quantity = Quantity;
            itemSprite = ItemSprite;
            this.itemDescription = itemDescription;
            UpdatingUI();
        } else if (itemName == newItemName)
        {
            quantity += Quantity;
            UpdatingUI();
            return;
        } else
        {
            Debug.LogWarning("Slot is full with a different item.");
            UpdatingUI();
            return;
        }
    }

    public void OnLeftClick()
    {
        Debug.Log($"inventoryManager null? {inventoryManager == null}, selectedShader null? {selectedShader == null}");
        Debug.Log($"UI target: {itemDescriptionImage.gameObject.name}, activeInHierarchy: {itemDescriptionImage.gameObject.activeInHierarchy}");
        Debug.Log($"Text name object: {itemDescriptionNameText.gameObject.name}, activeInHierarchy: {itemDescriptionNameText.gameObject.activeInHierarchy}");
        Debug.Log($"klik item: {itemName}, desc: {itemDescription}, sprite null? {itemSprite == null}");
        inventoryManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        if(itemDescriptionImage.sprite == null)
        {
            itemDescriptionImage.sprite = emptySprite;
        } else
        {
            itemDescriptionImage.sprite = itemSprite;
        }
        itemDescriptionNameText.text = itemName;
        itemDescriptionText.text = itemDescription;
    }
    public void UpdatingUI()
    {
        Debug.Log($"UpdateUI {itemName} | icon: {icon.name} | sprite masuk: {itemSprite}");
        Debug.Log($"UpdateUI {itemName} qty: {quantity}, quantityText is null? {quantityText == null}");
        icon.sprite = itemSprite;
        icon.enabled = true;
        quantityText.text = quantity.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
    }
}
