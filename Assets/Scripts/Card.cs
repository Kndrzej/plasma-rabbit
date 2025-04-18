using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rotatable))]
public class Card : MonoBehaviour
{
    public int ID;

    public Rotatable Rotatable;

    private void Awake()
    {
        Rotatable = GetComponent<Rotatable>();
    }

    public void SetFrontTexture(Texture2D texture)
    {
        var image = GetComponentInChildren<Image>();
        if (image != null && texture != null)
        {
            // Create a new instance of the material if needed
            if (image.material != null && image.material.name.EndsWith("(Instance)") == false)
            {
                image.material = new Material(image.material);
            }

            image.material.SetTexture("_MainTex", texture);
        }
    }




}
