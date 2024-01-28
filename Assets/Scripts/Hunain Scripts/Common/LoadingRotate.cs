using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingRotate : MonoBehaviour
{

    private RectTransform rectTransform;

    public float rotateSpeed = 200f;
    // Start is called before the first frame update
    void Awake()
    {
        //Debug.LogError("%%%%%%%%%%%%%%%"+this.gameObject);
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

        float currentSpeed = rotateSpeed * Time.deltaTime;
        rectTransform.Rotate(0f,0f,currentSpeed);
    }
}
