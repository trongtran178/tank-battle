using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SolidierHealth : MonoBehaviour
{
    Image healthyBar;
    float maxHealthy = 100f;
    public static float SoliderHealth;
    // Start is called before the first frame update
    void Start()
    {
        healthyBar = GetComponent<Image>();
        SoliderHealth = maxHealthy;
    }

    // Update is called once per frame
    void Update()
    {
        healthyBar.fillAmount = SoliderHealth / maxHealthy;
    }
}
