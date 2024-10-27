using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    bool waiting;

    public void HitStopEffect(float duration, float timeSpeed)
    {
        if (waiting)
            return;

        Time.timeScale = timeSpeed;
        StartCoroutine(ResetTime(duration));
    }

    IEnumerator ResetTime(float duration)
    {
        waiting = true;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
        waiting = false;
    }
}
