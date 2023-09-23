#pragma strict
var more : GUITexture;
var more_tex : Texture2D[];
var click : AudioClip;

function Start () {

}

function Update () {
//if (Input.touchCount>0) {
//    for (var touch : Touch in Input.touches) {
//    
//    
//     if (touch.phase == TouchPhase.Began){
//     
//     if (play.HitTest(touch.position)) {
//       	play.texture = play_tex[1];
//       	   
//		Application.LoadLevel("QuickPlay");
//	
//       	
//       }       
//           	                                                                                                                                  
//                                                                                                                                
//                      
//       }
//      if (touch.phase == TouchPhase.Ended){
//       	play.texture = play_tex[0]; 
//         
//     }  
//      
//        }
//       
//       
//       }

}
function OnMouseUp(){
   more.texture = more_tex[0];
//Application.OpenURL("https://play.google.com/store/apps/developer?id=Dimensionx.Studio&hl=en");

}
function OnMouseDown(){
audio.PlayOneShot(click);
 more.texture = more_tex[1];



}