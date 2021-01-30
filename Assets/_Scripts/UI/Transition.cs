using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    public GameObject TransitionImage;

    private bool transitionActive = false;
    private float startPos = 0;
    private float endPos = 0;
    private float transitionDuration = 0;
    private float elapsedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!transitionActive)
        {
            return;
        }

        elapsedTime += Time.deltaTime;
        float pos = Mathf.Lerp(startPos, endPos, elapsedTime / transitionDuration);
        SetImagePos(pos);
    }

    public void StartTransition(bool transitionIn, float duration)
    {
        startPos = CalculateStartPos(transitionIn);
        endPos = CalculateEndPos(transitionIn);
        transitionDuration = duration;

        SetImagePos(startPos);
        TransitionImage.SetActive(true);
    }

    private void SetImagePos(float posX)
    {
        Vector2 pos = TransitionImage.transform.position;
        pos.x = posX;
        TransitionImage.transform.position = pos;
    }

    private float CalculateStartPos(bool transitionIn)
    {
        if (transitionIn)
        {
            return 0.0f;
        }
        else
        {
            return -2000.0f;
        }
    }

    private float CalculateEndPos(bool transitionIn)
    {
        if (transitionIn)
        {
            // make this display size dependent?
            return 2000.0f;
        }
        else
        {
            return 0.0f;
        }
    }
}
