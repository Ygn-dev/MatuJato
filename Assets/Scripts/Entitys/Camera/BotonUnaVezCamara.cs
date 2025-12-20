using UnityEngine;

public class BotonUnaVezCamara : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public CameraRespondTrigger camara;
    private bool usado = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (usado) return;

        if (other.CompareTag("Player"))
        {
            camara.Girar(-90f, 3.5f);
            usado = true;
        }
    }

    public void ResetBoton()
    {
        usado = false;
    }
}
