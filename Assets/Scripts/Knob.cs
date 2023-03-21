/*
using UnityEngine;
using UnityEngine.XR;

public class Knob : MonoBehaviour
{
    public GameObject videoPlayer;
    private MyVideoPlayer videoPlayerScript;

    void Start()
    {
        videoPlayerScript = videoPlayer.GetComponent<MyVideoPlayer>();
    }

    void OnMouseDown()
    {
        videoPlayerScript.KnobOnPressDown();
    }

    void OnMouseUp()
    {
        videoPlayerScript.KnobOnRelease();
    }

    void OnMouseDrag()
    {
        videoPlayerScript.KnobOnDrag();
    }

}
*/
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Knob : MonoBehaviour
{
    public GameObject videoPlayer;
    private MyVideoPlayer videoPlayerScript;

    private XRGrabInteractable grabInteractable;
    public XRBaseInteractor interactor;

    void Start()
    {
        videoPlayerScript = videoPlayer.GetComponent<MyVideoPlayer>();

        // get the XRGrabInteractable component and add event listeners
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEntered.AddListener(OnGrab);
        grabInteractable.onSelectExited.AddListener(OnRelease);
        grabInteractable.onSelectEntered.AddListener(OnDrag);
    }

    void OnGrab(XRBaseInteractor interactor)
    {
        videoPlayerScript.KnobOnPressDown(interactor);
    }

    void OnRelease(XRBaseInteractor interactor)
    {
        videoPlayerScript.KnobOnRelease(interactor);
    }

    void OnDrag(XRBaseInteractor interactor)
    {
        videoPlayerScript.KnobOnDrag(interactor);
    }
}

