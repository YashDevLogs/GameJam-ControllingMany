using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

public class Wall : MonoBehaviour
{
    [SerializeField] private bool LeftRight;

    void Start()
    {
        if (LeftRight)
        { 
        transform.DOMoveX(-1f, 2f).SetLoops(1000000, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
        else
        {
            transform.DOMoveX(1f, 2f).SetLoops(1000000, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
    }

}


