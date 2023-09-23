#pragma strict
var next : GUITexture;
var next_tex : Texture2D[];
var click : AudioClip;
var levelnum : int;

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
	
   next.texture = next_tex[0];
	Application.LoadLevel(levelnum);

}
function OnMouseDown(){
	audio.PlayOneShot(click);
 	next.texture = next_tex[1];



}