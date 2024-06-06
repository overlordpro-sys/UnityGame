using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryManager : MonoBehaviour {
    public static BoundaryManager Instance;
    private EdgeCollider2D edgeCollider;
    // Start is called before the first frame update
    void Start() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    public void UpdateBoundaries() {
        float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        float screenHeight = Camera.main.orthographicSize * 2;

        float xEdge = screenWidth / 2;
        float yEdge = screenHeight / 2;

        List<Vector2> points = new List<Vector2>();
        points.Add(new Vector2(-xEdge, -yEdge));
        points.Add(new Vector2(xEdge, -yEdge));
        points.Add(new Vector2(xEdge, yEdge));
        points.Add(new Vector2(-xEdge, yEdge));
        points.Add(new Vector2(-xEdge, -yEdge));
        edgeCollider.SetPoints(points);

    }

    // Update is called once per frame
    void Update() {

    }


}
