using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static void SetActiveDelayed(this MonoBehaviour bev, bool a, float t)
    {
        bev.StartCoroutine(_SetActiveDelayed(t, a, bev.gameObject));
    }

    private static IEnumerator _SetActiveDelayed(float t, bool a, GameObject go)
    {
        yield return new WaitForSeconds(t);
        go.SetActive(a);
    }
}
