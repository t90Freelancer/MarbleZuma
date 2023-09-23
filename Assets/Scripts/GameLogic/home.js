#pragma strict
var home : GUITexture;
var home_tex : Texture2D[];
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
	
   home.texture = home_tex[0];
   Time.timeScale = 1;
	Application.LoadLevel("Menu");

}
function OnMouseDown(){
	audio.PlayOneShot(click);
 	home.texture = home_tex[1];



}