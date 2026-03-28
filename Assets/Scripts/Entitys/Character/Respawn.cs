using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class Respawn : MonoBehaviour
{
    public Transform characterTransform;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public InputActionReference move;
    [HideInInspector] public Vector3 respawnPoint;
    public event Action respawnEvent;

    private bool estaEnAnimacion = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("DeathZone") && !estaEnAnimacion)
        {
            SoundFXManager.instance.PlaySound(SoundType.GETTING_HIT,0.5f);
            estaEnAnimacion = true;
            move.action.Disable();
            StartCoroutine(VibrateRoutine());
            animator.SetTrigger("Respawn");
            StartCoroutine(PlayArrugadoSound());
        }
    }

    private IEnumerator PlayArrugadoSound()
    {
        yield return new WaitForSeconds(0.2f);
        SoundFXManager.instance.PlayRandomPitch(SoundType.ARRUGADO);
    }
    
    public void RespawnCharacter()
    {
        respawnEvent?.Invoke();
        characterTransform.position = respawnPoint;
        characterTransform.GetComponent<PlayerController>().FirstAnim();
        estaEnAnimacion = false;
    }

    private IEnumerator VibrateRoutine()
    {
        float dur = 0.15f;
        float amp = 0.08f;
        float freq = 45f;       // vibración X
        float blinkFreq = 25f;  // parpadeo (ajústalo)

        Vector3 restLocalPos = characterTransform.localPosition;

        // Color actual del sprite (para volver al final)
        Color originalColor = spriteRenderer.color;
        Color blinkColor = Color.black;

        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;

            // Vibración izquierda-derecha (onda cuadrada)
            float dir = Mathf.Sign(Mathf.Sin(t * Mathf.PI * 2f * freq)); // -1 o +1
            characterTransform.localPosition = restLocalPos + Vector3.right * (dir * amp);

            // Parpadeo entre color original y blinkColor
            bool useOriginal = Mathf.Sin(t * Mathf.PI * 2f * blinkFreq) > 0f;
            spriteRenderer.color = useOriginal ? originalColor : blinkColor;

            yield return null;
        }

        // Restaurar
        characterTransform.localPosition = restLocalPos;
        spriteRenderer.color = originalColor;
    }
}
