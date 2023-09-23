#pragma strict
var play : GUITexture;
var play_tex : Texture2D[];
var click : AudioClip;

function Start () {

}

function Update () {

 if (Input.GetKeyDown(KeyCode.Escape)) 
   Application.Quit(); 
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
   play.texture = play_tex[0];
Application.LoadLevel("WorldSelection");

}
function OnMouseDown(){
audio.PlayOneShot(click);
 play.texture = play_tex[1];



}