using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeWall : MonoBehaviour
{
    private void Start()
    {
        DestroyOverlapMaze();
    }

    private void DestroyOverlapMaze()
    {
        Vector3 boxSize = this.GetComponent<BoxCollider>().size;
        Collider[] colliders = Physics.OverlapBox(this.gameObject.transform.position, boxSize);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != this.gameObject && collider.gameObject.tag == "Wall")
            {
                Destroy(gameObject);
                break;
            }
        }
    }
}
