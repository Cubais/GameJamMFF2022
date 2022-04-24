using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : Singleton<CameraMovement>
{
    private float currentBorderXLeft = 0;
    private float currentBorderXRight = 0;

    void LateUpdate()
    {        
        if (GameManager.instance.playerCharacter.transform.position.x > currentBorderXLeft && GameManager.instance.playerCharacter.transform.position.x < currentBorderXRight)
            transform.position = new Vector3(GameManager.instance.playerCharacter.transform.position.x, 0.0f, -80f);
    }

    public void SetCameraEdge(float edgePosXLeft, float edgePosXRight)
    {
        currentBorderXLeft = edgePosXLeft;
        currentBorderXRight = edgePosXRight;
    }

    public void ResetCamera()
    {
        transform.position = new Vector3(0.0f, 0.0f, -80f);
    }
}
