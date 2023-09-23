
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public Texture BackgroundTexture;
    public int Scores = 0;

    public TextMesh TextMesh;

    void Update()
    {
        TextMesh.text = Scores.ToString();
        /*var aspectRatio = BackgroundTexture.width/BackgroundTexture.height;
        var height = Screen.width/aspectRatio;
        GUI.DrawTexture(new Rect(0,0,Screen.width, height), BackgroundTexture, ScaleMode.StretchToFill);
        Style.fontSize = (int)((height/(float)BackgroundTexture.height)*36);
        GUI.Label(new Rect( Screen.width/48f*32f, height/8f*2.5f, 250, 50), Scores.ToString(), Style);
         * */
    }
}
