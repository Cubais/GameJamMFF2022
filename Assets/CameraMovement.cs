using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : Singleton<CameraMovement>
{
    public bool CameraMoving = true;

    void LateUpdate()
    {
        if (CameraMoving)
            transform.position = new Vector3(GameManager.instance.playerCharacter.transform.position.x, 0.0f, -80f);       
    }
}
