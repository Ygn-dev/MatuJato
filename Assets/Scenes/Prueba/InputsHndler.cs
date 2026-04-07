using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;

public class InputsHndler : MonoBehaviour
{
    public InputActionAsset actions;
    private String currentSchema;

    void Awake()
    {
        actions.FindActionMap("MainMenu").Enable();
        SuscribirTodasLasAcciones();
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

    private void OnAnyActionPerformed(InputAction.CallbackContext context)
    {
        if( currentSchema != GetSchema(context))
        {
            Debug.Log("Esquema de control cambiado a: " + GetSchema(context));
            currentSchema = GetSchema(context);
        }
    }
}

