using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationForObjects : MonoBehaviour
{
    public Vector3 fromScale; // Initial scale, set in the Inspector
    public Vector3 toScale; // Target scale, set in the Inspector
    private float duration = 0.5f; // Duration of the scale animation

    void Start()
    {
        fromScale = transform.localScale;
        toScale = Vector3.one * 1.1f;
        //StartCoroutine(RepeatScaleTween());
    }
    private void OnEnable()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(RepeatScaleTween());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator RepeatScaleTween()
    {
        // Wait 5 seconds before repeating the animation

        // LeanTween the scale again

        yield return new WaitForSeconds(5f);
        LeanTween.scale(gameObject, toScale, duration)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
                 LeanTween.scale(gameObject, fromScale, duration)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
                OnEnable()));
    }


}
