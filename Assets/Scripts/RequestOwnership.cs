using Normal.Realtime;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class RequestOwnership : MonoBehaviour
{
    [SerializeField] private RealtimeView realtimeView;
    [SerializeField] private RealtimeTransform realtimeTransform;
    [SerializeField] private XRGrabInteractable xRGrabInteractable;

    private void OnEnable()
    {
        xRGrabInteractable.selectEntered.AddListener(RequestbjectOwnership);
    }

    private void RequestbjectOwnership(SelectEnterEventArgs args)
    {
        realtimeView.RequestOwnership();
        realtimeTransform.RequestOwnership();
    }

    private void OnDisable()
    {
        xRGrabInteractable.selectEntered.RemoveListener(RequestbjectOwnership);
    }
}
