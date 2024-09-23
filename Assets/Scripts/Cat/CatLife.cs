using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatLife : MonoBehaviour
{
    [SerializeField] Animator _lifeAnimator;

    public void ShowLife()
    {
        _lifeAnimator.SetBool("show", true);
    }
    
    public void GainLife()
    {
        _lifeAnimator.SetBool("fill", true);
    }
    
    public void LoseLife()
    {
        _lifeAnimator.SetBool("lose", true);
    }
    
}
