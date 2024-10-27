using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthManager : MonoBehaviour
{
    public GameObject bossHealthBar;

    public void ActivateBossHealthBar()
    {
        bossHealthBar.SetActive(true);
    }

    public void DeactivateBossHealthBar()
    {
        bossHealthBar.SetActive(false);
    }
}
