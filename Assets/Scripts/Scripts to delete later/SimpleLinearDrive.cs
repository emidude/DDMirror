using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class SimpleLinearDrive : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;
    public LinearMapping linearMapping;
    public bool repositionGameObject = true;

    protected float initialMappingOffset;
    protected int numMappingChangeSamples = 5;
    protected float[] mappingChangeSamples;
    protected float prevMapping = 0.0f;
    protected float mappingChangeRate;
    protected int sampleCount = 0;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        mappingChangeSamples = new float[numMappingChangeSamples];
    }

    protected virtual void Start()
    {
        if (linearMapping == null)
        {
            linearMapping = GetComponent<LinearMapping>();
        }

        if (linearMapping == null)
        {
            linearMapping = gameObject.AddComponent<LinearMapping>();
        }

        initialMappingOffset = linearMapping.value;

        if (repositionGameObject)
        {
            UpdateLinearMapping(transform);
        }
    }

    public void UpdateLinearMapping(Transform updateTransform)
    {
        prevMapping = linearMapping.value;
        linearMapping.value = Mathf.Clamp01(initialMappingOffset + CalculateLinearMapping(updateTransform));

        mappingChangeSamples[sampleCount % mappingChangeSamples.Length] = (1.0f / Time.deltaTime) * (linearMapping.value - prevMapping);
        sampleCount++;

        if (repositionGameObject)
        {
            transform.position = Vector3.Lerp(startPosition.position, endPosition.position, linearMapping.value);
        }
    }

    protected float CalculateLinearMapping( Transform updateTransform )
		{
			Vector3 direction = endPosition.position - startPosition.position;
			float length = direction.magnitude;
			direction.Normalize();

			Vector3 displacement = updateTransform.position - startPosition.position;

			return Vector3.Dot( displacement, direction ) / length;
		}

    public virtual void HandHvrUpdate(Transform handTf)
    {
            initialMappingOffset = linearMapping.value - CalculateLinearMapping(handTf);
            sampleCount = 0;
            mappingChangeRate = 0.0f;
    }

   /* protected virtual void HandAttachedUpdate(Hand hand)
    {
        UpdateLinearMapping(hand.transform);

        if (hand.IsGrabEnding(this.gameObject))
        {
            hand.DetachObject(gameObject);
        }
    }

    protected virtual void OnDetachedFromHand(Hand hand)
    {
        CalculateMappingChangeRate();
    }*/


    protected void CalculateMappingChangeRate()
    {
        //Compute the mapping change rate
        mappingChangeRate = 0.0f;
        int mappingSamplesCount = Mathf.Min(sampleCount, mappingChangeSamples.Length);
        if (mappingSamplesCount != 0)
        {
            for (int i = 0; i < mappingSamplesCount; ++i)
            {
                mappingChangeRate += mappingChangeSamples[i];
            }
            mappingChangeRate /= mappingSamplesCount;
        }
    }


}
