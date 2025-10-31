using UnityEngine;
using UnityEngine.UI;
public class CollectibleCoffee : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private int quantity = 1;
    [TextArea][SerializeField] private string itemDescription;

    private UICollect collect;


    void Start()
    {
        collect = FindObjectOfType<UICollect>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collect.AddCoffee();
            Destroy(gameObject);
        }
    }
}
