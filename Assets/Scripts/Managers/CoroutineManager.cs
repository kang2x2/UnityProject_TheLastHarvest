using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoroutineManager : MonoBehaviour
{
    public Coroutine MyStartCoroutine(IEnumerator _coroutine)
    {
        return StartCoroutine(_coroutine);
    }
}
