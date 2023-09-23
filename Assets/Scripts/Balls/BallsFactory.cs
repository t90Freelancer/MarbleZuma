using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class BallsFactory : MonoBehaviour
{
    /// <summary>
    /// Ball prefab.
    /// </summary>
    public GameObject BallPrefab;

    /// <summary>
    /// BeaierPathController script.
    /// </summary>
    public BezierPathController PathController;

    /// <summary>
    /// Audio source.
    /// </summary>
    public AudioSource AudioSource;

    /// <summary>
    /// The root of generated balls game object.
    /// </summary>
    public GameObject BallsRoot;

    /// <summary>
    /// Multiplicity of distance between balls.
    /// </summary>
    public float BallsDistanceMultiplicity = 1.05f;

    /// <summary>
    /// Amount of generated balls.
    /// </summary>
    public int GeneratedBallsAmount = 50;

    /// <summary>
    /// Already generated balls.
    /// </summary>
    public int GeneratedBalls = 0;

    /// <summary>
    /// Balls diameter.
    /// </summary>
    public float BallsDiameter { get; private set; }

    /// <summary>
    /// Total distance between balls.
    /// </summary>
    public float DistanceBetweenBalls { get; private set; }

    /// <summary>
    /// Probability of changing color (between 0 and 1).
    /// </summary>
    public double ProbabilityColorChange = 0.8f;

    /// <summary>
    /// Prob-y of bonus ball.
    /// </summary>
    public double BonusProbability;

    /// <summary>
    /// Colors of generated ball.
    /// </summary>
    public List<Material> AvailableMaterials;

    /// <summary>
    /// Bonus ball texture;
    /// </summary>
    public Material BonusMaterial;

    /// <summary>
    /// Is it needs to generate new balls?
    /// </summary>
    private bool _factoring = true;

    /// <summary>
    /// Random generator.
    /// </summary>
    private System.Random _random = new System.Random();

    /// <summary>
    /// Id of prev ball.
    /// </summary>
    private int _prevId=-3;
	
	private bool _alreadyStarted = false;
	
    /// <summary>
    /// Coroutine of generating.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Factory()
    {
        BallsDiameter = BallPrefab.transform.localScale.x;
        DistanceBetweenBalls = BallsDiameter*BallsDistanceMultiplicity;

        GeneratedBalls = 0;
        while (GeneratedBalls<GeneratedBallsAmount)
        {
            if (_factoring)
            {
                Debug.Log(PathController.BallSequence.Count);
                if (PathController.BallSequence.Count > 0)
                {
                    var lastBall = PathController.BallSequence.Last();   
                    if ((lastBall.transform.position - PathController.PathNodes[0]).magnitude > DistanceBetweenBalls)
                    {
                        GenerateNewBall();
                        PathController.Correction();
                        GeneratedBalls++;
                    }
                
                }
                else
                {
                    GenerateNewBall();
                    PathController.Correction();
                    GeneratedBalls++;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Get id of next ball.
    /// </summary>
    /// <returns></returns>
    public int GetNextId()
    {
        return _random.Next(AvailableMaterials.Count);
    }

    /// <summary>
    /// Generates a new ball.
    /// </summary>
    private void GenerateNewBall()
    {
        var ball = (GameObject)Instantiate(BallPrefab, transform.position, Quaternion.identity);
        ball.transform.parent = BallsRoot.transform;
        ball.transform.position = PathController.PathNodes[0];
        AudioSource.Play();
        var nextId = 0;
        if (_prevId == -3)
        {
            nextId = GetNextId();
            _prevId = nextId;
        }
        else
        {
            if (_random.NextDouble() <= ProbabilityColorChange)
            {
                nextId = GetNextId();
                _prevId = nextId;
            }
            else
            {
                nextId = _prevId;
            }
        }
        var ballComponent = ball.GetComponent<Ball>();
        ballComponent.SetBallId(nextId);
        ballComponent.NextNode = PathController.PathNodes[0];
        ballComponent.PathController = PathController;
        PathController.BallSequence.Add(ballComponent);
    }


    /// <summary>
    /// Starting the factory.
    /// </summary>
    public void StartFactory()
    {
        _factoring = true;
		if (_alreadyStarted == false){
			_alreadyStarted = true;	
			StartCoroutine(PathController.DelayedDecreaseSpeed(1, 1f));
		}
    }

    /// <summary>
    /// Stops the factory.
    /// </summary>
    public void StopFactory()
    {
        _factoring = false;
    }

    public bool IsFactoring()
    {
        return _factoring;
    }

    /// <summary>
    /// Initializing.
    /// </summary>
    void Start()
    {
        StartCoroutine(Factory());
    }
}
