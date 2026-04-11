using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Dynamc_Icon_Image : MonoBehaviour
{
    public InputActionReference actionReference;
    private Image image;


    void Awake() {
        image = GetComponent<Image>();
        InputSchemaManager.ChangedSchema += UpdateIcon;
        UpdateIcon(InputSchemaManager.Instance.currentSchema);
    }


    private void UpdateIcon(string newSchema)
    {
        Debug.Log($"Actualizando icono para el esquema: {newSchema}");
        var action = actionReference.action;

        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];

            string path = binding.effectivePath;
            string grupos = binding.groups; // Aquí están los control schemes

            if (grupos.Contains(newSchema))
            {
                string resultado = "SpritesIconsUI/" + newSchema + "/" + path.Split('/')[1];// "SpritesIconsUI/Keyboard/enter"
                Sprite nuevoSprite = Resources.Load<Sprite>(resultado);
                if (nuevoSprite == null)
                {
                    Debug.LogWarning($"No se pudo cargar la imagen desde el path: {resultado}");
                    continue;
                }
                else
                {
                    image.sprite = nuevoSprite;
                    return; 
                }
            }
        }

        Debug.LogWarning($"No se encontró una imagen para el esquema: {newSchema}");
    }
}
