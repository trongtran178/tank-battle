using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{

    private Slider slider;

    private float targetSlider;
    public float fillSpeed=0.5f;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        IncrementSlider(0.9f);
    }

    // Update is called once per frame
    void Update()
    {
        if(slider.value<targetSlider)
        {
            slider.value += fillSpeed * Time.deltaTime;
        }
    }


    public void IncrementSlider(float newSlider)
    {
        targetSlider= slider.value + newSlider;
    }
}
