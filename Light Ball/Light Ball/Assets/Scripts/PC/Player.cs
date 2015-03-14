using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : PlayerClass {

    //Header("Controller Bool")]
    private bool useController;
	PlayerData playerData = new PlayerData(100, 100, 0);

    [Header("Ball Variables")]
    [Header("Arrays Ball Colours must be the same")]
	public GameObject[] lightBallPrefabs;
	public string[] lightBallMode;
    public Material[] gunMaterials;
    public float whiteFirePower;
    public float otherBallFirePower;
    private int ballIndex = 0;

    [HideInInspector]
    public GameObject gun;
    [HideInInspector]
    public GameObject gunCylinder1;
    [HideInInspector]
    public GameObject gunCylinder2;

    void Start()
    {
        ControllerState controllerstate = GetComponent<ControllerState>();
        if (controllerstate.inputDevice == ControllerState.InputState.Keyboard)
            useController = false;
        else
            useController = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //initialise object pool
        InitPool(pooledAmount, lightBallPrefabs[0], lightBallPrefabs[1], lightBallPrefabs[2]);

    }

    void Update()
    {
        UserInput();
    }


    private void UserInput()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonUp("LB_1"))
        {
            ChangeBall();
        }
    }


    #region Object Pooling
    //lists where pooled gameobjects in scene are stored
    protected List<GameObject> redBalls = new List<GameObject>();
    protected List<GameObject> blueBalls = new List<GameObject>();
    protected List<GameObject> whiteBalls = new List<GameObject>();
    private int pooledAmount = 10;//amount of each object to pool

    //method to initilise pool system must be called in start function and passed light ball prefab array elements
    public void InitPool(int size, GameObject _white, GameObject _red, GameObject _blue)
    {
        Vector3 spawnPos = new Vector3(100, 100, 100);//object pool position

        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(_white);
            obj.transform.position = spawnPos;
            whiteBalls.Add(obj);
            obj.SetActive(false);
        }

        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(_red);
            obj.transform.position = spawnPos;
            redBalls.Add(obj);
            obj.SetActive(false);
        }
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(_blue);
            obj.transform.position = spawnPos;
            blueBalls.Add(obj);
            obj.SetActive(false);
        }
    }

    public void TopUpPool(int size, bool topUpWhite, bool topUpRed, bool topUpBlue)
    {
        Vector3 spawnPos = new Vector3(100, 100, 100);//object pool position

        if(topUpWhite)
        {
            for (int i = 0; i < pooledAmount; i++)
            {
                GameObject obj = (GameObject)Instantiate(lightBallPrefabs[0]);
                obj.transform.position = spawnPos;
                whiteBalls.Add(obj);
                obj.SetActive(false);
            }
        }
        else if (topUpRed)
        {
            for (int i = 0; i < pooledAmount; i++)
            {
                GameObject obj = (GameObject)Instantiate(lightBallPrefabs[1]);
                obj.transform.position = spawnPos;
                redBalls.Add(obj);
                obj.SetActive(false);
            }
        }
        else if (topUpBlue)
        {
            for (int i = 0; i < pooledAmount; i++)
            {
                GameObject obj = (GameObject)Instantiate(lightBallPrefabs[3]);
                obj.transform.position = spawnPos;
                blueBalls.Add(obj);
                obj.SetActive(false);
            }
        }
    }
    #endregion

    #region Light Ball Systems

    //this method can be called on button press or scoll wheel and the Boolean negates whether to increase the array index or not
    public void ChangeBall()
    {
        ballIndex++;
        //dont let ball index got out of the array bounds
        if (ballIndex > lightBallPrefabs.Length - 1)
            ballIndex = 0;

        Material[] _gunMats = gunCylinder2.GetComponent<MeshRenderer>().materials;
        _gunMats[1] = gunMaterials[ballIndex];
        gunCylinder2.GetComponent<MeshRenderer>().materials = _gunMats;

        _gunMats = gunCylinder1.GetComponent<MeshRenderer>().materials;
        _gunMats[0] = gunMaterials[ballIndex];
        gunCylinder1.GetComponent<MeshRenderer>().materials = _gunMats;
      

    }
    #endregion


    #region Fire 

    //Call this method to fire a ball
    public void Fire()
    {
        GameObject _bullet;

        switch (ballIndex)
        {
            case 0:

                 _bullet = FetchFromPool(whiteBalls);
                 ActivateBullet(_bullet);
                 break;

            case 1:

                _bullet = FetchFromPool(redBalls);
                ActivateBullet(_bullet);
                break;

            case 2:

                _bullet = FetchFromPool(blueBalls);
                ActivateBullet(_bullet);
                break;

        }

    }

    //Activate bullet will find a bullet in the pool and move it to the correct position ready to be fired
    private void ActivateBullet(GameObject _bullet)
    {
        _bullet.SetActive(true);//turn ball on
        Vector3 bulletPos = gun.transform.TransformPoint(Vector3.forward);//Camera.main.transform.position;//set bullet position
        _bullet.transform.position = bulletPos;
        _bullet.transform.rotation = Camera.main.transform.rotation;
        Rigidbody ballRigidbody = _bullet.GetComponent<Rigidbody>();

        //raycast from middle of camera to get where cursor is aiming
        Transform cam = Camera.main.transform;
        RaycastHit hit;
        Vector3 hitPoint;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 10000))
        {
            hitPoint = hit.point;

            if (_bullet.name == "whiteBall(Clone)")
            {
                GameObject clone = PhotonNetwork.Instantiate("whiteBall", bulletPos,
                                                        _bullet.transform.rotation,
                                                        0) as GameObject;
                clone.transform.LookAt(hitPoint);//make projectile travel towards raycast hit point and where the reticle was aiming
            }
            if (_bullet.name == "redBall(Clone)")
            {
                GameObject clone = PhotonNetwork.Instantiate("redBall", bulletPos,
                                                       _bullet.transform.rotation,
                                                       0) as GameObject;
                clone.transform.LookAt(hitPoint);
            }
            if (_bullet.name == "blueBall(Clone)")
            {
                GameObject clone = PhotonNetwork.Instantiate("blueBall", bulletPos,
                                                        _bullet.transform.rotation,
                                                        0) as GameObject;
                clone.transform.LookAt(hitPoint);
            }

            _bullet.SetActive(false);
        }
    }


    private GameObject FetchFromPool(List<GameObject> pool)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i].gameObject;
            }
        }

        return null;
    }

    #endregion 



}


//OLD SHOOTING METHOD
      //if (blueBalls.Count < 3)
      //      TopUpPool(10, false, false, false, true);
      //  else if (whiteBalls.Count < 3)
      //      TopUpPool(10, true, false, false, false);
      //  else if (redBalls.Count < 3)
      //      TopUpPool(10, false, true, false, false);
      //  else if (greenBalls.Count < 3)
      //      TopUpPool(10, false, false, true, false);