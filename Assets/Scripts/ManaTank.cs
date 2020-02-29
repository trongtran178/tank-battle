using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaTank : MonoBehaviour
{

    public Image manaShow;
    public float maxMana;
    public static float manaTank;
    // Start is called before the first frame update
    void Start()
    {
        manaTank = maxMana;
    }

    // Update is called once per frame
    void Update()
    {
        if (manaTank < maxMana)
            manaTank += 2f * Time.deltaTime;
        manaShow.fillAmount = manaTank / maxMana;
    }
}
