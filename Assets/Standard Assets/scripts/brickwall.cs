using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class brickwall : MonoBehaviour {

    public GameObject Brick;
    public float radius = 2.0f;
    public float angleRad = 2.0f * (float)Mathf.PI / 180.0f;
    public float angleDeg = 2.0f;
    public int height = 20;
    public int numOfBlocks = 20;

    // Use this for initialization
    void Start () {
        float angleIncrement = 2 * Mathf.PI / numOfBlocks;
        // creating the brick wall
        for (int floor = 0; floor < height; floor++)
        {
            for (int block = 0; block < numOfBlocks; block++)
            {
                float angle = block * angleIncrement;
                float angleOffset = 0.0f;
                if (floor % 2 != 0)
                {
                    angleOffset = angleIncrement / 2.0f;
                }
                float posX = radius * Mathf.Cos(angle + angleOffset);
                float posY = floor * (Brick.GetComponent<Transform>().localScale.y) + 0.7f;
                float posZ = radius * Mathf.Sin(angle + angleOffset);
                Vector3 pos = new Vector3(posX, posY, posZ);
                Vector3 newRotation = new Vector3(
                    0.0f, -((angle + angleOffset) * 180.0f / (float)Mathf.PI), 0.0f);
                Quaternion startRotation = Quaternion.Euler(newRotation);
                Transform transform = Instantiate(Brick, pos, startRotation).transform;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
    }

}
