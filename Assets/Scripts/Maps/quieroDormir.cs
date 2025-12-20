using UnityEngine;

public class quieroDormir : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = GameObject.FindGameObjectWithTag("MusicaZonaCamaras");
        Destroy(obj);
        GameInitiator.instance.hayMusica = false;
    }
}
