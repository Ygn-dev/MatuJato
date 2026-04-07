using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSchemaHandler : MonoBehaviour
{
    //SINGLETON
    public static InputSchemaHandler Instance { get; private set; }
    public static event Action<String> ChangedSchema;
    public InputActionAsset actions;
    public String currentSchema;
    
    
    void Awake()
    {
        // Implementación del patrón Singleton
        if (Instance == null) Instance = this;
        currentSchema = GetFirstCurrentSchema();
        SuscribirTodasLasAcciones();
    }

    private string GetFirstCurrentSchema()
    {
        if(Gamepad.current != null)
        {
            return "Gamepad";
        }
        else
        {
            return "Keyboard";
        }
    }


    private void SuscribirTodasLasAcciones()
    {
        foreach (var map in actions.actionMaps)
        {
            foreach (var action in map.actions)
            {
                action.started += OnAnyActionPerformed;
            }
        }
    }
    

    private void OnAnyActionPerformed(InputAction.CallbackContext context)
    {
        if( currentSchema != GetSchema(context))
        {
            Debug.Log("Esquema de control cambiado a: " + GetSchema(context));
            currentSchema = GetSchema(context);
            ChangedSchema?.Invoke(currentSchema);
        }
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
}
