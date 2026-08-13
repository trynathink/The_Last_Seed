using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SRAnimClipToImageAnimClip : MonoBehaviour
{
    SpriteRenderer SR;
    Image Img;

    void OnEnable()
    {
        SR = GetComponent<SpriteRenderer>();
        Img = GetComponent<Image>();

        SR.RegisterSpriteChangeCallback(OnSpriteChanged);
    }

    void OnDisable()
    {
        SR.UnregisterSpriteChangeCallback(OnSpriteChanged);
    }

    void OnSpriteChanged(SpriteRenderer SRR)
    {
        Img.sprite = SR.sprite;

        Rect rec = SR.sprite.rect;

        if(!(rec.width >= 2048 && rec.height >= 1152))
        {
            Img.rectTransform.sizeDelta = new Vector2(rec.width, rec.height);
        }
    }
}
