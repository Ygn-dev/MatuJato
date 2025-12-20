using UnityEngine;
using UnityEngine.InputSystem;

public class Interactuar : MonoBehaviour
{
    public InputActionReference interact;
    private bool isInRange = false;
    private Collider2D triggerCollider;
    private bool isDialouge = false;
    private bool isDoor = false;
    private bool isClossedDoor = false;
    

    void Start()
    {
        interact.action.performed += OnInteract;
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        if (isInRange)
        {
            if (isDialouge) triggerCollider.GetComponent<DialogueTrigger>().TriggerDialogue();
            if (isDoor) triggerCollider.GetComponent<TransicionNiveles>().CambiarNivel();
            if (isClossedDoor)
            {
                triggerCollider.GetComponent<TransicionNiveles>().siempreCerrado();
                return;
            }
            triggerCollider.GetComponent<BotonInteraccion>().OcultarBoton();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dialogue") || collision.CompareTag("Door") || collision.CompareTag("ClosedDoor")) 
        {
            triggerCollider = collision;
            isInRange = true;
            if (collision.CompareTag("Dialogue")) isDialouge = true;
            if (collision.CompareTag("Door")) isDoor = true;
            if (collision.CompareTag("ClosedDoor")) isClossedDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Dialogue") || collision.CompareTag("Door"))
        {
            triggerCollider = null;
            isInRange = false;
            isDialouge = false;
            isDoor = false;
        }
        
    }

    public void Respawn()
    {
        GetComponentInChildren<Respawn>().RespawnCharacter();
    }

    public void ReproducirAparicion()
    {
        SoundFXManager.instance.PlaySound(SoundType.REVIVE);
    }
}
