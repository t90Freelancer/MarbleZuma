
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class PlaneScaleController : MonoBehaviour
{

    private MeshRenderer _meshRenderer;

	// Use this for initialization
	void Start ()
	{
	    _meshRenderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var texture = _meshRenderer.material.mainTexture;
	    var aspectRatio = (float)texture.width/texture.height;
        transform.localScale = new Vector3(aspectRatio,1,1);
	}
}
