using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    public GameObject TransitionImage;
    public bool TransitionInOnStart = false;
    public float TransitionInDuration = 0.6f;


    private bool transitionActive = false;
    private float startPos = 0;
    private float endPos = 0;
    private float transitionDuration = 0;
    private float elapsedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (TransitionInOnStart)
        {
            StartTransition(true, TransitionInDuration);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!transitionActive)
        {
            return;
        }

        elapsedTime += Time.deltaTime;
        float t = Mathf.Min(1.0f, elapsedTime / transitionDuration);
        float posX = Mathf.Lerp(startPos, endPos, t);
        SetImagePos(posX);

        if (elapsedTime > transitionDuration)
        {
            transitionActive = false;
        }
    }

    public void StartTransition(bool transitionIn, float duration)
    {
        startPos = CalculateStartPos(transitionIn);
        endPos = CalculateEndPos(transitionIn);
        transitionDuration = duration;
        elapsedTime = 0;
        transitionActive = true;

        SetImagePos(startPos);
        TransitionImage.SetActive(true);
    }

    private void SetImagePos(float posX)
    {
        Vector2 pos = TransitionImage.transform.localPosition;
        pos.x = posX;
        TransitionImage.transform.localPosition = pos;
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
            return 2500.0f;
        }
        else
        {
            return 0.0f;
        }
    }
}
