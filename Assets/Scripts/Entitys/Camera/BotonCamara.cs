using UnityEngine;

public class BotonCamara : MonoBehaviour
{
    public Collider2D triggerArea;
    public CameraRespondTrigger camaraScript;
    public DatosNivel datosNivel;
    public Respawn respawn;
    public bool antigirarxd;
    
    private bool antiGirarBuff;
    private bool alternarBuff;

    void Start()
    {
        respawn.respawnEvent += Reset;
        antiGirarBuff = antigirarxd;
        alternarBuff = datosNivel.alternar;
    }

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
    

    private void Reset()
    {
        datosNivel.alternar = alternarBuff;
        antigirarxd = antiGirarBuff;
    }
}
