using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarTank : MonoBehaviour
{

    Image healthyBar;
    public float maxHealthy = 100f;
    public static float healthTank;
    // Start is called before the first frame update
    void Start()
    {
        healthyBar = GetComponent<Image>();
        healthTank = maxHealthy;
    }

    // Update is called once per frame
    void Update()
    {
        healthyBar.fillAmount = healthTank / maxHealthy;
    }
}
