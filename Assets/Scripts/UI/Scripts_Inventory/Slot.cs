
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;
    public bool empty = true;
    public Button button;

    public Transform slotIcon;

    public ItemDetailsPanel itemDetailsPanel;

    private void Awake()
    {
        slotIcon = transform.GetChild(0);//El Panel del Slot
        button = GetComponent<Button>();

        button.onClick.AddListener(OnSlotClicked);
    }
    public void UpdateSlot()
    {
        if (slotIcon == null) slotIcon = transform.GetChild(0);
        slotIcon.GetComponent<Image>().sprite = item.sprite;
    }


    void OnSlotClicked()
    {
        if (empty) return;
        itemDetailsPanel.ShowDetails(item);
    }
}
