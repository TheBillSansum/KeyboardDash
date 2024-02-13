using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UIElements;

public class KeyController : MonoBehaviour
{
    public float keyPressDistance = 0.1f;
    public float keyPressSpeed = 5f;
    public KeyCode associatedKey;
    private bool isActivated = false;

    public GameObject[] connectedKeysObject;
    public List<KeyController> connectedKeysCon = new List<KeyController>();

    private Vector3 initialPosition;
    private Vector3 pressedPosition;
    private Rigidbody rb;

    public KeyType keyType;

    public float attractionRadius = 5f;


    public Material normalMaterial;
    public float magneticForce = 10f;

    public LayerMask magneticLayer;
    public GameObject rodInstance;
    public GameObject rodPrefab;
    public Rigidbody rbMagnet;

    public Rigidbody rbBlow;
    public float blowForce = 0.1f;
    public GameObject fanObject;
    public GameObject blades;

    public Material magnetMaterial;
    public Material inactiveMaterial;

    public enum KeyType
    {
        Normal,
        Inactive,
        Magnet,
        Fan
    }

    public TextMeshPro keyText;

    void Start()
    {
        foreach (GameObject key in connectedKeysObject)
        {
            connectedKeysCon.Add(key.GetComponent<KeyController>());
        }
        initialPosition = transform.position;
        pressedPosition = initialPosition + Vector3.up * keyPressDistance;

        switch (keyType)
        {
            case KeyType.Magnet:

                this.GetComponent<MeshRenderer>().material = magnetMaterial;
                break;

            case KeyType.Inactive:
                this.GetComponent<MeshRenderer>().material = inactiveMaterial;
                break;

            case KeyType.Fan:
                fanObject.SetActive(true);
                break;
        }



        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;


        if (keyText != null)
        {
            keyText.text = GetKeyStringWithoutAlpha(associatedKey);
        }
    }
    bool proxy;
    public void ProxyPress()
    {
        MoveKey(pressedPosition);
        isActivated = true;
        Debug.Log("Proxy Pressed" + this.gameObject.name);
        proxy = true;
    }



    void FixedUpdate()
    {
        if (keyType != KeyType.Inactive)
        {
            if (Input.GetKey(associatedKey))
            {
                MoveKey(pressedPosition);
                isActivated = true;

                int i = 0;
                foreach (KeyController key in connectedKeysCon)
                {
                    key.ProxyPress();
                    Debug.Log("Called");
                }
            }
            else if(!proxy)
            {
                foreach(KeyController key in connectedKeysCon)
                {
                    key.proxy = false;
                }
                MoveKey(initialPosition);
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


                case KeyType.Fan:
                    if (isActivated)
                    {
                        BlowObjects();
                    }
                    break;
            }
        }
    }


    void MoveKey(Vector3 targetPosition)
    {
        Vector3 currentPosition = rb.position;
        Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, keyPressSpeed * Time.deltaTime);
        rb.MovePosition(newPosition);
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
        rodInstance.transform.localScale = new Vector3(0.3f, 0.3f, distance - 1);

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

                blades.transform.Rotate(0, 0, 1);
            }
        }
    }
}