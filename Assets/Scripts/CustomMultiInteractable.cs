using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomMultiInteractable : XRBaseInteractable
{
    // Start is called before the first frame update
    void Start()
    {
        XRBaseInteractor interactor = selectingInteractor;

        IXRSelectInteractor newInteractor = firstInteractorSelecting;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MatchAttachPoint(XRBaseInteractor interactor1, IXRSelectInteractor interactor2)
    {
        bool isDirect = interactor1 is XRDirectInteractor;
        //attachTransform.position = isDirect ? interactor1.attachTransform.position : transform.position;
        //attachTransform.rotation = isDirect ? interactor1.attachTransform.rotation : transform.rotation;

    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if(HasTwoInteractors())
        {
            MatchAttachPoint(selectingInteractor, firstInteractorSelecting);
        }
    }

    private bool HasTwoInteractors()
    {
        return interactorsSelecting.Count == 2;
    }
}
