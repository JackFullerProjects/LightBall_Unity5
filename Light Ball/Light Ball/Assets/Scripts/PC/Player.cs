using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : PlayerClass, IEditAble {


    //classes 
    public DestructionModuleClass destructionModuleClass;
    public ImpairmentModuleClass impairmentModuleClass;
    //Header("Controller Bool")]
    private bool useController;
	public PlayerData playerData = new PlayerData(100, 100, 100, 0);
    [Header("Controller States Hold Input Device")]
    public ControllerState controllerstate;

    [Header("Ball Variables")]
    [Header("Arrays Ball Colours must be the same")]
	public GameObject[] lightBallPrefabs;
	public string[] lightBallMode;
    public List<Material> gunMaterials = new List<Material>();
    public float whiteFirePower;
    public float otherBallFirePower;
    [HideInInspector]
    public int ballIndex = 0;

    private bool canChangeColour = true;
    private bool playGunAnimationOnce = false;
    public float changeCooldown;
    private bool currentlyChangingColour;

    [Header("Gun Aesthetic Variables")]
    [HideInInspector]
    public GameObject gun;
    [HideInInspector]
    public GameObject gunCylinder1;
    [HideInInspector]
    public GameObject gunCylinder2;
    [SerializeField]
    [HideInInspector]
    public GameObject GunAnimation;
    public ParticleSystem gunParticle;
    public AnimationClip[] GunAnimationsArray;
    private bool isReloading;
    public LayerMask layerMask;

    public PunTeams.Team team;

    void Start()
    {
        
        if (controllerstate.inputDevice == ControllerState.InputState.Keyboard)
            useController = false;
        else
            useController = true;

        SetTeamLoadout(team);
        LevelManager.FirstRespawn(gameObject);


    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UserInput();
        AnimationManager();
    }

    private void SetTeamLoadout(PunTeams.Team _playerTeam)
    {
        var objectPool = GameObject.Find("ObjectManager").GetComponent<ObjectBank>();
        if (_playerTeam == PunTeams.Team.red)
        {
            gameObject.layer = 12;
            gunMaterials.Add(objectPool.GunMaterials[0]);
            gunMaterials.Add(objectPool.GunMaterials[1]);
        }
        else if (_playerTeam == PunTeams.Team.blue)
        {
            gameObject.layer = 13;
            gunMaterials.Add(objectPool.GunMaterials[0]);
            gunMaterials.Add(objectPool.GunMaterials[2]);
        }
    }

    private void AnimationManager()
    {
        if (currentlyChangingColour)
        {
            if (!GunAnimation.GetComponent<Animation>().isPlaying && !playGunAnimationOnce)
            {
                playGunAnimationOnce = true;
                GunAnimation.GetComponent<Animation>().clip = GunAnimationsArray[0];
                GunAnimation.GetComponent<Animation>().Play();
            }
            if (!GunAnimation.GetComponent<Animation>().isPlaying)
            {
                if (gunCylinder2.GetComponent<MeshRenderer>().enabled == true)
                {
                    Material[] _gunMats = gunCylinder2.GetComponent<MeshRenderer>().materials;
                    _gunMats[1] = gunMaterials[ballIndex];
                    gunCylinder2.GetComponent<MeshRenderer>().materials = _gunMats;

                    _gunMats = gunCylinder1.GetComponent<MeshRenderer>().materials;
                    _gunMats[0] = gunMaterials[ballIndex];
                    gunCylinder1.GetComponent<MeshRenderer>().materials = _gunMats;
                }
                StartCoroutine(ChangeCooldown(changeCooldown));
                currentlyChangingColour = false;
                canChangeColour = true;
                playGunAnimationOnce = false;
            }
        }

        if (isReloading)
        {
            if (!GunAnimation.GetComponent<Animation>().isPlaying)
            {
                GunAnimation.GetComponent<Animation>().clip = GunAnimationsArray[2];
                GunAnimation.GetComponent<Animation>().Play();
                isReloading = false;
            }
        }
    }

    private void UserInput()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonUp("LB_1"))
        {
            if(canChangeColour)
                ChangeBall();
        }
    }

    private void Reset()
    {
        playerData.health = 100;
        playerData.armour = 100;
    }

    #region Light Ball Systems

    //this method can be called on button press or scoll wheel and the Boolean negates whether to increase the array index or not
    public void ChangeBall()
    {
        canChangeColour = false;
        currentlyChangingColour = true;
        ballIndex++;
        //dont let ball index got out of the array bounds
        if (ballIndex > gunMaterials.Count - 1)
            ballIndex = 0;

    }

    IEnumerator ChangeCooldown(float _wait)
    {
        yield return new WaitForSeconds(_wait);
        canChangeColour = true;
    }
    #endregion


    #region Fire

    public void Fire()
    {
        GunAnimation.GetComponent<Animation>().clip = GunAnimationsArray[1];
        GunAnimation.GetComponent<Animation>().Play();

        //raycast from middle of camera to get where cursor is aiming
        Transform cam = Camera.main.transform;
        Vector3 raycastDirection = cam.forward;
        raycastDirection.x += Random.Range(-destructionModuleClass.Accuracy, destructionModuleClass.Accuracy);
        raycastDirection.y += Random.Range(-destructionModuleClass.Accuracy, destructionModuleClass.Accuracy);

        RaycastHit hit;
        Vector3 hitPoint;

        switch (ballIndex)
        {

            case 0 :
                
                if (Physics.Raycast(cam.position, raycastDirection, out hit, destructionModuleClass.Range, layerMask))
                {
                    hitPoint = hit.point;
                    GameObject clone = PhotonNetwork.Instantiate("HitParticle", hitPoint, transform.rotation, 0) as GameObject;
                    clone.GetComponentInChildren<ParticleSystem>().startColor = Color.white;

                    var hitPlayerPhotonView = hit.collider.gameObject.GetComponent<PhotonView>();

                    if (hit.collider.gameObject.transform.parent != null)
                    {
                        if (hit.collider.gameObject.transform.parent.tag == "ForceFieldRed")
                        {
                            if (team != PunTeams.Team.red)
                            {
                                hitPlayerPhotonView = hit.collider.gameObject.transform.parent.GetComponent<PhotonView>();

                                if (hitPlayerPhotonView != null)
                                    hitPlayerPhotonView.RPC("TakeDamage", PhotonTargets.All, destructionModuleClass.ForceFieldDamage);
                            }
                        }
                        else if (hit.collider.gameObject.transform.parent.tag == "ForceFieldBlue")
                        {
                            if (team != PunTeams.Team.blue)
                            {
                                hitPlayerPhotonView = hit.collider.gameObject.transform.parent.GetComponent<PhotonView>();

                                if (hitPlayerPhotonView != null)
                                    hitPlayerPhotonView.RPC("TakeDamage", PhotonTargets.All, destructionModuleClass.ForceFieldDamage);
                            }
                        }
                    }
                    
                    if (!hitPlayerPhotonView)
                        return;

                    if (hit.collider.gameObject.tag == "Player")
                    {
                        hitPlayerPhotonView.RPC("TakeDamage", PhotonTargets.All, destructionModuleClass.HealthDamage, gameObject);
                    }
                   
                }

            break;

            case 1:

                if (Physics.Raycast(cam.position, cam.forward, out hit, 20000))
                {
                    hitPoint = hit.point;

                    if (team == PunTeams.Team.red)
                    {
                        float _BoxHeight = 0.5f;
                        Vector3 _boxPos = hitPoint;
                        _boxPos.y += _BoxHeight;

                        GameObject clone = PhotonNetwork.Instantiate("ForceFieldRed", _boxPos,
                                                        transform.rotation,
                                                        0) as GameObject;

                    }
                    else
                    {
                        float _BoxHeight = 0.5f;
                        Vector3 _boxPos = hitPoint;
                        _boxPos.y += _BoxHeight;

                        GameObject clone = PhotonNetwork.Instantiate("ForceFieldBlue", _boxPos,
                                                        transform.rotation,
                                                        0) as GameObject;
                    }
                }

            break;
        }

        isReloading = true;
    }




    #endregion 

    #region Health and Armour
    public void DoDamage(int _healthDam)
    {

        playerData.health -= _healthDam;

        if (playerData.health <= 0)
        {
            Reset();
            LevelManager.RespawnPlayer(gameObject);
        }
    }
    #endregion

    #region Interfaces
    public void DestructionModify(int ammo, int clipSize, int MaxAmmo, float reloadTime, float cooldown, float accuracy, int range, int healthdamage, int forcefieldDamage)
    {
        destructionModuleClass.MaxAmmo = MaxAmmo;
        destructionModuleClass.ClipSize = clipSize;
        destructionModuleClass.ReloadTime = reloadTime;
        destructionModuleClass.Ammo += ammo;
        destructionModuleClass.ModuleCooldown = cooldown;
        destructionModuleClass.Accuracy = accuracy;
        destructionModuleClass.Range = range;
        destructionModuleClass.HealthDamage = healthdamage;
        destructionModuleClass.ForceFieldDamage = forcefieldDamage;
        GetComponent<PlayerShoot>().destructionCooldown = cooldown;
    }

    public void ImpairmentModify()
    {
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


    //if(hit.collider.gameObject.GetComponent<PhotonView>())
    //            {
    //                var playerHitPhoton = hit.collider.gameObject.GetComponent<PhotonView>();
    //                playerHitPhoton.RPC("TakeDamage", PhotonTargets.All, HealthDamage, ArmourDamage);
    //            }
                
    //            GameObject clone = PhotonNetwork.Instantiate("HitParticle", hitPoint, transform.rotation, 0) as GameObject;
    //            clone.GetComponentInChildren<ParticleSystem>().startColor = Color.blue;



//#region Object Pooling
////lists where pooled gameobjects in scene are stored
//protected List<GameObject> redBalls = new List<GameObject>();
//protected List<GameObject> blueBalls = new List<GameObject>();
//protected List<GameObject> whiteBalls = new List<GameObject>();
//private int pooledAmount = 10;//amount of each object to pool

////method to initilise pool system must be called in start function and passed light ball prefab array elements
//public void InitPool(int size, GameObject _white, GameObject _red, GameObject _blue)
//{
//    Vector3 spawnPos = new Vector3(100, 100, 100);//object pool position

//    for (int i = 0; i < pooledAmount; i++)
//    {
//        GameObject obj = (GameObject)Instantiate(_white);
//        obj.transform.position = spawnPos;
//        whiteBalls.Add(obj);
//        obj.SetActive(false);
//    }

//    for (int i = 0; i < pooledAmount; i++)
//    {
//        GameObject obj = (GameObject)Instantiate(_red);
//        obj.transform.position = spawnPos;
//        redBalls.Add(obj);
//        obj.SetActive(false);
//    }
//    for (int i = 0; i < pooledAmount; i++)
//    {
//        GameObject obj = (GameObject)Instantiate(_blue);
//        obj.transform.position = spawnPos;
//        blueBalls.Add(obj);
//        obj.SetActive(false);
//    }
//}

//public void TopUpPool(int size, bool topUpWhite, bool topUpRed, bool topUpBlue)
//{
//    Vector3 spawnPos = new Vector3(100, 100, 100);//object pool position

//    if(topUpWhite)
//    {
//        for (int i = 0; i < pooledAmount; i++)
//        {
//            GameObject obj = (GameObject)Instantiate(lightBallPrefabs[0]);
//            obj.transform.position = spawnPos;
//            whiteBalls.Add(obj);
//            obj.SetActive(false);
//        }
//    }
//    else if (topUpRed)
//    {
//        for (int i = 0; i < pooledAmount; i++)
//        {
//            GameObject obj = (GameObject)Instantiate(lightBallPrefabs[1]);
//            obj.transform.position = spawnPos;
//            redBalls.Add(obj);
//            obj.SetActive(false);
//        }
//    }
//    else if (topUpBlue)
//    {
//        for (int i = 0; i < pooledAmount; i++)
//        {
//            GameObject obj = (GameObject)Instantiate(lightBallPrefabs[3]);
//            obj.transform.position = spawnPos;
//            blueBalls.Add(obj);
//            obj.SetActive(false);
//        }
//    }
//}
//#endregion



////Call this method to fire a ball
//public void Fire()
//{
//    GameObject _bullet;

//    switch (ballIndex)
//    {
//        case 0:

//             _bullet = FetchFromPool(whiteBalls);
//             ActivateBullet(_bullet);
//             break;

//        case 1:

//            _bullet = FetchFromPool(redBalls);
//            ActivateBullet(_bullet);
//            break;

//        case 2:

//            _bullet = FetchFromPool(blueBalls);
//            ActivateBullet(_bullet);
//            break;

//    }

//}

////Activate bullet will find a bullet in the pool and move it to the correct position ready to be fired
//private void ActivateBullet(GameObject _bullet)
//{


//    _bullet.SetActive(true);//turn ball on
//    Vector3 bulletPos = gun.transform.TransformPoint(Vector3.forward);//Camera.main.transform.position;//set bullet position
//    _bullet.transform.position = bulletPos;
//    _bullet.transform.rotation = Camera.main.transform.rotation;
//    Rigidbody ballRigidbody = _bullet.GetComponent<Rigidbody>();

//    //raycast from middle of camera to get where cursor is aiming
//    Transform cam = Camera.main.transform;
//    RaycastHit hit;
//    Vector3 hitPoint;

//    if (Physics.Raycast(cam.position, cam.forward, out hit, 20000))
//    {
//        hitPoint = hit.point;

//        if (_bullet.name == "whiteBall(Clone)")
//        {
//            GameObject clone = PhotonNetwork.Instantiate("whiteBall", bulletPos,
//                                                    _bullet.transform.rotation,
//                                                    0) as GameObject;
//            clone.transform.LookAt(hitPoint);//make projectile travel towards raycast hit point and where the reticle was aiming
//        }
//        else if (_bullet.name == "redBall(Clone)")
//        {

//        }
//        else if (_bullet.name == "blueBall(Clone)")
//        {
//            GameObject clone = PhotonNetwork.Instantiate("BlueBox", hitPoint,
//                                                    transform.rotation,
//                                                    0) as GameObject;

//            float _heightCorrection = clone.transform.localScale.y / 2;
//            Vector3 _boxPos = hitPoint;
//            _boxPos.y += _heightCorrection;
//            clone.transform.position = _boxPos;
//        }

//        isReloading = true;
//        _bullet.SetActive(false);
//    }
//}


//private GameObject FetchFromPool(List<GameObject> pool)
//{
//    for (int i = 0; i < pool.Count; i++)
//    {
//        if (!pool[i].activeInHierarchy)
//        {
//            return pool[i].gameObject;
//        }
//    }

//    return null;
//}
