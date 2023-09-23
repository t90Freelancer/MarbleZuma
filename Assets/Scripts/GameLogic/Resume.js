#pragma strict
var resume : GUITexture;
var resume_tex : Texture2D[];
var pauseMenu : GUITexture;
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
resume.texture = resume_tex[0];
Time.timeScale = 1;
pauseMenu.active = false;


}
function OnMouseDown(){
audio.PlayOneShot(click);
 resume.texture = resume_tex[1];



}