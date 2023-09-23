
using System.Collections;
using System.Linq;
using UnityEngine;

public class BallsTrap : MonoBehaviour
{

    public static bool _gameOver = false;

    public static bool _win = false;

    public BezierPathController PathController;

    public AnimationManager AnimationManager;

    public MusicManager MusicManager;
	
	public GameObject GameOverMenu;
	
	public GameObject GameWonMenu;

    void Start()
    {
        StartCoroutine(CheckWinCoroutine());
    }

    private IEnumerator CheckWinCoroutine()
    {
        while (true)
        {
            if (PathController.Factory.GeneratedBalls == PathController.Factory.GeneratedBallsAmount &&
                PathController.BallSequence.Count == 0 && !_gameOver)
            {
                _win = true;
				GameWonMenu.active = true;
                MusicManager.Win();
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            var ball = other.GetComponent<Ball>();
            if (PathController.BallSequence.Contains(ball))
            {
                PathController.DestroyBall(ball);
            }
        }

        ///Check to loose;
        if (other.gameObject.tag == "Ball" && PathController.BallSequence.Contains(other.GetComponent<Ball>()) && _gameOver == false)
        {
            MusicManager.Loose();
            PathController.StopSequence();
            AnimationManager.RunAnimation(AnimationThrowType.OnGameOver);
            _gameOver = true;
			GameOverMenu.active = true;
            PathController.MoveToDestroyAll();
        }
    }

//    private void OnGUI()
//    {
//        if (_gameOver)
//        {
//            GUI.Label(new Rect(Screen.width/2, Screen.height/2, 200, 50), "GameOver!");
//        }
//        if (_win)
//        {
//            GUI.Label(new Rect(Screen.width/2, Screen.height/2, 200, 50), "Win!");
//        }
//    }
}
