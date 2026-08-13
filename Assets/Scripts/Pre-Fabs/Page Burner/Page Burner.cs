using UnityEngine;
using UnityEngine.UI;

// Gaurav Singh

public class PageBurner : MonoBehaviour
{
    [SerializeField]
    PlayerDataSO PDSO;

    [SerializeField]
    PageBurnerSO Burner;

    Image img;

    bool BURN;

    void Awake()
    {
        img = GetComponent<Image>();

        img.color = new Color(1, 1, 1, 1);

        if (PDSO.FireStage > 4)
        {
            BURN = true;
        }
        else
        {
            if(Mathf.FloorToInt(PDSO.Fire) == 0)
            {
                img.color = new Color(0, 0, 0, 0);
            }
            else
            {
                img.sprite = Burner.Burn[Mathf.FloorToInt(PDSO.Fire) - 1];
            }
        }
    }

    void Update()
    {
        
    }
}
