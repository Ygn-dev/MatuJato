using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Manager : MonoBehaviour
{
    public bool inventoryEnable = false;
    public GameObject inventory;
    [Header("Slots")]
    private int allSlots;
    private int enableSlots;
    private GameObject[] arraySlot;
    public GameObject slotHolder;
    [Header("Animation")]
    public AnimationCurve curvaAparicion;
    public float duracionAnimacion = 0.25f;
    private CanvasGroup canvasGroup;
    void Start()
    {
        canvasGroup = inventory.GetComponent<CanvasGroup>();
        allSlots = slotHolder.transform.childCount;
        arraySlot = new GameObject[allSlots];
        for (int i = 0; i < allSlots; i++)
        {
            arraySlot[i] = slotHolder.transform.GetChild(i).gameObject;

            if (arraySlot[i].GetComponent<Slot>().item == null)
            {
                arraySlot[i].GetComponent<Slot>().empty = true;
            }

        }
        CloseInventoryInstant();//Start close
    }

    public void OpenInventory()
    {
        inventory.SetActive(true);
        inventory.transform.SetAsLastSibling();
        StartCoroutine(AnimationInventory(0, 1));
        inventoryEnable = true;
    }
    IEnumerator AnimationInventory(float alphaInicial, float alphaFinal)
    {
        float time = 0f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        while (time < duracionAnimacion)
        {
            time += Time.unscaledDeltaTime;
            float normalizado = time / duracionAnimacion;
            float curva = curvaAparicion.Evaluate(normalizado);

            canvasGroup.alpha = Mathf.Lerp(alphaInicial, alphaFinal, curva);

            yield return null;
        }
        canvasGroup.alpha = alphaFinal;

        if (alphaFinal == 0)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            inventory.SetActive(false);
        }
    }
    public void CloseInventory()
    {
        //inventory.SetActive(false);
        StartCoroutine(AnimationInventory(1, 0));
        inventoryEnable = false;
    }
    void CloseInventoryInstant()
    {
        inventory.SetActive(false);
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }


    public void AddItem(Item itemData)
    {
        for (int i = 0; i < allSlots; i++)
        {
            Slot slot = arraySlot[i].GetComponent<Slot>();
            if (slot.empty)
            {
                itemData.pickedUp = true;
                slot.item = itemData;

                //itemData.transform.parent = slot.transform;
                itemData.gameObject.SetActive(false);


                slot.UpdateSlot();


                slot.empty = false;
                break;//Para evitar que el objeto se llene en todos los slots vacios
            }
        }
    }
}
