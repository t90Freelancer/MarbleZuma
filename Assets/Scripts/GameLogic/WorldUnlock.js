#pragma strict

var worldArray : GUITexture[];
var worldArray_tex : Texture2D[];
var locked : Texture2D[];
static var num : int= 1;


function Start () {
//PlayerPrefs.SetInt("world",0);

 for (var i=0;i<worldArray.Length;i++){
 	if(i<=PlayerPrefs.GetInt("world")){
 		print("world=" + i);
 		worldArray[i].texture = worldArray_tex[i];
 		
 		}
 	 else 
     worldArray[i].texture = locked[i];
 	    
 	    
 }
 
}

function Update () {
 for(var i =0; i<worldArray.length;i++){
    if(Input.GetMouseButton(0) && worldArray[i].HitTest(Input.mousePosition))

        {
            
            
            if(i<=PlayerPrefs.GetInt("world")){
                num = i+1;
//				print("aaaaaaaaaaaaaaaaaaa" + num);
               Application.LoadLevel("LevelSelection");
                    
                
             }
            
        }
    }

}

