using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class ItemDetailsPanel : MonoBehaviour
{
    [Header("UI")]
    public Image icon;
    public TMP_Text itemName;
    public TMP_Text description;
    [Header("Input")]
    public InputActionReference cancel;
    private void Awake()
    {
        Close();
    }
    void OnEnable()
    {
        cancel.action.performed += OnCancel;
    }
    void OnDisable()
    {
        cancel.action.performed -= OnCancel;
    }
    void OnCancel(InputAction.CallbackContext context)
    {
        Close();
    }
    public void ShowDetails(Item item)
    {
        icon.sprite = item.sprite;
        itemName.text = item.name;
        description.text = item.description;
        gameObject.SetActive(true);
        gameObject.transform.SetAsLastSibling();
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
