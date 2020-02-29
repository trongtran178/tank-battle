using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneHealthBar : MonoBehaviour
{
    Image healthyBar;
    float maxHealthy = 100f;
    public float PlaneHealth;
    // Start is called before the first frame update
    void Start()
    {
        healthyBar = GetComponent<Image>();
        PlaneHealth = maxHealthy;
    }

    // Update is called once per frame
    void Update()
    {
        healthyBar.fillAmount = PlaneHealth / maxHealthy;
    }
}
