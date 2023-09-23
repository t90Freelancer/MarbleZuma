

using UnityEngine;
using System.Collections;

public class mnb : MonoBehaviour {
	public static AndroidJavaClass adjav;
	public static int count = 0;
	// Use this for initialization
	void Start () {
	
		if( Application.platform == RuntimePlatform.Android ) {
			// Initialize Fluct/ZucksAdnetSDK
			adjav = new AndroidJavaClass("com.dxone.zumablast.MainActivity");
			
			// ZucksAdnetSDK display banner
		//	adjav.CallStatic("onexit");
		//	if(count%5==0)
			adjav.CallStatic("showAd");
		//	count++;
			//adjav.Call("myrate");
			
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
