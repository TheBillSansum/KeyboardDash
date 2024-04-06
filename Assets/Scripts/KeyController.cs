using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyController : MonoBehaviour
{
    public LevelSpawner levelSpawner;

    public float keyPressDistance = 0.1f;
    public float keyPressSpeed = 5f;
    public KeyCode associatedKey;
    private bool isActivated = false;
    public bool lockedElevated = false;
    public bool permPowerOn = false;

    public bool staticKey = false;

    public GameObject[] connectedKeysObject;
    public List<KeyController> connectedKeysCon = new List<KeyController>();
    public List<KeyController> neighbourKeys;

    private Vector3 spawnPoint;

    public Vector3 initialPosition;
    public Vector3 pressedPosition;
    private Rigidbody rb;

    public KeyType keyType;

    public float attractionRadius = 5f;


    public Material normalMaterial;
    public float magneticForce = 10f;

    public LayerMask magneticLayer;
    public GameObject rodInstance;
    public GameObject rodPrefab;
    public Rigidbody rbMagnet;

    public GameObject stickySkin;

    public Rigidbody rbBlow;
    public float blowForce = 0.1f;
    public GameObject fanObject;
    public GameObject blades;

    public Material magnetMaterial;
    public Material inactiveMaterial;

    public Mesh normalMesh;
    public Mesh magnetMesh;
    public Mesh fanMesh;
    public Mesh inactiveMesh;
    public Mesh conveyorMesh;
    public Mesh powerOffMesh;
    public Mesh powerOnMesh;

    public Mesh[] conveyorAnimMesh = new Mesh[6];
    public float animationInterval = 0.05f;
    private int currentMeshIndex = 0;
    public Direction conveyorDirection;

    private Coroutine conveyorAnimationCoroutine;
    public GameObject conveyorObject;
    public ConveyorPush conveyorPush;
    public bool powerOn = false;
    public bool runOnce = false;

    public bool proxyPowered = false;

    public enum KeyType
    {
        Normal,
        Inactive,
        Magnet,
        Fan,
        Conveyor,
        Power
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public TextMeshPro keyText;

    void Start()
    {
        levelSpawner = GameObject.FindAnyObjectByType<LevelSpawner>();
        spawnPoint = transform.position;
        foreach (GameObject key in connectedKeysObject)
        {
            connectedKeysCon.Add(key.GetComponent<KeyController>());
            key.GetComponent<KeyController>().stickySkin.SetActive(true);
            key.GetComponent<KeyController>().connectedKeysObject = connectedKeysObject;
        }
        if(connectedKeysObject.Length >= 1)
        {
            stickySkin.SetActive(true);
        }
        initialPosition = transform.position;
        pressedPosition = initialPosition + Vector3.up * keyPressDistance;

        UpdateKeyType();



        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;


        if (keyText != null)
        {
            keyText.text = GetKeyStringWithoutAlpha(associatedKey);
        }
    }
    bool proxy;

    public void UpdateKeyType()
    {
        this.transform.position = spawnPoint;
        powerOn = false;
        fanObject.SetActive(false);
        this.gameObject.transform.rotation = Quaternion.identity;
        keyText.transform.rotation = Quaternion.Euler(90, 180, -90);
        keyText.gameObject.SetActive(true);


        if (lockedElevated)
        {
            transform.position = pressedPosition;
            pressedPosition = initialPosition + Vector3.down * 0.03f * keyPressDistance;
            initialPosition = transform.position;
        }
        else
        {
            pressedPosition = initialPosition + Vector3.up * keyPressDistance;
        }



        switch (keyType)
        {

            case KeyType.Normal:
                this.GetComponent<MeshFilter>().mesh = normalMesh;
                break;

            case KeyType.Magnet:
                this.GetComponent<MeshFilter>().mesh = magnetMesh;
                break;

            case KeyType.Inactive:
                this.GetComponent<MeshFilter>().mesh = inactiveMesh;
                break;

            case KeyType.Fan:
                fanObject.SetActive(true);
                this.GetComponent<MeshFilter>().mesh = fanMesh;
                keyText.gameObject.SetActive(false);
                pressedPosition = transform.position;
                break;

            case KeyType.Conveyor:
                this.GetComponent<MeshFilter>().mesh = conveyorMesh;
                pressedPosition = transform.position;

                switch (conveyorDirection)
                {
                    case Direction.Down:
                        this.gameObject.transform.Rotate(0, 0, 0);
                        conveyorPush.conveyorDirection_ = ConveyorPush.Direction.Down;
                        keyText.transform.rotation = Quaternion.Euler(90, 180, -90);
                        break;

                    case Direction.Right:
                        this.gameObject.transform.Rotate(0, -90, 0);
                        conveyorPush.conveyorDirection_ = ConveyorPush.Direction.Right;
                        keyText.transform.rotation = Quaternion.Euler(90, 180, -90);
                        break;

                    case Direction.Up:
                        this.gameObject.transform.Rotate(0, -180, 0);
                        conveyorPush.conveyorDirection_ = ConveyorPush.Direction.Up;
                        keyText.transform.rotation = Quaternion.Euler(90, 180, -90);
                        break;

                    case Direction.Left:
                        this.gameObject.transform.Rotate(0, -270, 0);
                        conveyorPush.conveyorDirection_ = ConveyorPush.Direction.Left;
                        keyText.transform.rotation = Quaternion.Euler(90, 180, -90);
                        break;
                }
                break;

            case KeyType.Power:
                this.GetComponent<MeshFilter>().mesh = powerOffMesh;
                pressedPosition = transform.position;
                if (powerOn)
                {
                    powerOn = false;
                    this.GetComponent<MeshFilter>().mesh = powerOffMesh;
                }
                else
                {
                    powerOn = true;
                    this.GetComponent<MeshFilter>().mesh = powerOnMesh;
                }
                break;

        }
    }
    //public void ProxyPress()
    //{     
    //    isActivated = true;    
    //    proxy = true;
    //    MoveKey(pressedPosition);
    //    Debug.Log("Proxy Pressed" + this.gameObject.name);

    //}

    public void ProxyPower()
    {
        isActivated = true;
        proxyPowered = true;

        Debug.Log("Proxy Powered" + this.gameObject.name);

        switch (keyType)
        {

            case KeyType.Magnet:

                if (isActivated)
                {
                    AttractObjects();
                }
                else
                {
                    if (rodInstance != null)
                    {
                        rodInstance.SetActive(false);
                    }
                }
                break;

            case KeyType.Fan:
                if (isActivated)
                {
                    BlowObjects();
                }
                break;

            case KeyType.Conveyor:

                if (isActivated && conveyorAnimationCoroutine == null)
                {
                    conveyorAnimationCoroutine = StartCoroutine(AnimateConveyor());
                    conveyorObject.SetActive(true);
                    

                }
                else if (!isActivated && conveyorAnimationCoroutine != null)
                {
                    conveyorObject.SetActive(false);
                    StopCoroutine(conveyorAnimationCoroutine);
                    conveyorAnimationCoroutine = null;
                    this.GetComponent<MeshFilter>().mesh = conveyorMesh;
                }
                break;
        }


    }

    public void ProxyPowerOff()
    {
        proxyPowered = false;
    }

    public void Clicked()
    {
        switch (keyType)
        {
            case KeyType.Conveyor:
                if (staticKey != true)
                {
                    this.gameObject.transform.Rotate(0, -90, 0);//Rotate(0, -90, 0);
                    keyText.transform.rotation = Quaternion.Euler(90, 180, -90);//Rotate(0, -90, 0)
                }
                break;

            case KeyType.Power:
                if (powerOn)
                {
                    powerOn = false;
                    this.GetComponent<MeshFilter>().mesh = powerOffMesh;
                }
                else
                {
                    powerOn = true;
                    this.GetComponent<MeshFilter>().mesh = powerOnMesh;
                }
                break;
        }
    }



    void FixedUpdate()
    {
        if (keyType != KeyType.Inactive)
        {
            if (Input.GetKey(associatedKey) && levelSpawner.presses < levelSpawner.levelData[levelSpawner.levelNumber].pressLimits + 1 || Input.GetKey(associatedKey) && levelSpawner.levelData[levelSpawner.levelNumber].pressLimits == 0)
            {
                MoveKey(pressedPosition);
                isActivated = true;
                if (levelSpawner.firstPress != true)
                {
                    levelSpawner.firstPress = true;
                }

                if (runOnce == false)
                {
                    runOnce = true;
                    if (levelSpawner.gameStarted)
                    {
                        levelSpawner.presses += 1;
                    }
                }

                foreach (KeyController key in connectedKeysCon)
                {
                   key.ProxyPress();
                }
            }
            else if (!proxy && !proxyPowered)
            {
                foreach (KeyController key in connectedKeysCon)
                {
                    key.proxy = false;
                }

                if (isActivated == false)
                {
                    MoveKey(initialPosition);
                }
                runOnce = false;
                isActivated = false;
            }



            switch (keyType)
            {
                case KeyType.Magnet:

                    if (isActivated)
                    {
                        AttractObjects();
                    }
                    else
                    {
                        if (rodInstance != null)
                        {
                            rodInstance.SetActive(false);
                        }
                    }
                    break;

                case KeyType.Conveyor:

                    if (isActivated && conveyorAnimationCoroutine == null)
                    {
                        conveyorAnimationCoroutine = StartCoroutine(AnimateConveyor());
                        conveyorObject.SetActive(true);


                    }
                    else if (!isActivated && conveyorAnimationCoroutine != null)
                    {
                        conveyorObject.SetActive(false);
                        StopCoroutine(conveyorAnimationCoroutine);
                        conveyorAnimationCoroutine = null;
                        this.GetComponent<MeshFilter>().mesh = conveyorMesh;
                    }
                    break;


                case KeyType.Fan:
                    if (isActivated)
                    {
                        BlowObjects();
                    }
                    break;

                case KeyType.Power:
                    if (powerOn)
                    {
                        foreach (KeyController keys in neighbourKeys)
                        {
                            if (keys.keyType == KeyType.Fan || keys.keyType == KeyType.Conveyor || keys.keyType == KeyType.Magnet)
                            {
                                keys.ProxyPower();
                            }
                        }
                    }
                    else
                    {
                        foreach (KeyController keys in neighbourKeys)
                        {
                            if (keys.keyType == KeyType.Fan || keys.keyType == KeyType.Conveyor || keys.keyType == KeyType.Magnet)
                            {
                                keys.ProxyPowerOff();
                            }
                        }
                    }
                    break;
            }
        }
        if (permPowerOn)
        {
            ProxyPower();
        }
    }

    public void ProxyPress()
    {
        if (!isActivated)
        {
            isActivated = true;
            SynchronizeKeys();
            Debug.Log("Proxy Pressed" + this.gameObject.name);
        }
    }

    void SynchronizeKeys()
    {
        foreach (KeyController key in connectedKeysCon)
        {
            key.MoveKey(key.pressedPosition);
            Debug.Log("Run");
        }
    }

    void MoveKey(Vector3 targetPosition)
    {
        Vector3 currentPosition = rb.position;
        Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, keyPressSpeed * Time.deltaTime);
        rb.MovePosition(newPosition);

        // Check if all connected keys have reached the target position
        //bool allKeysReachedTarget = true;
        //foreach (KeyController key in connectedKeysCon)
        //{
        //    if (Vector3.Distance(key.transform.position, targetPosition) > 0.001f)
        //    {
        //        allKeysReachedTarget = false;
        //        break;
        //    }
        //}

        //// If all connected keys have reached the target, reset isActivated flag
        //if (allKeysReachedTarget)
        //{
        //    isActivated = false;
        //}
    }

    string GetKeyStringWithoutAlpha(KeyCode key)
    {
        string keyString = key.ToString();
        if (keyString.StartsWith("Alpha"))
        {
            return keyString.Substring("Alpha".Length);
        }
        return keyString;
    }

    IEnumerator AnimateConveyor()
    {
        while (true)
        {
            currentMeshIndex = (currentMeshIndex + 1) % conveyorAnimMesh.Length;

            GetComponent<MeshFilter>().mesh = conveyorAnimMesh[currentMeshIndex];

            yield return new WaitForSeconds(animationInterval);
        }
    }

    private void AttractObjects()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attractionRadius, layerMask: magneticLayer);

        foreach (Collider collider in colliders)
        {
            Rigidbody rbMagnet = collider.GetComponent<Rigidbody>();

            if (rbMagnet != null)
            {
                Vector3 direction = transform.position - collider.transform.position;
                rbMagnet.AddForce(direction.normalized * magneticForce);

                UpdateRod(rbMagnet.transform.position, rbMagnet.gameObject);

            }
        }
    }

    private void UpdateRod(Vector3 targetPosition, GameObject targetObject)
    {
        if (rodInstance == null)
        {
            rodInstance = Instantiate(rodPrefab, this.gameObject.transform);
        }
        rodInstance.SetActive(true);
        Debug.Log(targetObject);

        Vector3 midpoint = (this.gameObject.transform.position + targetObject.transform.position) / 2f;
        rodInstance.transform.position = midpoint;


        rodInstance.transform.LookAt(targetObject.transform.position);

        float distance = Vector3.Distance(this.gameObject.transform.position, targetObject.transform.position);
        rodInstance.transform.localScale = new Vector3(0.3f, 0.3f, distance);

    }

    private void BlowObjects()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attractionRadius, layerMask: magneticLayer);

        foreach (Collider collider in colliders)
        {

            Debug.Log(collider);
            Rigidbody rbBlow = collider.GetComponent<Rigidbody>();

            if (rbBlow != null)
            {
                Vector3 direction = this.transform.position - collider.transform.position;
                rbBlow.AddForce(-direction * blowForce);
                fanObject.transform.LookAt(rbBlow.gameObject.transform);
                blades.transform.Rotate(0, 0, 10);
            }
        }
        blades.transform.Rotate(0, 0, 5f);
    }
}