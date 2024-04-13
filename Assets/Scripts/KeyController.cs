using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This script is on every single key that the player can utilise throughout the game
/// <para>The type of the key can be switched by changing the enum 'KeyType' </para>
/// </summary>
public class KeyController : MonoBehaviour
{
    public LevelSpawner levelSpawner;

    public float keyPressDistance = 0.1f; //How high the key goes
    public float keyPressSpeed = 5f; //How quickly the key moves
    public KeyCode associatedKey; //The keycode for this specific key, set in inspector (Keycode.A for example)
    private bool isActivated = false; //If pressed
    public bool lockedElevated = false; //If it is locked at the extended position (Used for puzzle design)
    public bool permPowerOn = false; //If an object should be powered on, even without a power block next to it

    public bool staticKey = false; //If the key should be locked from new changes by the player (Also puzzle design)

    public GameObject[] connectedKeysObject; //The keys next to it that should be moved up and down if static
    public List<KeyController> connectedKeysCon = new List<KeyController>(); //Reference to the this script on those keys
    public List<KeyController> neighbourKeys;   

    private Vector3 spawnPoint; //The start location of this key, set on start

    public Vector3 initialPosition; //The lower point
    public Vector3 pressedPosition; //The extended point
    private Rigidbody rb;

    public KeyType keyType; //This keys type, set in inspector for each level

    public float attractionRadius = 5f; //Magnet range

    public Material normalMaterial;
    public float magneticForce = 10f;

    public LayerMask magneticLayer; //The layer that the magnet attracts
    public GameObject rodInstance; //The magnets rod to show connection
    public GameObject rodPrefab; 
    public Rigidbody rbMagnet;

    public GameObject stickySkin; //Outer gameobject that displays that the object is a sticky key

    public Rigidbody rbBlow;
    public float blowForce = 0.1f;
    public GameObject fanObject;
    public GameObject blades;

    public Material magnetMaterial;
    public Material inactiveMaterial;

    //Each mesh changes the look of the key due to how I created the keys.
    public Mesh normalMesh;
    public Mesh magnetMesh;
    public Mesh fanMesh;
    public Mesh inactiveMesh;
    public Mesh conveyorMesh;
    public Mesh powerOffMesh;
    public Mesh powerOnMesh;

    public Mesh[] conveyorAnimMesh = new Mesh[6]; //Array of each frame of the conveyor animation
    public float animationInterval = 0.05f;
    private int currentMeshIndex = 0;
    public Direction conveyorDirection;

    private Coroutine conveyorAnimationCoroutine;
    public GameObject conveyorObject;
    public ConveyorPush conveyorPush;
    public bool powerOn = false;
    public bool runOnce = false;

    public bool proxyPowered = false;

    /// <summary>
    /// This objects KeyType. Pick From
    /// <para>Normal</para>
    /// <para>Inactive</para>
    /// <para>Magnet</para>
    /// <para>Fan</para>
    /// <para>Conveyor</para>
    /// <para>Power</para>
    /// <para>Must run 'UpdateKeyType' if changed</para>
    ///       
    /// </summary>
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

    public TextMeshPro keyText; //The text that is on top of the key with the letter

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
        if (connectedKeysObject.Length >= 1)
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

