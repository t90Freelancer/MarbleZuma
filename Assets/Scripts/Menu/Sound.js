#pragma strict
var sound : GUITexture;
var sound_tex : Texture2D[];

function Start () {
if(AudioListener.pause){
 sound.texture = sound_tex[1];
 }

}

function Update () {

}

function OnMouseDown(){

 
 if(sound.texture.name=="SPEAKER"){
      AudioListener.pause = true;
       sound.texture = sound_tex[1];
      }
     else if(sound.texture.name=="MUTE"){
      AudioListener.pause = false;
      sound.texture = sound_tex[0];
      }



}