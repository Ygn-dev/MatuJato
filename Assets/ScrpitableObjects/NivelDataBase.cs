using UnityEngine;

[CreateAssetMenu(fileName = "NivelDataBase", menuName = "Scriptable Objects/NivelDataBase")]
public class NivelDataBase : ScriptableObject
{
    [Header("Información General")]
    public string nivelName;

    [Header("Personaje")]
    public Vector3 escalaPersonaje;
    
    [Header("Respawn Points")]
    public Vector3 respawnPoint;

    [Header("Cámara")]
    public float camaraZoom;
    public Vector2 screenPositionComposer;
    public bool esDeadZone;
    public Vector2 deadZoneWidthHeight;
    public float damping;
    public float slowingDistance;

    [Header("Confiner")]
    public GameObject confiner;
    public GameObject triggerDialogoInicial;
}
