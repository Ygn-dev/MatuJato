using UnityEngine;

public class BotonCamara : MonoBehaviour
{
    public Collider2D triggerArea;
    public CameraRespondTrigger camaraScript;
    public DatosNivel datosNivel;

    public bool antigirarxd;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!camaraScript.activated)
            {
                if (datosNivel.alternar)
                {
                    if(antigirarxd) camaraScript.Girar(90f, 3.5f); 
                    else camaraScript.Girar(-90f, 3.5f);
                }
                else
                {
                    if(antigirarxd) camaraScript.Girar(-90f, 3.5f); 
                    else camaraScript.Girar(90f, 3.5f);
                }
                datosNivel.alternar = !datosNivel.alternar;
            }       
        }
    }
}
