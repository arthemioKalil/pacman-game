using UnityEngine;
using UnityEngine.UI;

public class CollectibleUI : MonoBehaviour
{
    public Image StrawberryImage;
    public Image CherryImage;

    private int limitStrawberry = -1;
    private int limitCherry = -1;

    void Start()
    {
        var _allCollectible = FindObjectsOfType<Collectible>();
        foreach (var collectible in _allCollectible)
        {
            collectible.OnSpecialFruits += Collectible_OnSpecialFruits;
        }

        var allStrawberry = GameObject.FindGameObjectsWithTag("Strawberry");
        var allCherry = GameObject.FindGameObjectsWithTag("Cherry");

        if (limitStrawberry == -1)
        {
            limitStrawberry = allStrawberry.Length;
        }
        if (limitCherry == -1)
        {
            limitCherry = allCherry.Length;

        }

        CherryImage.gameObject.SetActive(true);
        StrawberryImage.gameObject.SetActive(true);
    }

    private void Collectible_OnSpecialFruits(Collectible collectible, GameObject specialFruit)
    {
        if (specialFruit.CompareTag("Cherry"))
        {
            limitCherry--;

            if (limitCherry <= 0)
            {
                CherryImage.gameObject.SetActive(false);
                limitCherry = -1;
            }

        }
        else if (specialFruit.CompareTag("Strawberry"))
        {
            limitStrawberry--;

            if (limitStrawberry <= 0)
            {
                StrawberryImage.gameObject.SetActive(false);
                limitStrawberry = -1;
            }

        }

        collectible.OnSpecialFruits -= Collectible_OnSpecialFruits;

    }

}
