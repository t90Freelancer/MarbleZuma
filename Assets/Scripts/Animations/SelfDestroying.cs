

using UnityEngine;
using System.Collections;

public class SelfDestroying : MonoBehaviour
{

    public float Delay;

    void Awake()
    {
        StartCoroutine(SelfDestroy(Delay));
    }

    private IEnumerator SelfDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
