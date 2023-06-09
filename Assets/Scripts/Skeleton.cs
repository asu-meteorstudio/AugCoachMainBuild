﻿using UnityEngine;
using System.Collections;
using com.rfilkov.kinect;
using System.Collections.Generic;

namespace com.rfilkov.components
{
    /// <summary>
    /// SensorSkeletonOverlayer displays the the body joints and bones, as detected by a specific sensor, with spheres and lines.
    /// </summary>
    public class Skeleton : MonoBehaviour
    {
        [Tooltip("Depth sensor index - 0 is the 1st one, 1 - the 2nd one, etc.")]
        public int sensorIndex = 0;

        [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
        public int playerIndex = 0;

        [Tooltip("Game object used to overlay the joints.")]
        public GameObject jointPrefab;

        [Tooltip("Line object used to overlay the bones.")]
        public LineRenderer linePrefab;
        //public float smoothFactor = 10f;

        [Tooltip("Camera that will be used to overlay the 3D-objects over the background.")]
        public Camera foregroundCamera;

        [Tooltip("Scene object that will be used to represent the sensor's position and rotation in the scene.")]
        public Transform sensorTransform;

        [Tooltip("Color of the skeleton bones.")]
        public Color skeletonColor = Color.blue;

        //public UnityEngine.UI.Text debugText;

        private GameObject[] joints = null;
        private LineRenderer[] lines = null;

        // initial body rotation
        private Quaternion initialRotation = Quaternion.identity;

        // reference to KM
        private KinectManager kinectManager = null;

        // background rectangle
        private Rect backgroundRect = Rect.zero;

        float worldScale;

        RaycastHit hit;
        public GameObject inputField;
        private Transform _selection;
        Ray handRay;
        Transform jointSelection;
        [SerializeField] private Material highlightedMaterial;
        [SerializeField] private Material defMaterial;
        [SerializeField] private string selectableTag = "Selectable";

        //[SerializeField] GameObject textBoxPrefab;

        public List<GameObject> inputs = new List<GameObject>();


        void Start()
        {
            kinectManager = KinectManager.Instance;

            if (kinectManager && kinectManager.IsInitialized())
            {
            }

            // always mirrored
            initialRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

            //if (!foregroundCamera)
            //{
            //    // by default - the main camera
            //    foregroundCamera = Camera.main;
            //}
            worldScale = transform.lossyScale[0];
        }

        public void JointChoice(Ray ray)
        {

            int cnt = 0;
            //ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //deselect
            //_selection = null;
            if (_selection != null)
            {
                jointSelection = hit.transform;
                var selectionRenderer = jointSelection.GetComponent<Renderer>();
                selectionRenderer.material = defMaterial;
                _selection = null;
            }
            //select
            LayerMask mask = LayerMask.GetMask("Annotation");
            if (Physics.Raycast(ray, out hit, mask))
            {
                jointSelection = hit.transform;
                var selectionRenderer = jointSelection.GetComponent<Renderer>();
                if (jointSelection.CompareTag("Selectable"))
                {
                             string jointTag = jointSelection.gameObject.tag;
                    print(jointTag);
                    if (selectionRenderer != null)
                    {
                        print("Hand ray hit!");
                        selectionRenderer.material = highlightedMaterial;
                        Debug.Log("selected");
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
                    _selection = jointSelection;
                }
            }
        }

        void Update()
        {
            print("Already got there!");
            /*
            if (InputRayUtils.TryGetHandRay(Handedness.Right, out handRay) && (EnterAnnotationMode.instance.isAnnotation == true))
            {
                JointChoice(handRay);
                print("Hand ray detected!");
            }
            */
            if (kinectManager && kinectManager.IsInitialized())
            {
                if (foregroundCamera)
                {
                    // get the background rectangle (use the portrait background, if available)
                    backgroundRect = foregroundCamera.pixelRect;
                    PortraitBackground portraitBack = PortraitBackground.Instance;

                    if (portraitBack && portraitBack.enabled)
                    {
                        backgroundRect = portraitBack.GetBackgroundRect();
                    }
                }
                
                // overlay all joints in the skeleton
                if (kinectManager.IsSensorBodyDetected(sensorIndex, playerIndex))
                {
                    int jointsCount = kinectManager.GetJointCount();

                    if (joints == null && jointPrefab != null)
                    {
                        // array holding the skeleton joints
                        joints = new GameObject[jointsCount];

                        for (int i = 0; i < joints.Length; i++)
                        {
                            joints[i] = Instantiate(jointPrefab) as GameObject;
                            joints[i].transform.parent = transform;
                            joints[i].name = ((KinectInterop.JointType)i).ToString();
                            joints[i].SetActive(false);

                            Renderer renderer = joints[i].GetComponent<Renderer>();
                            if (renderer != null)
                            {
                                renderer.material.color = skeletonColor;
                            }
                        }
                    }

                    if (lines == null)
                    {
                        // array holding the skeleton lines
                        lines = new LineRenderer[jointsCount];
                    }

                    

                    for (int i = 0; i < jointsCount; i++)
                    {
                        int joint = i;

                        if (kinectManager.IsSensorJointTracked(sensorIndex, playerIndex, joint))
                        {
                            Vector3 posJoint = GetJointPosition(joint);
                            if (sensorTransform)
                            {
                                posJoint = sensorTransform.TransformPoint(posJoint);
                                
                            }

                            posJoint *= worldScale;

                            if (joints != null)
                            {
                                // overlay the joint
                                if (posJoint != Vector3.zero)
                                {
                                    joints[i].SetActive(true);
                                    
                                    
                                    joints[i].transform.position = posJoint;

                                    Quaternion rotJoint = kinectManager.GetSensorJointOrientation(sensorIndex, playerIndex, joint, false);
                                    rotJoint = initialRotation * rotJoint;
                                    
                                    joints[i].transform.rotation = rotJoint;

                                    //if (i == (int)KinectInterop.JointType.WristLeft)
                                    //{
                                    //    Debug.Log(string.Format("SSO {0:F3} {1} user: {2}, state: {3}\npos: {4}, rot: {5}", Time.time, (KinectInterop.JointType)i,
                                    //        playerIndex, kinectManager.GetSensorJointTrackingState(sensorIndex, playerIndex, joint),
                                    //        kinectManager.GetSensorJointPosition(sensorIndex, playerIndex, joint), 
                                    //        kinectManager.GetSensorJointOrientation(sensorIndex, playerIndex, joint, false).eulerAngles));
                                    //}
                                }
                                else
                                {
                                    joints[i].SetActive(false);
                                }
                            }

                            if (lines[i] == null && linePrefab != null)
                            {
                                lines[i] = Instantiate(linePrefab) as LineRenderer;
                                lines[i].transform.parent = transform;
                                lines[i].gameObject.SetActive(false);

                                lines[i].startColor = skeletonColor;
                                lines[i].endColor = skeletonColor;
                            }

                            if (lines[i] != null)
                            {
                                // overlay the line to the parent joint
                                int jointParent = (int)kinectManager.GetParentJoint((KinectInterop.JointType)joint);
                                Vector3 posParent = GetJointPosition(jointParent);
                                posParent *= worldScale;

                                if (sensorTransform)
                                {
                                    posParent = sensorTransform.TransformPoint(posParent);
                                }

                                if (posJoint != Vector3.zero && posParent != Vector3.zero)
                                {
                                    lines[i].gameObject.SetActive(true);

                                    //lines[i].SetVertexCount(2);
                                    lines[i].SetPosition(0, posParent);
                                    lines[i].SetPosition(1, posJoint);
                                }
                                else
                                {
                                    lines[i].gameObject.SetActive(false);
                                }
                            }
                        }
                        else
                        {
                            if (joints[i] != null)
                            {
                                joints[i].SetActive(false);
                            }

                            if (lines[i] != null)
                            {
                                lines[i].gameObject.SetActive(false);
                            }
                        }
                    }

                }
                else
                {
                    // disable the skeleton
                    int jointsCount = kinectManager.GetJointCount();

                    for (int i = 0; i < jointsCount; i++)
                    {
                        if (joints != null && joints[i] != null)
                        {
                            joints[i].SetActive(false);
                        }

                        if (lines != null && lines[i] != null)
                        {
                            lines[i].gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        // returns body joint position
        private Vector3 GetJointPosition(int joint)
        {
            Vector3 posJoint = Vector3.zero;

            if (foregroundCamera)
            {
                Vector3 posJointKinect = kinectManager.GetSensorJointKinectPosition(sensorIndex, playerIndex, joint, false);
                posJoint = kinectManager.GetJointPosColorOverlay(posJointKinect, sensorIndex, foregroundCamera, backgroundRect);
            }
            else if (sensorTransform)
            {
                posJoint = kinectManager.GetSensorJointKinectPosition(sensorIndex, playerIndex, joint, true);
            }
            else
            {
                posJoint = kinectManager.GetSensorJointPosition(sensorIndex, playerIndex, joint);
            }

            return posJoint;
        }

    }
}
