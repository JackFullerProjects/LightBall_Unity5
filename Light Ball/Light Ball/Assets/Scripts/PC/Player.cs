using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : PlayerClass {


	PlayerData playerData = new PlayerData(100, 100, 0);

	public GameObject[] lightBallPrefabs;
	public string[] lightBallMode;
	private int ballIndex;

    void Start()
    {
        //initialise object pool
        InitPool(pooledAmount, lightBallPrefabs[0], lightBallPrefabs[1], lightBallPrefabs[3], lightBallPrefabs[2]);
    }

    #region Object Pooling
    //lists where pooled gameobjects in scene are stored
    protected List<GameObject> redBalls = new List<GameObject>();
    protected List<GameObject> blueBalls = new List<GameObject>();
    protected List<GameObject> whiteBalls = new List<GameObject>();
    protected List<GameObject> greenBalls = new List<GameObject>();
    private int pooledAmount = 5;//amount of each pobject to pool

    //method to initilise pool system must be called in start function and passed light ball prefab array elements
    public void InitPool(int size, GameObject _white, GameObject _red, GameObject _green, GameObject _blue)
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
            GameObject obj = (GameObject)Instantiate(_green);
            obj.transform.position = spawnPos;
            greenBalls.Add(obj);
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

    #endregion

    #region Light Ball Systems

    //this method can be called on button press or scoll wheel and the Boolean negates whether to increase the array index or not
    public void ChangeBall(bool Increase)
    {
        if (Increase)
            ballIndex++;
        else
            ballIndex--;

        //dont let ball index got out of the array bounds
        if (ballIndex > lightBallPrefabs.Length - 1)
            ballIndex--;
        else if (ballIndex < 0)
            ballIndex = 0;
    }
    #endregion


    //TODO: JACK YOU MAY NEED TO EDIT THIS REGIONS METHODS
    #region Fire 

    //Call this method to fire a ball
    public void Fire()
    {
        GameObject _bullet;

        switch (ballIndex)
        {
            case 1:

                 _bullet = FetchFromPool(whiteBalls);

                 if (!_bullet)//if fetchfrompool returns null dont continue
                     return;

                 ActivateBullet(_bullet);
                break;

            case 2:

                _bullet = FetchFromPool(redBalls);

                if (!_bullet)//if fetchfrompool returns null dont continue
                    return;

                ActivateBullet(_bullet);
                break;

            case 3:

                _bullet = FetchFromPool(blueBalls);

                if (!_bullet)//if fetchfrompool returns null dont continue
                    return;

                ActivateBullet(_bullet);
                break;

            case 4:

                _bullet = FetchFromPool(greenBalls);

                if (!_bullet)//if fetchfrompool returns null dont continue
                    return;

                ActivateBullet(_bullet);
                break;
        }

    }

    //Activate bullet will find a bullet in the pool and move it to the correct position ready to be fired
    //JACK YOU MAY WANT TO EDIT THIS FUNCTION 
    private void ActivateBullet(GameObject _bullet)
    {
        _bullet.SetActive(true);//turn ball on
        Vector3 bulletPos = transform.TransformPoint(Vector3.forward);//set bullet position
        _bullet.transform.position = bulletPos;

        //We still need to add a force/direction to the ball.
    }

    private GameObject FetchFromPool(List<GameObject> pool)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if(!pool[i].activeInHierarchy)
            {
                return pool[i].gameObject;
            }
        }

        //if we reach here we dont have any free balls in the pool to return null
        Debug.Log("No balls avaliable in pool");
        return null;
    }

    #endregion 

}
