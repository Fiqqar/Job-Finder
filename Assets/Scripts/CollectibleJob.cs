using UnityEngine;

public class CollectibleJob : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] public int quantity = 1;
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
            collect.AddJob();
            Destroy(gameObject);
        }
    }
}
