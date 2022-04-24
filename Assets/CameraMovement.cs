using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : Singleton<CameraMovement>
{
    private float currentBorderX = 0;
    
    void LateUpdate()
    {        
        if (GameManager.instance.playerCharacter.transform.position.x > currentBorderX)    
            transform.position = new Vector3(GameManager.instance.playerCharacter.transform.position.x, 0.0f, -80f);       
    }

    public void SetCameraEdge(float edgeXPos)
    {
        currentBorderX = edgeXPos;
    }

    public void ResetCamera()
    {
        transform.position = new Vector3(0.0f, 0.0f, -80f);
    }
}
