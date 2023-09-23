#pragma strict
var num2 : int;
var str : String;
var levelnum : String;
var lnum : int;

function Start () {
    num2 = WorldUnlock.num;
 levelnum = Application.loadedLevelName;
 
 lnum = int.Parse(levelnum.Substring(2));
	  if(WorldUnlock.num==1)
		str = "w1";
		 else if (WorldUnlock.num==2)
		 str = "w2";
		 else if (WorldUnlock.num==3)
		 str = "w3";
//		 print(sstr" + str);
	PlayerPrefs.SetInt(str,lnum);	
	if(lnum>=10 && str == "w1"){
	
		PlayerPrefs.SetInt("world",1);
		
	}else if(lnum>=10 && str == "w2"){
	
		print("levelnum" + levelnum.Substring(2));
		PlayerPrefs.SetInt("world",2);
		
	}


}

function Update () {

}