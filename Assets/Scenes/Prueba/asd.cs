using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class asd : MonoBehaviour
{
    public InputActionAsset inputActions;
    private InputActionMap gameplay;
    public Button button;


    void Awake()
    {
        gameplay = inputActions.FindActionMap("MainMenu");
        gameplay.Enable();
        InputSchemaManager.Instance.defaultCovered = button.gameObject;
    }
}
