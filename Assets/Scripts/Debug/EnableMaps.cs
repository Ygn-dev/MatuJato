using UnityEngine;
using UnityEngine.InputSystem;

public class EnableMaps : MonoBehaviour
{
    public InputActionAsset inputActions;
    private InputActionMap playerMap;

    void Awake()
    {
        playerMap = inputActions.FindActionMap("MainMenu");
    }

    void OnEnable()
    {
        playerMap.Enable(); // Activa el Action Map
    }

    void OnDisable()
    {
        playerMap.Disable(); // Lo desactiva
    }
}


