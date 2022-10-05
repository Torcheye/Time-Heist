using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class DecLight : MonoBehaviour
{
    private Light2D _light2D;
    private bool _forward;

    private void Awake()
    {
        _light2D = GetComponent<Light2D>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(Random.Range(0f, 3f));
        while (true)
        {
            _light2D.intensity += _forward ?  .01f : -.01f;
            if (_light2D.intensity >= 1)
                _forward = false;
            else if (_light2D.intensity <= 0)
                _forward = true;
            yield return new WaitForSeconds(.01f);
        }
    }
}