    public void ProxyPower()
    {
        isActivated = true;
        proxyPowered = true;


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

            case KeyType.Fan: //If fan is activated, blow closest object
                if (isActivated)
                {
                    BlowObjects();
                }
                break;

            case KeyType.Conveyor: //Turns on the conveyor object which adds force to any object touching it

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
    /// <summary>
    /// Makes sure proxy powers are turned off properly
    /// </summary>
    public void ProxyPowerOff()
    {
        proxyPowered = false;
    }

    /// <summary>
    /// Contains functionality for pressing the key with a mouse, only utilised by the Conveyor and Power currently
    /// </summary>
    public void Clicked()
    {
        switch (keyType)
        {
            case KeyType.Conveyor: //Spins the conveyor
                if (staticKey != true) //Make sure the player can't ruin a puzzle by spinning conveyors not intended
                {
                    this.gameObject.transform.Rotate(0, -90, 0); //Spin the whole object
                    keyText.transform.rotation = Quaternion.Euler(90, 180, -90); //But make text always face the same way
                }
                break;

            case KeyType.Power:
                if (staticKey != true) //Make sure players can't turn off static powers to ruin puzzles (olly)
                {
                    if (powerOn) //Toggle between different status, on and off.
                    {
                        powerOn = false;
                        this.GetComponent<MeshFilter>().mesh = powerOffMesh;
                    }
                    else
                    {
                        powerOn = true;
                        this.GetComponent<MeshFilter>().mesh = powerOnMesh;
                    }
                }
                break;

        }
    }



    void FixedUpdate()
    {
        if (keyType != KeyType.Inactive)
        {
            //This long line basically checks if the player should be able to move this specific key
            if ((Input.GetKey(associatedKey) && levelSpawner.presses < levelSpawner.levelData[levelSpawner.levelNumber].pressLimits + 1) || (Input.GetKey(associatedKey) && levelSpawner.levelData[levelSpawner.levelNumber].pressLimits == 0))
            {
                MoveKey(pressedPosition); // Move the key to the pressed position
                isActivated = true; // Set the activation flag to true
                if (!levelSpawner.firstPress)
                {
                    levelSpawner.firstPress = true; // Reset the first press flag if needed
                }

                if (!runOnce)
                {
                    runOnce = true; // Increment the number of presses if not already done for this key
                    if (levelSpawner.gameStarted)
                    {
                        levelSpawner.presses += 1;
                    }
                }

                foreach (KeyController key in connectedKeysCon)
                {
                    key.ProxyPress(); // Trigger proxy presses for connected keys
                }
            }
            else if (!proxy && !proxyPowered)
            {
                foreach (KeyController key in connectedKeysCon)
                {
                    key.proxy = false; // Reset proxy flags for connected keys
                }

                if (!isActivated)
                {
                    MoveKey(initialPosition); // Move the key to its initial position if not already activated
                }
                runOnce = false; // Reset runOnce and isActivated flags
                isActivated = false;
            }

            switch (keyType)
            {
                case KeyType.Magnet:
                    if (isActivated)
                    {
                        AttractObjects(); // Attract nearby objects if the key is activated
                    }
                    else
                    {
                        if (rodInstance != null)
                        {
                            rodInstance.SetActive(false); // Deactivate the rod instance if the key is not activated
                        }
                    }
                    break;

                case KeyType.Conveyor:
                    if (isActivated && conveyorAnimationCoroutine == null)
                    {
                        conveyorAnimationCoroutine = StartCoroutine(AnimateConveyor()); // Start conveyor animation coroutine if the key is activated
                        conveyorObject.SetActive(true);
                    }
                    else if (!isActivated && conveyorAnimationCoroutine != null)
                    {
                        conveyorObject.SetActive(false); // Stop conveyor animation coroutine and reset conveyor if the key is not activated
                        StopCoroutine(conveyorAnimationCoroutine);
                        conveyorAnimationCoroutine = null;
                        this.GetComponent<MeshFilter>().mesh = conveyorMesh;
                    }
                    break;

                case KeyType.Fan:
                    if (isActivated)
                    {
                        BlowObjects(); // Apply blowing effect to nearby objects if the key is activated
                    }
                    break;

                case KeyType.Power:
                    if (powerOn)
                    {
                        foreach (KeyController keys in neighbourKeys)
                        {
                            if (keys.keyType == KeyType.Fan || keys.keyType == KeyType.Conveyor || keys.keyType == KeyType.Magnet)
                            {
                                keys.ProxyPower(); // Trigger power proxy for connected keys if power is on
                            }
                        }
                    }
                    else
                    {
                        foreach (KeyController keys in neighbourKeys)
                        {
                            if (keys.keyType == KeyType.Fan || keys.keyType == KeyType.Conveyor || keys.keyType == KeyType.Magnet)
                            {
                                keys.ProxyPowerOff(); // Trigger power off proxy for connected keys if power is off
                            }
                        }
                    }
                    break;
            }
        }
        if (permPowerOn)
        {
            ProxyPower(); // Trigger permanent power proxy if permPowerOn bool is true
        }
    }

    /// <summary>
    /// If pressed from a stickykey next to it
    /// </summary>
    public void ProxyPress()
    {
        if (!isActivated)
        {
            isActivated = true;
            SynchronizeKeys();
        }
    }

    /// <summary>
    /// Ensure all keys in 'StickyKeys' are moving together
    /// </summary>
    void SynchronizeKeys()
    {
        foreach (KeyController key in connectedKeysCon)
        {
            key.MoveKey(key.pressedPosition); //Move to the extended position
        }
    }

    /// <summary>
    /// Moves the key towards the target position using linear interpolation.
    /// </summary>
    /// <param name="targetPosition">The target position to move the key towards.</param>
    void MoveKey(Vector3 targetPosition)
    {
        Vector3 currentPosition = rb.position; // Get the current position of the key

        Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, keyPressSpeed * Time.deltaTime); // Calculate the new position using linear interpolation

        rb.MovePosition(newPosition); // Move the key to the extended position
    }

