using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : ScreenOverlay
{
    public Transform startPosition;
    public Transform endPosition;
    public Transform loadingObject;
    public float Speed = 5f;

    public override void RunEffect()
    {
        StartCoroutine(RunOnScreen());
    }

    private IEnumerator RunOnScreen()
    {
        loadingObject.position = startPosition.position;

        while (Vector2.Distance(loadingObject.position, endPosition.position) > 0.1f)
        {
            //print(Vector2.Distance(loadingObject.position, endPosition.position));
            loadingObject.position += (endPosition.position - startPosition.position).normalized * Time.fixedDeltaTime * Speed;
            //print("AFTER " + Vector2.Distance(loadingObject.position, endPosition.position));
            yield return new WaitForFixedUpdate();
        }

        OnScreenFinished(screenType);
    }
}
