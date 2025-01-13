using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGoldEffect : MonoBehaviour
{
    void Start()
    {
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height * Camera.main.aspect;

        ParticleSystem.ShapeModule shape = GetComponent<ParticleSystem>().shape;
        shape.radius = width + 2.0f;

        float x = shape.radius / 2.0f;
        float y = (height / 2.0f) + 2.0f;

        transform.position = new Vector3(transform.position.x - x, transform.position.y + y, transform.position.z);
    }
}
