using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public InputActionReference move;
    [SerializeField] private bool testing = false;
    [HideInInspector] public Vector2 moveInputVector;

    void Start()
    {
        if (testing)
        {
            move.action.actionMap.Enable();
        }
    }

    void Update()
    {
        moveInputVector = Vector4Direcciones();
        SetAnimations();
        Turn();
        saveData();
        loadData();
    }

    public void saveData()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SaveManager.SavePlayerData(this);
            Debug.Log("Datos guardados");
        }
    }
    public void loadData()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerData playerData = SaveManager.LoadPlayerData();
            speed = playerData.speed;
            transform.position = new Vector3(playerData.position[0], playerData.position[1], playerData.position[2]);
            Debug.Log("Datos cardados");
        }
    }

    public Vector2 Vector4Direcciones()
    {
        Vector2 direccion = move.action.ReadValue<Vector2>();
        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.y))
            direccion = new Vector2(Mathf.Sign(direccion.x), 0);
        else if (Mathf.Abs(direccion.y) > 0)
            direccion = new Vector2(0, Mathf.Sign(direccion.y));
        return direccion;
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(moveInputVector.x * speed, moveInputVector.y * speed);
    }

    void SetAnimations()
    {
        if (moveInputVector != Vector2.zero) GetComponent<Animator>().SetBool("isRunning", true);
        else GetComponent<Animator>().SetBool("isRunning", false);

        if (moveInputVector == Vector2.zero) return;
        GetComponent<Animator>().SetFloat("moveX", moveInputVector.x);
        GetComponent<Animator>().SetFloat("moveY", moveInputVector.y);
    }

    void Turn()
    {
        if (GetComponent<Rigidbody2D>().linearVelocity != Vector2.zero)
        {
            if (moveInputVector.x > 0) transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else if (moveInputVector.x < 0) transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public void FirstAnim()
    {
        GetComponent<Animator>().SetFloat("moveX", 0);
        GetComponent<Animator>().SetFloat("moveY", -1);
    }

    public void FirstAnim2()
    {
        moveInputVector = new Vector2(-1, 0);
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        GetComponent<Animator>().SetFloat("moveX", moveInputVector.x);
        GetComponent<Animator>().SetFloat("moveY", moveInputVector.y);
        StartCoroutine(GameInitiator.instance.continuacionCarga());
    }

    public void moverForzoso(float xval)
    {
        Vector3 newPos = new Vector3(xval, transform.position.y, transform.position.z);
        transform.position = newPos;
    }

    public void AbleControl()
    {
        move.action.Enable();
    }
}