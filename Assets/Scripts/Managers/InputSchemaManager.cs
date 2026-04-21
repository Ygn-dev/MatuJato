using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


public class InputSchemaManager : MonoBehaviour
{
    //SINGLETON
    public static InputSchemaManager Instance { get; private set; }
    
    //REFERENCIA AL ASSET DE INPUT ACTIONS
    public InputActionAsset actions;

    //EVENTO PARA NOTIFICAR CAMBIO DE ESQUEMA
    public static event Action<String> ChangedSchema;
    
    //HIDE IN INSPECTOR
    [HideInInspector] public String currentSchema;
    [HideInInspector] public bool isCursorMode;
     public GameObject defaultCovered = null;
 
    
    void Awake()
    {
        // Implementación del patrón Singleton
        if (Instance == null) Instance = this;

        GetFirstCurrentSchema();
        SuscribirTodasLasAcciones();
    }


    void Update()
    {
        ComprobarModoCursor();
    }


    private void ComprobarModoCursor()
    {
        if (isCursorMode)
        {
            Cursor.visible = true;
            return;
        }
        else
        {
            // Detectar movimiento del mouse
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            if (mouseDelta.x != 0 || mouseDelta.y != 0)
            {
                SetMouseMode(true);
            }
        }
    }


    private void GetFirstCurrentSchema()
    {
        if(Gamepad.current != null)  currentSchema = "Gamepad";
        else currentSchema = "Keyboard";
        isCursorMode = false;
    }


    private void SuscribirTodasLasAcciones()
    {
        foreach (var map in actions.actionMaps)
        {
            foreach (var action in map.actions)
            {
                action.started += OnAnyActionPerformed;
                action.performed += OnAnyActionPerformed;
            }
        }
    }
    

    private void OnAnyActionPerformed(InputAction.CallbackContext context)
    {
        if(esDrift(context)) return;
        if( currentSchema != GetSchema(context))
        {
            Debug.Log("Esquema de control cambiado a: " + GetSchema(context));
            currentSchema = GetSchema(context);
            ChangedSchema?.Invoke(currentSchema);
        }
        SetMouseMode(false);
    }


    private string GetSchema(InputAction.CallbackContext context)
    {
        var control = context.control;
        var Schema = context.action.GetBindingForControl(control).Value;
        string schemaString = Schema.ToString();
        int startIndex = schemaString.IndexOf("[;");
        startIndex += 2;
        int endIndex = schemaString.IndexOf("]", startIndex);
        return (endIndex != -1) ? schemaString.Substring(startIndex, endIndex - startIndex) : schemaString.Substring(startIndex);
    }

    private void SetMouseMode(bool esModoMouse)
    {
        if (esModoMouse)
        {
            // Activar modo cursor
            isCursorMode = true;

            // Cambiar el esquema a "Keyboard" si no lo está ya
            if(currentSchema != "Keyboard")
            {
                Debug.Log("Esquema de control cambiado a: Keyboard");
                currentSchema = "Keyboard";
                ChangedSchema?.Invoke(currentSchema);
            }

            // Mostrar el cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            EventSystem.current.SetSelectedGameObject(null);
            
            Debug.Log("Modo cursor activado");
        }
        else
        {
            // Si ya estamos en ModoCursor = false, no hacemos nada
            if(!isCursorMode) return; 
            

            List<GameObject> hoveredObjects = GetAllUIUnderMouse();
            if(hoveredObjects.Count > 0)
            {
                GameObject currentHovered = GetTopLevelUIInteractable(hoveredObjects);
                if(currentHovered != null)
                {
                    EventSystem.current.SetSelectedGameObject(currentHovered);
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(defaultCovered);
                }
            }
            //Si no hay objetos bajo el mouse
            else
            {
                EventSystem.current.SetSelectedGameObject(defaultCovered);
            }

            // Desactivar modo cursor
            isCursorMode = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;


            Debug.Log("Modo cursor desactivado");
        }
    }

    private GameObject GetTopLevelUIInteractable(List<GameObject> hoveredObjects)
{
    // Componentes UI interactuables que nos interesan
    Type[] interactableTypes = new Type[]
    {
        typeof(Button),
        typeof(Toggle),
        typeof(Slider),
        typeof(Dropdown),
        typeof(TMP_Dropdown),
        typeof(InputField),
        typeof(TMP_InputField),
        typeof(Scrollbar),
        typeof(ScrollRect),
    };

    foreach (GameObject obj in hoveredObjects)
    {
        foreach (Type type in interactableTypes)
        {
            if (obj.GetComponent(type) != null)
                return obj;
        }
    }

    return null;
}


    List<GameObject> GetAllUIUnderMouse()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        List<GameObject> objects = new List<GameObject>();

        foreach (var r in results)
        {
            objects.Add(r.gameObject);
        }

        return objects;
    }

    private bool esDrift(InputAction.CallbackContext context)
    {
        if (context.action.name == "Move")
        {
            Vector2 inputVector = context.ReadValue<Vector2>();
            return inputVector.magnitude < 0.1f; // Umbral para evitar drift
        }
        return false;
    }
}
