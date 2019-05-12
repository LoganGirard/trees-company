using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShuffleHand : MonoBehaviour
{
    public bool ShouldShuffle = false;

    public GameObject[] PoolOfCards;

    private const int CARDS_IN_HAND = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ShouldShuffle)
        {
            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            List<GameObject> hand = GetRandomHandWithoutReplacement(CARDS_IN_HAND);

            foreach(GameObject card in hand)
            {
                Instantiate(card);
            }
        }
    }

    List<GameObject> GetRandomHandWithoutReplacement(int numCards)
    {
        List<GameObject> pool = new List<GameObject>(PoolOfCards);
        List<GameObject> hand = new List<GameObject>();

        for(int i = 0; i < numCards; i++)
        {
            int randomIndex = Random.Range(0, pool.Count);
            GameObject card = PoolOfCards[randomIndex];

            hand.Add(card);
            pool.RemoveAt(randomIndex);
        }

        return hand;
    } 
}
