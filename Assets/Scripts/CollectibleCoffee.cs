using UnityEngine;
using UnityEngine.UI;
public class CollectibleCoffee : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] public int quantity = 1;
    [TextArea][SerializeField] private string itemDescription;

    private UICollect collect;
    private Health health;

    void Start()
    {
        collect = FindObjectOfType<UICollect>();
        health = FindObjectOfType<Health>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collect.AddCoffee();
            health.RegenHealth(5);
            Destroy(gameObject);
        }
    }

}
