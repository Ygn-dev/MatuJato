using System.Drawing;
using UnityEngine;

public class MapTransition : MonoBehaviour {
    public ScriptableObject nivelData;
    public Respawn character;
    public GameObject[] newMapTriggers;


    private float xCameraPosition;
    private float yCameraPosition;  
    private float sizeCamera;
    private float xRespawn;
    private float yRespawn;
    

    public void Start() {
        NivelDataBase data = (NivelDataBase)nivelData;
        sizeCamera = data.camaraZoom;
        xRespawn = data.respawnPoint.x;
        yRespawn = data.respawnPoint.y;
    }
    
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            Camera.main.transform.position = new Vector3(xCameraPosition, yCameraPosition, -10f);
            Camera.main.orthographicSize = sizeCamera;
            character.respawnPoint = new Vector3(xRespawn, yRespawn, character.respawnPoint.z);
            foreach (GameObject trigger in newMapTriggers)
            {
                trigger.SetActive(true);
            }
            gameObject.SetActive(false);
        }
    }
}
