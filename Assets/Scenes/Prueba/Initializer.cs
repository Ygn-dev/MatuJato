using UnityEngine;

public class Initializer : MonoBehaviour
{
    private static bool initialized = false;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]  //Se ejecuta antes de que se cargue cualquier escena
    public static void Init()
    {
        if (initialized) return;

        GameObject persistObject = Resources.Load<GameObject>("PERSIST_OBJECT");

        if (persistObject != null)
        {
            DontDestroyOnLoad(Instantiate(persistObject));
        }
        else
        {
            Debug.LogError("No se encontró PERSIST_OBJECT en Resources");
        }

        initialized = true;
    }
}
