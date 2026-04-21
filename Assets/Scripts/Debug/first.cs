using UnityEngine;

public class first : MonoBehaviour
{
    public GameObject botonInicial;

    void OnEnable()
    {
        InputSchemaManager.Instance.defaultCovered = botonInicial;
    }

    void ODisable()
    {
        InputSchemaManager.Instance.defaultCovered = null;
    }
}
