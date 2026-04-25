using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Inventory_Manager inventory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item itemData = collision.GetComponent<Item>();
            if (itemData != null)
            {
                inventory.AddItem(itemData);
            }
        }
    }
}
