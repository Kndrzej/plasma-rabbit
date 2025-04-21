using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rotatable))]
public class Card : MonoBehaviour
{
    public int ID;

    public Rotatable Rotatable;
    [SerializeField] public Image Image;

    private void Awake()
    {
        Rotatable = GetComponent<Rotatable>();
    }

    public void SetFrontTexture(Texture2D texture)
    {
      
        if (Image != null && texture != null)
        {
            // Create a new instance of the material if needed
            if (Image.material != null)
            {

                Debug.Log(Image.material);
                Image.material = new Material(Image.material);
            }

            Image.material.SetTexture("_MainTex", texture);
        }
    }




}
