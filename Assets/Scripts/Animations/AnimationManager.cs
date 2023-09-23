

using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    /// <summary>
    /// List of animation players.
    /// </summary>
    public List<AnimationPlayer> AnimationListeners;
    
    /// <summary>
    /// Run specified animations in listeners.
    /// </summary>
    /// <param name="throwType"></param>
    public void RunAnimation(AnimationThrowType throwType)
    {
        foreach (var animationListener in AnimationListeners)
        {
            animationListener.Play(throwType);
        }
    }

}
