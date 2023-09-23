

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BallsCannon : MonoBehaviour
{
    /// <summary>
    /// Prefab of the ball.
    /// </summary>
    public GameObject BallPrefab;

    /// <summary>
    /// Reference to BezierPathController.
    /// </summary>
    public BezierPathController PathController;

    /// <summary>
    /// Balls factory.
    /// </summary>
    public BallsFactory Factory;

    /// <summary>
    /// Sound of shooting.
    /// </summary>
    private AudioSource _audioSource;

    /// <summary>
    /// Max distance of shooting.
    /// </summary>
    public float ShootLength;

    /// <summary>
    /// Speed of "bullet".
    /// </summary>
    public float ShootSpeed;

    /// <summary>
    /// Color of next "bullet".
    /// </summary>
    private int _nextIndex;

    /// <summary>
    /// Renderer of ball.
    /// </summary>
    public MeshRenderer MeshRenderer;

    /// <summary>
    /// Is cannon can shoot?
    /// </summary>
    private bool _canShoot = true;

    /// <summary>
    /// Manager of animations.
    /// </summary>
    public AnimationManager AnimationManager;

    /// <summary>
    /// Shoot delay.
    /// </summary>
    public float ShootDelay;

    private System.Random _random = new System.Random();
	
	public BallsTrap ballstrap;

	// Use this for initialization
	void Start ()
	{
		
		BallsTrap._gameOver = false;
		BallsTrap._win = false;
		
	    _audioSource = GetComponent<AudioSource>();
	    if (_random.NextDouble() < Factory.BonusProbability)
	    {
	        _nextIndex = -1;
	        MeshRenderer.material = Factory.BonusMaterial;
	    }
	    else
	    {
	        _nextIndex = Factory.GetNextId();
	        MeshRenderer.material = Factory.AvailableMaterials[_nextIndex];
	    }
	    StartCoroutine(FollowCoroutine());
	}

    /// <summary>
    /// Coroutine of shooting delay.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootDelayCoroutine()
    {
        _canShoot = false;
        AnimationManager.RunAnimation(AnimationThrowType.OnCannonLocked);
        yield return new WaitForSeconds(ShootDelay);
        _canShoot = true;
        AnimationManager.RunAnimation(AnimationThrowType.OnCannonUnlocked);
    }

    /// <summary>
    /// Coroutine of following to mouse.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FollowCoroutine()
    {
        while (true)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            
            
                var dx = Input.mousePosition.x - Camera.main.WorldToScreenPoint(transform.position).x;
                var dy = Input.mousePosition.y - Camera.main.WorldToScreenPoint(transform.position).y;
                var strawRadians = Mathf.Atan2(dy, dx);
                var strawDigrees = 360.0f*strawRadians/(2.0f*Mathf.PI);
                transform.localRotation = Quaternion.Euler(transform.localRotation.x, -strawDigrees + 90,
                    transform.rotation.z);
            
#else
            if (Input.touchCount > 0)
            {
                var dx = Input.touches[0].position.x - Camera.main.WorldToScreenPoint(transform.position).x;
                var dy = Input.touches[0].position.y - Camera.main.WorldToScreenPoint(transform.position).y;
                var strawRadians = Mathf.Atan2(dy, dx);
                var strawDigrees = 360.0f*strawRadians/(2.0f*Mathf.PI);
                transform.localRotation = Quaternion.Euler(transform.localRotation.x, -strawDigrees + 90,
                    transform.rotation.z);
            }
#endif
                
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Making a shoot.
    /// </summary>
    void Shoot()
    {
        if (PathController.BallSequence.Count == 0) return;
        
        _audioSource.Play();
        var shootVector = -transform.forward;
        var ball = (GameObject)Instantiate(BallPrefab, transform.position, Quaternion.identity);
        
        var ballController = ball.GetComponent<Ball>();
        ballController.SetBallId(_nextIndex);
        ballController.PathController = PathController;
        ballController.Shoot(transform.position + shootVector * ShootLength, ShootLength / ShootSpeed);
        StartCoroutine(ShootDelayCoroutine());
        PathController.AnimationManager.RunAnimation(AnimationThrowType.OnShoot);

        
            if (_random.NextDouble() > Factory.BonusProbability)
            {
                _nextIndex = PathController.BallSequence[_random.Next(PathController.BallSequence.Count)].BallId;
                MeshRenderer.material = Factory.AvailableMaterials[_nextIndex];
            }
            else
            {
                _nextIndex = -1;
                MeshRenderer.material = Factory.BonusMaterial;    
            }
       
    }

    // Update is called once per frame
	void Update () {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0) && BallsTrap._gameOver == false && BallsTrap._win == false)
        {
            if (PathController.Factory.IsFactoring() && _canShoot) Shoot();
        }
#else
        if (Input.touchCount>0)
        if (Input.touches[0].phase == TouchPhase.Ended)
        {
            if (PathController.Factory.IsFactoring() && _canShoot) Shoot();
        }
#endif
        
	}
}
