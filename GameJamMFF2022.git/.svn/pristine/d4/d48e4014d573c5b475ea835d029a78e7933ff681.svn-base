using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BordersResizer : MonoBehaviour
{
    [SerializeField] BoxCollider2D leftBorder;
    [SerializeField] BoxCollider2D rightBorder;
    [SerializeField] BoxCollider2D bottomBorder;
    [SerializeField] BoxCollider2D topBorder;
    
    void Update()
    {
        topBorder.size = new Vector2(Screen.width, Screen.height / 4f);
        topBorder.transform.position = new Vector2(Screen.width / 2f, Screen.height - (topBorder.size.y / 2f));

        leftBorder.size = new Vector2(100f, Screen.height);
        leftBorder.transform.position = new Vector2(-50, Screen.height / 2f);

        bottomBorder.size = new Vector2(Screen.width, 100f);
        bottomBorder.transform.position = new Vector2(Screen.width / 2f, -50f);

        rightBorder.size = new Vector2(100f, Screen.height);
        rightBorder.transform.position = new Vector2(Screen.width + 50f, Screen.height / 2f);
    }
}
