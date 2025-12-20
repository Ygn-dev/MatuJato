using UnityEngine;
using System.Collections;//Usar la corrutina "IEnumerator"
public class CamaraSwitchTemporizado : MonoBehaviour
{
    public CamaraLightController[] camarasToToggle;
    //private bool isActive = false; // estado del switch
    private bool isRunning = false;
    public float activeTime = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isRunning)
        {
            StartCoroutine(ToggleTemporal());
        }
    }

    private IEnumerator ToggleTemporal()
    {
        isRunning = true;

        // Guarda el estado original de cada cámara
        bool[] estadosOriginales = new bool[camarasToToggle.Length];
        for (int i = 0; i < camarasToToggle.Length; i++)
        {
            if (camarasToToggle[i] != null)
                estadosOriginales[i] = camarasToToggle[i].GetIsActive();
        }

        // Invierte el estado de cada cámara
        foreach (var cam in camarasToToggle)
        {
            if (cam != null)
                cam.Toggle(!cam.GetIsActive());
        }

        // Espera el tiempo definido
        yield return new WaitForSeconds(activeTime);

        // Restaura cada cámara a su estado original
        for (int i = 0; i < camarasToToggle.Length; i++)
        {
            if (camarasToToggle[i] != null)
                camarasToToggle[i].Toggle(estadosOriginales[i]);
        }

        isRunning = false;
    }
}
