

using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SpriteSequence : MonoBehaviour
{

    public Material TargetMaterial;

    public List<Texture2D> Sequence;

    public float Delay;

    private IEnumerator AnimationCoroutine()
    {
        foreach (var texture2D in Sequence)
        {
            TargetMaterial.mainTexture = texture2D;
            yield return new WaitForSeconds(Delay);
        }
        yield break;
    }

    void Awake()
    {
        StartCoroutine(AnimationCoroutine());
    }

}
