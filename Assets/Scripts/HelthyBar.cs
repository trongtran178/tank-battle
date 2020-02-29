using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelthyBar : MonoBehaviour
{

    Image healthyBar;
    float maxHealthy = 100f;
    public static float health;
    // Start is called before the first frame update
    void Start()
    {
        healthyBar = GetComponent<Image>();
        health = maxHealthy;
    }

    // Update is called once per frame
    void Update()
    {
        healthyBar.fillAmount = health / maxHealthy;
    }
}
