using NUnit.Compatibility;
using UnityEngine;

public class PlayFootsteps : MonoBehaviour
{
    private AudioSource audioSource;
    private Vector2 previousMoveInput = Vector2.zero;

    void Update()
    {
        Vector2 currentMoveInput = GetComponent<PlayerController>().moveInputVector;   


        // Detecta cambio de estado: quieto -> movimiento
        if (previousMoveInput == Vector2.zero && currentMoveInput != Vector2.zero)
        {
            audioSource = SoundFXManager.instance.GetRandomClipWithPitch(SoundType.FOOTSTEP);
            audioSource.volume = 0.2f;
            audioSource.loop = true;
            audioSource.Play();
        }

        // Detecta cambio de estado: movimiento -> quieto
        else if (previousMoveInput != Vector2.zero && currentMoveInput == Vector2.zero)
        {
            Destroy(audioSource.gameObject);
        }
        // Actualiza el estado anterior
        previousMoveInput = currentMoveInput;
    }
}
