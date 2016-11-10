using UnityEngine;
using System.Collections;

public class ThrowGame_Obstacle : MonoBehaviour {

    Vector3[] paths;
    float speed = 0.02f;
    int current_node = 0;
    int next_node = 1;
    Vector3 direction;
    float diatance;

    public bool enable = false;

    // Use this for initialization
    void Start () {
        paths = new Vector3[4];

        paths[0] = new Vector3(-4, 1, 3);
        paths[1] = new Vector3(4, 1, 3);
        paths[2] = new Vector3(-4, 3, 3);
        paths[3] = new Vector3(4, 3, 3);

        direction = new Vector3(1, 0, 0);
        diatance = 6;

        transform.position = new Vector3(0, -100, -100);
    }

    public void enableObstacle()
    {
        enable = true;
        transform.position = paths[0];
    }

    public void disableObstacle()
    {
		if (enable) {
			enable = false;
			transform.position = new Vector3 (0, -100, -100);
		}
    }

    // Update is called once per frame
    void Update () {

        if (enable)
        {
            transform.position += direction * speed;
            transform.Rotate(new Vector3(1, 1, 2));

            if (Vector3.Distance(transform.position, paths[current_node]) > diatance)//arrive next node
            {
                current_node = (current_node + 1) % 4;
                next_node = (next_node + 1) % 4;
                direction = Vector3.Normalize(paths[next_node] - paths[current_node]);
                diatance = Vector3.Distance(paths[next_node], paths[current_node]);
            }
        }

	}
}
