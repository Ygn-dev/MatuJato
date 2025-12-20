using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLightSwitch : MonoBehaviour
{
    public Light2D globalLight;

    [Header("Intensidades")]
    public float intensidadOscura = 0.3f;

    private float intensidadOriginal;
    private bool usado = false;

    [Header("Sonido")]
    public AudioSource audioSource;
    public AudioClip sonidoSwitch;

    private void Start()
    {
        // Guardamos la intensidad que ya tiene la luz
        intensidadOriginal = globalLight.intensity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (usado) return; //solo se usa una vez

        globalLight.intensity = intensidadOscura;
        if (audioSource != null && sonidoSwitch != null)
            audioSource.PlayOneShot(sonidoSwitch);
        usado = true;
    }
}

