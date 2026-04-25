using UnityEngine;
using UnityEngine.InputSystem;

public class MenuSelector : MonoBehaviour
{
    [Header("Elementos del menú")]
    public GameObject menuPausa;
    public GameObject diario;
    public GameObject inventario;
    public Inventory_Manager inventory_Manager;
    private int indiceElemento;
    private GameObject[] listaElementosMenu;

    private Vector3[] listaEscalas;

    [Header("Escala")]
    public float factorSeleccion = 2.0f;

    [Header("Input")]
    public InputActionReference move;
    public InputActionReference select;
    public InputActionReference cancel;

    private bool ejeBloqueado;

    void Awake()
    {
        listaElementosMenu = new GameObject[3]
        {
            diario,
            menuPausa,
            inventario
        };

        listaEscalas = new Vector3[listaElementosMenu.Length];

        for (int i = 0; i < listaElementosMenu.Length; i++)
        {
            listaEscalas[i] = listaElementosMenu[i].transform.localScale;
        }
    }

    void OnEnable()
    {
        indiceElemento = 1;
        ejeBloqueado = false;

        for (int i = 0; i < listaElementosMenu.Length; i++)
        {
            estadoNormal(i);
        }

        estadoSelecionado(indiceElemento);

        move.action.performed += OnMove;
        /* move.action.Enable(); */

        select.action.performed += OnSelect;
        /* select.action.Enable(); */

        cancel.action.performed += OnCancel;
        /* cancel.action.Enable(); */
    }

    void OnDisable()
    {
        move.action.performed -= OnMove;
        /* move.action.Disable(); */

        select.action.performed -= OnSelect;
        /* select.action.Disable(); */

        cancel.action.performed -= OnCancel;
        /* cancel.action.Disable(); */
    }
    void OnCancel(InputAction.CallbackContext context)
    {
        if (inventory_Manager.inventoryEnable)
        {
            inventory_Manager.CloseInventory();
            return;
        }
    }
    void OnSelect(InputAction.CallbackContext context)
    {
        if (inventory_Manager.inventoryEnable) return;
        confirmarSelecion();
    }
    void OnMove(InputAction.CallbackContext ctx)
    {
        if (inventory_Manager.inventoryEnable) return;

        Vector2 input = ctx.ReadValue<Vector2>();

        // cuando el eje vuelve al centro → desbloqueamos
        if (Mathf.Abs(input.x) < 0.2f)
        {
            ejeBloqueado = false;
            return;
        }

        if (ejeBloqueado) return;

        int indiceAnterior = indiceElemento;

        if (input.x < -0.5f && indiceElemento > 0)
            indiceElemento--;

        if (input.x > 0.5f && indiceElemento < listaElementosMenu.Length - 1)
            indiceElemento++;

        if (indiceAnterior != indiceElemento)
        {
            estadoNormal(indiceAnterior);
            estadoSelecionado(indiceElemento);
            ejeBloqueado = true;
        }
    }
    public void resetSelector()
    {
        indiceElemento = 1;
        for (int i = 0; i < listaElementosMenu.Length; i++)
        {
            estadoNormal(i);
        }
        estadoSelecionado(indiceElemento);
    }
    void estadoSelecionado(int idx)
    {
        listaElementosMenu[idx].transform.localScale = listaEscalas[idx] * factorSeleccion;
        CanvasGroup canvasGroup = listaElementosMenu[idx].GetComponent<CanvasGroup>();
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    void estadoNormal(int idx)
    {
        listaElementosMenu[idx].transform.localScale = listaEscalas[idx];
        CanvasGroup canvasGroup = listaElementosMenu[idx].GetComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    public void confirmarSelecion()
    {
        switch (indiceElemento)
        {
            case 0:
                Debug.Log("Abrir Diario");

                break;
            case 1:
                Debug.Log("Menú Pausa");
                break;
            case 2:
                Debug.Log("Abrir Inventario");
                inventory_Manager.OpenInventory();
                break;
        }
    }
}
