using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Annotation : MonoBehaviour
{
    public static Annotation instance;
    public GameObject joint;
    Transform jointSelection;
    public GameObject inputField;
    public List<GameObject> inputs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddAnnotation()
    {
        Debug.Log("A joint is selected!");
        jointSelection = joint.transform;
        GameObject box = new GameObject();
        box = Instantiate(inputField, Vector3.zero, Quaternion.Euler(new Vector3(0, 0, 0))); //tranform.rotation to camera
                                                                                             //posit = Camera.main.WorldToScreenPoint(jointSelection.transform.position);
        box.transform.position = new Vector3(jointSelection.transform.position.x + 0.1f, jointSelection.transform.position.y, jointSelection.transform.position.z);
        //box.transform.parent = GameObject.FindGameObjectWithTag("Selectable").transform;
        box.transform.parent = jointSelection.transform;
        //textBoxPrefab.transform.SetParent(jointSelection, false);
        box.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        inputs.Add(box);
    }

    

    void Awake()
    {
        // If the instance reference has not been set, yet, 
        if (instance == null)
        {
            // Set this instance as the instance reference.
            instance = this;
        }
        /*
        else if (instance != this)
        {
            // If the instance reference has already been set, and this is not the
            // the instance reference, destroy this game object.
            Destroy(gameObject);
        }
        */
        // Do not destroy this object, when we load a new scene.
        DontDestroyOnLoad(gameObject);
    }
}
