using UnityEngine;

public class CameraRespondTrigger : MonoBehaviour
{
    public bool activated = false;
    public Respawn respawn;

    private float startZ;
    private float targetZ;
    private float rotationTimer;
    private float initialZ;
    private float rotationDuration;
    private Vector3 positionBuff;
    private Quaternion rotationBuff;

    void Start()
    {
        initialZ = transform.eulerAngles.z;
        respawn.respawnEvent += respawnEvento;
        positionBuff = transform.position;
        rotationBuff = transform.rotation;
    }

    public void Girar(float rotationAmount, float rotaDuration)
    {
        if (!activated)
        {
            SoundFXManager.instance.PlaySound(SoundType.ENTE_MOVIENDO, 0.6f);
            activated = true;
            startZ = transform.eulerAngles.z;
            rotationDuration = rotaDuration;
            targetZ = startZ + rotationAmount;
            rotationTimer = 0f;
        }
    }

    public void Update()
    {
        if (rotationTimer < rotationDuration && activated)
        {
            rotationTimer += Time.deltaTime;
            float t = Mathf.Clamp01(rotationTimer / rotationDuration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            float newZ = Mathf.LerpAngle(startZ, targetZ, smoothT);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, newZ);
        }
        if (rotationTimer >= rotationDuration)
        {
            activated = false;
        }
    }

    private void respawnEvento()
    {
        transform.position = positionBuff;
        transform.rotation = rotationBuff; 
    }

    public void ResetEstado()
    {/*
        activated = false;
        rotationTimer = 0f;

        transform.rotation = Quaternion.Euler(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            initialZ
        );*/
    }
}
