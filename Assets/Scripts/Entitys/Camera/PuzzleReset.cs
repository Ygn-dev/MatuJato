using UnityEngine;

public class PuzzleReset : MonoBehaviour
{
    public CameraRespondTrigger[] camaras;
    public BotonUnaVezCamara[] botones;

    public CamaraSwitch[] switches;

    public void ResetPuzzle()
    {
        foreach (var cam in camaras)
            cam.ResetEstado();

        foreach (var boton in botones)
            boton.ResetBoton();

        foreach (var s in switches)
            s.ResetSwitch();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            ResetPuzzle();
    }
}