    /// <summary>
    /// Returns the string representation of a key without the "Alpha" prefix if present.
    /// </summary>
    /// <param name="key">The key code to get the string representation for.</param>
    /// <returns>The string representation of the key without the "Alpha" prefix.</returns>
    string GetKeyStringWithoutAlpha(KeyCode key)
    {

        string keyString = key.ToString(); // Get the string representation of the key

        if (keyString.StartsWith("Alpha")) // Check if the key string starts with "Alpha"
        {
            return keyString.Substring("Alpha".Length); // Return the key string without the "Alpha" prefix
        }

        return keyString; // Return the original key string
    }

    /// <summary>
    /// Animates the conveyor by cycling through different mesh frames.
    /// </summary>
    /// <returns>An IEnumerator for yielding in the coroutine.</returns>
    IEnumerator AnimateConveyor()
    {
        while (true) // Loop indefinitely to animate the conveyor
        {
            currentMeshIndex = (currentMeshIndex + 1) % conveyorAnimMesh.Length; // Update the current mesh index       
            GetComponent<MeshFilter>().mesh = conveyorAnimMesh[currentMeshIndex]; // Set the current mesh frame      
            yield return new WaitForSeconds(animationInterval); // Wait for the specified interval before animating the next frame
        }
    }


    /// <summary>
    /// Attracts nearby objects within the attraction radius towards the magnet.
    /// </summary>
    private void AttractObjects()
    {
        // Find colliders within the attraction radius and on the magnetic layer
        Collider[] colliders = Physics.OverlapSphere(transform.position, attractionRadius, layerMask: magneticLayer);


        foreach (Collider collider in colliders) // Apply attraction force to each collider found
        {

            Rigidbody rbMagnet = collider.GetComponent<Rigidbody>(); // Get the rigidbody component of the collider


            if (rbMagnet != null) // If the collider has a rigidbody component
            {
                Vector3 direction = transform.position - collider.transform.position; // Calculate the direction from the magnet to the collider
                rbMagnet.AddForce(direction.normalized * magneticForce); // Apply force in the direction of the magnet to attract the collider                                                                         // 
                UpdateRod(rbMagnet.transform.position, rbMagnet.gameObject); // Update the rod between the magnet and the attracted object
            }
        }
    }


    /// <summary>
    /// Updates the rod between the fan and the target object.
    /// </summary>
    /// <param name="targetPosition">The position of the target object.</param>
    /// <param name="targetObject">The target object.</param>
    private void UpdateRod(Vector3 targetPosition, GameObject targetObject)
    {

        if (rodInstance == null) // If the rod instance doesn't exist, instantiate it
        {
            rodInstance = Instantiate(rodPrefab, this.gameObject.transform);
        }

        rodInstance.SetActive(true); // Activate the rod instance      
        Vector3 midpoint = (this.gameObject.transform.position + targetObject.transform.position) / 2f; // Calculate the midpoint between the fan and the target object 
        rodInstance.transform.position = midpoint; // Set the rod instance position to the midpoint   
        rodInstance.transform.LookAt(targetObject.transform.position); // Rotate the rod instance to face the target object       
        float distance = Vector3.Distance(this.gameObject.transform.position, targetObject.transform.position); // Calculate the distance between the fan and the target object   
        rodInstance.transform.localScale = new Vector3(0.3f, 0.3f, distance); // Set the scale of the rod instance to represent the distance between them
    }



    /// <summary>
    /// Applies a force to nearby objects within the attraction radius to simulate blowing effect.
    /// </summary>
    private void BlowObjects()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attractionRadius, layerMask: magneticLayer); // Find colliders within the attraction radius and on the magnetic layer

        foreach (Collider collider in colliders) // Apply force to each collider found
        {
            Rigidbody rbBlow = collider.GetComponent<Rigidbody>(); // Get the rigidbody component of the collider

            if (rbBlow != null) // If the collider has a rigidbody component
            {
                Vector3 direction = this.transform.position - collider.transform.position; // Calculate the direction from the fan to the collider            
                rbBlow.AddForce(-direction * blowForce); // Apply force in the opposite direction to simulate blowing
                fanObject.transform.LookAt(rbBlow.gameObject.transform); // Rotate the fan object to face the collider             
                blades.transform.Rotate(0, 0, 10); // Rotate the blades of the fan at full speed
            }
        }
        blades.transform.Rotate(0, 0, 5f); // Rotate the blades of the fan half as fast to show not currently blowing anything
    }
}