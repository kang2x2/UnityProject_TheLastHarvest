using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSet : MonoBehaviour
{
    RectTransform _rect;
    BoxCollider2D _collider;

    void Start()
    {
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height * Camera.main.aspect;

        // 화면 경계 (화면이 이동하면서 삼지창이 밖으로 나갈 때가 있음)
        _rect = gameObject.AddComponent<RectTransform>();
        _rect.sizeDelta = new Vector2(width, height);

        _collider = GetComponent<BoxCollider2D>();
        _collider.size = new Vector2(width, height);


        // Left Wall
        GameObject wall = new GameObject { name = "WallLeft", tag = "Wall" };
        wall.transform.parent = transform;
        wall.AddComponent<BoxCollider2D>().isTrigger = true;
        wall.transform.localPosition = new Vector2(-(width / 2.0f), 0);
        wall.GetComponent<BoxCollider2D>().size = new Vector2(0.2f, height);

        // Right Wall
        wall = new GameObject { name = "WallRight", tag = "Wall" };
        wall.transform.parent = transform;
        wall.AddComponent<BoxCollider2D>().isTrigger = true; ;
        wall.transform.localPosition = new Vector2(width / 2.0f, 0);
        wall.GetComponent<BoxCollider2D>().size = new Vector2(0.2f, height);

        // Top Wall
        wall = new GameObject { name = "WallTop", tag = "Wall" };
        wall.transform.parent = transform;
        wall.AddComponent<BoxCollider2D>().isTrigger = true; ;
        wall.transform.localPosition = new Vector2(0, height / 2.0f);
        wall.GetComponent<BoxCollider2D>().size = new Vector2(width, 0.2f);

        // Bottom Wall
        wall = new GameObject { name = "WallBottom", tag = "Wall" };
        wall.transform.parent = transform;
        wall.AddComponent<BoxCollider2D>().isTrigger = true; ;
        wall.transform.localPosition = new Vector2(0, -(height / 2.0f));
        wall.GetComponent<BoxCollider2D>().size = new Vector2(width, 0.2f);
    }

}
