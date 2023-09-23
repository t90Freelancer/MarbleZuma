

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BezierPathController : MonoBehaviour
{
    /// <summary>
    /// List of Bezier curve's control points.
    /// </summary>
    public List<BezierControlPoint> ControlPoints;

    /// <summary>
    /// Reference to animation manager script.
    /// </summary>
    public AnimationManager AnimationManager;

    /// <summary>
    /// All balls in ballSequence.
    /// </summary>
    public List<Ball> BallSequence = new List<Ball>();

    /// <summary>
    /// Balls factory.
    /// </summary>
    public BallsFactory Factory;

    /// <summary>
    /// List of simple point in curve. (See debug to understand).
    /// </summary>
    public List<Vector3> PathNodes { get; private set; } 

    /// <summary>
    /// Segments per simple Bezier curve.
    /// </summary>
    public int SegmentsPerCurve=20;

    /// <summary>
    /// Speed of balls in start of round;
    /// </summary>
    public float BallsSpeed = 5f;

    /// <summary>
    /// Relaxed balls speed.
    /// </summary>
    public float RelaxedSpeed = 1f;

    /// <summary>
    /// Speed of backward movement.
    /// </summary>
    public float BackBallsSpeed = 3f;

    /// <summary>
    /// Relative speed of ball insertion;
    /// </summary>
    public float InsertionSpeedMultiplicity = 2f;
    
    /// <summary>
    /// Is debugging needed?
    /// </summary>
    public bool DebugBezierPath;

    /// <summary>
    /// Prefab containig destroy animation.
    /// </summary>
    public GameObject DestroyingParticleSystemPrefab;

    /// <summary>
    /// Prefab of popup scores.
    /// </summary>
    public GameObject PopupScoresPrefab;

    /// <summary>
    /// How many balls will destroyed by bonus.
    /// </summary>
    public int BonusRadiusDestroy = 2;
    /// <summary>
    /// Max equal balls in sequence.
    /// </summary>
    public int MaxEqualsBalls = 3;

    /// <summary>
    /// Sound of blast.
    /// </summary>
    public AudioClip BlastSound;

    /// <summary>
    /// Sound of destroy.
    /// </summary>
    public AudioClip DestroySound;

    /// <summary>
    /// Sound of balls movement.
    /// </summary>
    public AudioClip MovingSound;

    /// <summary>
    /// Main audio source.
    /// </summary>
    public AudioSource MainAudioSource;

    /// <summary>
    /// Additional audio source.
    /// </summary>
    public AudioSource AdditionalAudioSource;

    private bool _moving = false;

    /// <summary>
    /// Returns a length of path in Bezier curve.
    /// </summary>
    /// <param name="objectCoords">Coords of interested object.</param>
    /// <returns>Length.</returns>
    public float GetBezierPath(Vector3 objectCoords)
    {
        var length = 0f;
        for (var i = 0; i < PathNodes.Count-1; i++)
        {
            var lengthToNextNode = (PathNodes[i] - PathNodes[i+1]).magnitude;
            var lengthToBall = (PathNodes[i] - objectCoords).magnitude;
            if (lengthToNextNode < lengthToBall)
            {
                length += lengthToNextNode;
            }
            else
            {
                length += lengthToBall;
                break;
            }
        
        }
        return length;
    }

    public void Correction()
    {
        for (int i = 0; i < BallSequence.Count-1; i++)
        {          
            var currentBall = GetBezierPath(BallSequence[i].transform.position);

            var nextBall = GetBezierPath(BallSequence[i + 1].transform.position);
            var d = currentBall - nextBall;

            if (d < Factory.DistanceBetweenBalls) 
                MoveBallBackward(BallSequence[i + 1].GetComponent<Ball>(),Mathf.Abs(d-Factory.DistanceBetweenBalls));
            else
                MoveBallForward(BallSequence[i + 1].GetComponent<Ball>(), Mathf.Abs(d - Factory.DistanceBetweenBalls));
        }
    }

    /// <summary>
    /// Move ball backward on specified length.
    /// </summary>
    /// <param name="ball">Ball.</param>
    /// <param name="length">Length on Bezier curve.</param>
    public void MoveBallBackward(Ball ball, float length)
    {
        if (!BallSequence.Contains(ball) || ball == null) return;

        var passedDistance = 0f;
        var currentNodeIndexInBall = PathNodes.IndexOf(ball.NextNode);
        var currentNodeIndex = currentNodeIndexInBall - 1;

        while (passedDistance < length)
        {
            if (currentNodeIndex < 0) return;
            var distanceToCurrentNode = (ball.transform.position - PathNodes[currentNodeIndex]).magnitude;

            if (length - passedDistance < distanceToCurrentNode)
            {
                ball.transform.position = Vector3.Lerp(ball.transform.position, PathNodes[currentNodeIndex],
                    (length - passedDistance)/distanceToCurrentNode);
                passedDistance = length;
            }
            else
            {
                var translationVector = PathNodes[currentNodeIndex] - ball.transform.position;
                ball.transform.Translate(translationVector);
                passedDistance += distanceToCurrentNode;
                currentNodeIndex--;
                currentNodeIndexInBall--;
                ball.NextNode=PathNodes[currentNodeIndexInBall];
            }

        }
    }

    /// <summary>
    /// Move ball forward on Bezier curve to specified distance.
    /// </summary>
    /// <param name="ball">Ball.</param>
    /// <param name="length">Distance.</param>
    public void MoveBallForward(Ball ball, float length)
    {
        if (!BallSequence.Contains(ball) || ball == null)
        {
            //Object can be deleted at this time
            return;
        }

        var tmpPosition = ball.transform.position;
        var passedDistance = 0f;
        var currentNodeIndex = PathNodes.IndexOf(ball.NextNode);
        while (passedDistance < length)
        {
            if (currentNodeIndex >= PathNodes.Count)
            {
                //end of curve.
                ball.NextNode = PathNodes[PathNodes.Count - 1];
                //TODO: to destroying the balls at the end of the curve, calls DestroyBall here.
                DestroyBall(ball);
                return;
            }
            var distanceToNextNode = (PathNodes[currentNodeIndex] - tmpPosition).magnitude;
            if (passedDistance + distanceToNextNode < length)
            {
                passedDistance += distanceToNextNode;
                tmpPosition = PathNodes[currentNodeIndex];
                currentNodeIndex++;
            }
            else
            {
                var translationDirection = (PathNodes[currentNodeIndex] - ball.transform.position).normalized;
                var translationLength = length - passedDistance;
                var translationVector = translationDirection*translationLength;
                passedDistance += translationLength;
                tmpPosition += translationVector;
                ball.NextNode = PathNodes[currentNodeIndex];
            }
        }
        ball.transform.Translate(tmpPosition-ball.transform.position);
    }

    /// <summary>
    /// Move all balls forward.
    /// </summary>
    /// <param name="balls">List of moving balls</param>
    /// <param name="length">Distance.</param>
    public void MoveAllBallsForward(List<Ball> balls, float length)
    {
        //That's right. It's closure.
        var balls1 = new List<Ball>();
        balls1.AddRange(balls);
        foreach (var ball in balls1)
        {
            MoveBallForward(ball, length);
        }
    }

    /// <summary>
    /// Destroy specified ball.
    /// </summary>
    /// <param name="ball"></param>
    /// <param name="delay"></param>
    public void DestroyBall(Ball ball, float delay = 0)
    {
        ball.GetComponent<AnimationPlayer>().Play(AnimationThrowType.OnDestroy);
        Instantiate(DestroyingParticleSystemPrefab, ball.transform.position, Quaternion.identity);
        StartCoroutine(DestroyBallCoroutine(ball, delay));
    }

    /// <summary>
    /// Stop all.
    /// </summary>
    public void StopSequence()
    {
        StopAllCoroutines();
        AnimationManager.RunAnimation(AnimationThrowType.OnPause);
        Factory.StopFactory();
        _moving = false;
    }

    /// <summary>
    /// When game ends, all balls moving to hole.
    /// </summary>
    public void MoveToDestroyAll()
    {
        StopSequence();
        StartCoroutine(MoveSequenceForward(BallSequence, 10000f, 7));
    }

    /// <summary>
    /// Start all.
    /// </summary>
    /// <param name="delayTime"></param>
    public void StartSequence(float delayTime)
    {
        if (!_moving)
        {
            _moving = true;
            StartCoroutine(DelayedResume(BallSequence, delayTime));
        }
    }

    /// <summary>
    /// Insert new ball into sequence BEFORE specified ball.
    /// </summary>
    /// <param name="newBall">New ball.</param>
    /// <param name="contactedBall">Ball, which collided with new ball.</param>
    public void InsertBallInSequence(Ball newBall, Ball contactedBall)
    {
        StopSequence();
        var d = Factory.DistanceBetweenBalls;
        if (BallSequence.Contains(contactedBall))
        {
            var index = BallSequence.IndexOf(contactedBall)+1;
            newBall.NextNode = contactedBall.NextNode;
            newBall.PathController = this;
            BallSequence.Insert(index, newBall);
            newBall.GetComponent<AnimationPlayer>().Play(AnimationThrowType.OnInsert);
            StartCoroutine(MoveSequenceForward(BallSequence.GetRange(0,index), d, BallsSpeed*InsertionSpeedMultiplicity));
            StartCoroutine(ShiftBallTo(newBall, contactedBall.transform.position, (d/BallsSpeed)/InsertionSpeedMultiplicity));
            StartCoroutine(DelayedCheckEqualBalls(newBall, d/BallsSpeed*1.1f/InsertionSpeedMultiplicity));
            
            MainAudioSource.Stop();
            MainAudioSource.clip = BlastSound;
            MainAudioSource.Play();
        }
    }

    /// <summary>
    /// Update the list of drawing points.
    /// </summary>
    private void RecalculateDrawPoints()
    {
        var controlPointsCoords = ControlPoints.Select(bezierControlPoint => bezierControlPoint.transform.position).ToList();
        PathNodes = BezierCurves.GetDrawingPoints(controlPointsCoords, SegmentsPerCurve);
    }

    #region Coroutines
    /// <summary>
    /// Moving ball sequence backward on Bezier curve.
    /// </summary>
    /// <param name="balls">List of movable balls.</param>
    /// <param name="length">Length of movement.</param>
    /// <param name="speed">Speed of movement.</param>
    private IEnumerator MoveSequenceBackward(List<Ball> balls, float length, float speed, float delay = 0f)
    {
		yield return new WaitForSeconds(delay);
        AdditionalAudioSource.clip = MovingSound;
        AdditionalAudioSource.Play();
        var passedTime = 0f;
        while (passedTime <= length / speed)
        {
            passedTime += Time.deltaTime;
            foreach (var ball in balls)
            {
                MoveBallBackward(ball, Time.deltaTime * speed);
            }
            yield return new WaitForEndOfFrame();
        }
        AdditionalAudioSource.Stop();
		Debug.Log ("Passed:"+passedTime);
        //StartSequence(0);
    }

    /// <summary>
    /// Moving ball sequence forward on Bezier curve.
    /// </summary>
    /// <param name="balls">List of movable balls.</param>
    /// <param name="length">Length of movement.</param>
    /// <param name="speed">Speed of movement.</param>
    private IEnumerator MoveSequenceForward(List<Ball> balls, float length, float speed)
    {
        var passedTime = 0f;
        while (passedTime <= length / speed)
        {
            passedTime += Time.deltaTime;
            MoveAllBallsForward(balls, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Continiously moving of all ball sequence.
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveContiniously(List<Ball> balls)
    {
        while (true)
        {

            MoveAllBallsForward(balls, BallsSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Shift ball to specified point.
    /// </summary>
    /// <param name="ball">Shifting ball.</param>
    /// <param name="targetPosition">Destination.</param>
    /// <param name="time">Time.</param>
    /// <returns></returns>
    private IEnumerator ShiftBallTo(Ball ball, Vector3 targetPosition, float time)
    {
        var passedTime = 0f;
        var startPosition = ball.transform.position;
        while (passedTime < time)
        {
            passedTime += Time.deltaTime;
            var position = Vector3.Lerp(startPosition, targetPosition, passedTime / time);
            ball.transform.position = position;
            yield return new WaitForEndOfFrame();
        }
        ball.transform.position = targetPosition;
    }

    /// <summary>
    /// Coroutine of dalayed resuming into gameplay.
    /// </summary>
    /// <param name="balls"></param>
    /// <param name="delay"></param>
    private IEnumerator DelayedResume(List<Ball> balls, float delay)
    {
        yield return new WaitForSeconds(delay);
        Factory.StartFactory();
        StartCoroutine(MoveContiniously(balls));
        AnimationManager.RunAnimation(AnimationThrowType.OnResume);
    }

    /// <summary>
    /// Coroutine of checking and deleting balls.
    /// </summary>
    /// <param name="insertedBall">Ball, which had been inserted in sequence.</param>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    private IEnumerator DelayedCheckEqualBalls(Ball insertedBall, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        var currentId = int.MinValue;
        var ballsToDelete = new List<Ball>();
        var tmp = new List<Ball>();
        Ball nextCheckBall = null;
        foreach (var ball in BallSequence)
        {
            if (ball == null) break;
            var currentBallId = ball.GetComponent<Ball>().BallId;

            //bonus
            if (currentBallId == -1)
            {
                tmp.Clear();
                ballsToDelete.Clear();
                var bonusIndex = BallSequence.IndexOf(ball);
                for (int i = 0; i <= BonusRadiusDestroy; i++)
                {
                    try
                    {
                        if (!ballsToDelete.Contains(BallSequence[bonusIndex+i])) ballsToDelete.Add(BallSequence[bonusIndex+i]);
                        if (!ballsToDelete.Contains(BallSequence[bonusIndex-i])) ballsToDelete.Add(BallSequence[bonusIndex-i]);
                    }
                    catch (ArgumentOutOfRangeException) 
                    {
                    }
                }
                break;
            }

            if (currentBallId != currentId)
            {
                currentId = currentBallId;
                if (tmp.Count > MaxEqualsBalls && tmp.Contains(insertedBall))
                {
                    var nextCheckBallIndex = BallSequence.IndexOf(tmp.First()) - 1;
                    if (nextCheckBallIndex >= 0) nextCheckBall = BallSequence[nextCheckBallIndex];
                    ballsToDelete.AddRange(tmp);
                    tmp.Clear();
                }
                else tmp.Clear();
            }
            tmp.Add(ball);
        }
        if (tmp.Count > MaxEqualsBalls && tmp.Contains(insertedBall))
        {
            ballsToDelete.AddRange(tmp);
            tmp.Clear();
        }
        if (ballsToDelete.Count > 0)
        {
            //StartSequence and move balls backward
            var ballsBeforeDeletingSequence = new List<Ball>();
            foreach (var ball in BallSequence)
            {
                if (!ballsToDelete.Contains(ball))
                {
                    ballsBeforeDeletingSequence.Add(ball);
                }
                else
                {
                    break;
                }
            }

            var length = ballsToDelete.Count * Factory.DistanceBetweenBalls;
            

            //NOTE: scores
            var prefab = (GameObject)Instantiate(PopupScoresPrefab, ballsToDelete[0].transform.position,Quaternion.identity);
            GameObject.FindGameObjectWithTag("GlobalManager").GetComponent<ScoreManager>().Scores +=
                ballsToDelete.Count*10;
            prefab.GetComponentInChildren<TextMesh>().text = ("+" + ballsToDelete.Count*10).ToString();

            MainAudioSource.Stop();
            MainAudioSource.clip = DestroySound;
            MainAudioSource.Play();

            foreach (var ballToDelete in ballsToDelete)
            {
                DestroyBall(ballToDelete);
            }
			
			var speed = BallsSpeed;
			BallsSpeed = 0;
			yield return new WaitForSeconds(0.7f);
			BallsSpeed = speed;
			
			//Debug.Log("Count:"+ballsBeforeDeletingSequence.Count.ToString()+" Length:"+length.ToString()+" Time:"+(length / BackBallsSpeed).ToString());
			//StartCoroutine(
				//MoveSequenceBackward(ballsBeforeDeletingSequence, length, BackBallsSpeed)
				//);
			
			AdditionalAudioSource.clip = MovingSound;
        	AdditionalAudioSource.Play();
        	var passedTime = 0f;
        	while (passedTime <= length / BackBallsSpeed)
        	{
            	passedTime += Time.deltaTime;
            	foreach (var ball in ballsBeforeDeletingSequence)
            	{
                	MoveBallBackward(ball, Time.deltaTime * BackBallsSpeed);
            	}
            	yield return new WaitForEndOfFrame();
        	}
        	AdditionalAudioSource.Stop();
			
			//if (ballsBeforeDeletingSequence.Count>0)
			//	yield return new WaitForSeconds(length/BackBallsSpeed);
            if (nextCheckBall != null)
            {
				//Correction();
                StartCoroutine(DelayedCheckEqualBalls(nextCheckBall,0));// length / BackBallsSpeed));
                
            }else{
				StartSequence(0);
				Correction();
				
			}

        }
        else {
			StartSequence(0);
			Correction();
		}
    }

    /// <summary>
    /// Coroutine of ballDestroying.
    /// </summary>
    /// <param name="ball"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator DestroyBallCoroutine(Ball ball, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        if (BallSequence.Contains(ball))
        {
            BallSequence.Remove(ball);
        }
        Destroy(ball.gameObject);
    }

    public IEnumerator DelayedDecreaseSpeed(float delay, float newSpeed)
    {
        yield return new WaitForSeconds(delay);
      //  BallsSpeed = newSpeed;
		BallsSpeed = RelaxedSpeed;
        Correction();
    }

    #endregion Coroutines

    #region MonoBehaviour

    /// <summary>
    /// Debugging a Bezier path.
    /// </summary>
    void OnDrawGizmos()
    {
        if (!DebugBezierPath) return;
        List<Vector3> controlPointsCoords;
        try
        {
            controlPointsCoords =
                ControlPoints.Select(bezierControlPoint => bezierControlPoint.transform.position).ToList();
        }
        catch (Exception) { return; }
        var drawingPoints = BezierCurves.GetDrawingPoints(controlPointsCoords, SegmentsPerCurve);

        //Drawing a Bezier path.
        Gizmos.color = Color.red;
        for (var i = 0; i < drawingPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(drawingPoints[i], drawingPoints[i + 1]);
        }

        //Drawing a directing pairs of control points.
        Gizmos.color = Color.yellow;
        var pairsCount = controlPointsCoords.Count/2;
        for (var i = 0; i < pairsCount; i++)
        {
            Gizmos.DrawLine(controlPointsCoords[i*2], controlPointsCoords[i*2+1]);
        }

        //Drawing a simple point of curve.
        Gizmos.color = Color.blue;
        foreach (var drawingPoint in drawingPoints)
        {
            Gizmos.DrawSphere(drawingPoint,0.1f);
        }
    }

//    void OnGUI()
//    {
//        if (GUI.Button(new Rect(0, 0, 200, 50), "Start"))
//        {
//            StartSequence(0);
//            
//        }
//        if (GUI.Button(new Rect(0, 60, 200, 50), "Pause"))
//        {
//            StopSequence();
//        }
//    }

    void Awake()
    {
        MainAudioSource = GetComponent<AudioSource>();
        if (ControlPoints.Count() < 4) throw new ArgumentException("Bezier curve requires minimum 4 control points");
        RecalculateDrawPoints();
    }
	
	void Start()
    {
        StartSequence(0);
    }
    #endregion MonoBehaviour
}
