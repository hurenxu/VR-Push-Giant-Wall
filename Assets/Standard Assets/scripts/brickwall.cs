using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class brickwall : MonoBehaviour {

    public GameObject Brick;
    public GameObject Camera;
    List<GameObject> tower;
    Collider LookingObj;
    public float radius = 2.0f;
    public float angleRad = 2.0f * (float)Mathf.PI / 180.0f;
    public float angleDeg = 2.0f;
    public int height = 20;
    public int numOfBlocks = 20;
    bool needRebuild;
    public GameObject ball;
    LoadingBar Bar;
    bool towerHasBeenTouched, test;
    RaycastHit hitInfo;
    public float Force = 10;

    // Use this for initialization
    void Start () {
        Camera.transform.position = new Vector3(0.0f, 1.0f, 0.0f);
        Bar = GetComponent<LoadingBar>();
        Bar.OnReachMaximum += Bar_OnReachMaximum;
        tower = new List<GameObject>();
        RebuildTower();
        test = true;
    }

    private void Bar_OnReachMaximum(object sender, EventArgs e)
    {
        if (LookingObj != null)
        {
            if (LookingObj.name == "Brick(Clone)")
            {
                LookingObj.attachedRigidbody.AddForce(Force * Mathf.Log(Bar.Duration, 30) * Camera.transform.forward, ForceMode.Acceleration);
                var particle = LookingObj.GetComponent<ParticleSystem>();
                particle.Play();
                towerHasBeenTouched = true;
            }
            else if (LookingObj.name == "ground")
            {
                Camera.transform.position = new Vector3(hitInfo.point.x, Camera.transform.position.y, hitInfo.point.z);
                Bar.Reset();
                Bar.IsLocked = true;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (needRebuild)
        {
            RebuildTower();
            needRebuild = false;
        }
    }

    private void RebuildTower()
    {
        float angleIncrement = 2 * Mathf.PI / numOfBlocks;
        // creating the brick wall
        for (int floor = 0; floor < height; floor++)
        {
            for (int block = 0; block < numOfBlocks; block++)
            {
                float angle = (block + 0.5f) * angleIncrement;
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
                    0.0f, -((angle + angleOffset) * 180.0f / Mathf.PI), 0.0f);
                Quaternion startRotation = Quaternion.Euler(newRotation);
                var brickOBJ = Instantiate(Brick, pos, startRotation);
                var particle = brickOBJ.GetComponent<ParticleSystem>();
                particle.Stop();               
                tower.Add(brickOBJ);
            }
        }
    }

    void FixedUpdate()
    {
        
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        /*if (Physics.Raycast(transform.position, fwd, out hitInfo, 10))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            if (hitObject != previousObj)
            {
                duration = 0.0f;
                previousObj = hitObject;
            }
            else
            {
                duration += Time.deltaTime;
                if (duration > 2.0f)
                {
                    if (hitObject.name == "Brick(Clone)")
                    {
                        Vector3 forward = transform.TransformDirection(Vector3.forward);
                        GameObject newball = Instantiate(ball, Camera.GetComponent<Transform>().localPosition, Camera.GetComponent<Transform>().rotation).gameObject;
                        newball.GetComponent<Rigidbody>().velocity = 30.0f * forward;
                    }
                    else if (hitObject.name == "ground")
                    {
                        Camera.transform.position = new Vector3(hitInfo.point.x, Camera.transform.position.y, hitInfo.point.z);
                    }
                    else if (hitObject.name == "Reset")
                    {
                        Camera.transform.position = new Vector3(0.0f, 0.5f, 0.0f);
                        foreach (Transform child in tower)
                        {
                            Destroy(child.gameObject);
                        }
                        rebuild = true;
                    }
                    duration = 0.0f;
                }
            }
        }
        //else
        {
            duration = 0.0f;
        }*/
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hitInfo, 10))
        {            
            Collider collider = hitInfo.collider;            
            if (collider != LookingObj)
            {
                if (LookingObj != null)
                {
                    var particle = LookingObj.GetComponent<ParticleSystem>();
                    if (particle != null) { particle.Stop(); }
                }
                LookingObj = collider;
                Bar.Reset();
                
            }
            else
            {
                Bar.IncreastTime(Time.deltaTime);
            }
        }
        else
        {
            if (LookingObj != null)
            {
                var particle = LookingObj.GetComponent<ParticleSystem>();
                if (particle != null) { particle.Stop(); }
            }
            Bar.Reset();
            if (Mathf.Abs(Camera.transform.forward.x) < 0.1 && (Mathf.Abs(Camera.transform.forward.z) < 0.1) && towerHasBeenTouched && test)
            {                              
                foreach (var brick in tower)
                {
                    Destroy(brick);                    
                }
                tower = new List<GameObject>();
                towerHasBeenTouched = false;
                needRebuild = true;
            }
        }
    }

}
