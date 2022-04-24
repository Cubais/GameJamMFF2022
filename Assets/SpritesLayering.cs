using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesLayering : MonoBehaviour
{
    private SpriteRenderer[] sprites;
    private List<int> baseOrders = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        sprites = gameObject.GetComponentsInChildren<SpriteRenderer>(true);
        foreach (var item in sprites)
        {
            baseOrders.Add(item.sortingOrder);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var index = 0;
        foreach (var sprite in sprites)
        {
            sprite.sortingOrder = baseOrders[index] + Mathf.RoundToInt(Mathf.Abs(transform.position.y) * 10f);
            index++;
        }
    }
}
