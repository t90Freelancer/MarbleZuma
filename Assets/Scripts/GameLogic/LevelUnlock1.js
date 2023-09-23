#pragma strict

var levelArray : GUITexture[];
var levelArray_tex : Texture2D[];
var locked : Texture2D;


function Start () {

 for (var i=0;i<levelArray.Length;i++){
 
 	if(i<=PlayerPrefs.GetInt("w" + WorldUnlock.num))
 		levelArray[i].texture = levelArray_tex[i];
 	 else 
 	    levelArray[i].texture = locked;
 }
}

function Update () {
 for(var i =0; i<levelArray.length;i++){
    if(Input.GetMouseButton(0) && levelArray[i].HitTest(Input.mousePosition))

        {
            
            
            if(i<=PlayerPrefs.GetInt("w" + WorldUnlock.num)){
                
				
				var str : String;
				if(WorldUnlock.num==1)
				 str = "w1";
				 else if (WorldUnlock.num==2)
				 str = "w2";
				 else if (WorldUnlock.num==3)
				 str = "w3";
				 
               Application.LoadLevel(str + (i +1));
                    
                
             }
            
        }
    }

}