using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interpolator : MonoBehaviour
{
    [SerializeField] private float timeElapsed = 0f;
    [SerializeField] private float timeToReachTarget = 0.05f;
    [SerializeField] private float movementThreshold = 0.05f;

    private readonly List<TransformUpdate> futureTransformUpdates = new();
    private float squareMovementThreshold;
    private TransformUpdate to;
    private TransformUpdate from;
    private TransformUpdate previous;

    private void Start()
    {
        squareMovementThreshold = movementThreshold * movementThreshold;
        to = new TransformUpdate(NetworkManager.Singleton.ServerTick, transform.position);
        from = new TransformUpdate(NetworkManager.Singleton.InterpolationTick, transform.position);
        previous = new TransformUpdate(NetworkManager.Singleton.InterpolationTick, transform.position);
    }
    private void Update()
    {
        for(int i = 0; i < futureTransformUpdates.Count; i++)
        {
            if(NetworkManager.Singleton.ServerTick >= futureTransformUpdates[i].Tick)
            {
                previous = to;
                to = futureTransformUpdates[i];
                from = new TransformUpdate(NetworkManager.Singleton.InterpolationTick, transform.position);
                futureTransformUpdates.RemoveAt(i);
                i--;
                timeElapsed = 0f;
                timeToReachTarget = (to.Tick - from.Tick) * Time.fixedDeltaTime;
            }
        }
        timeElapsed += Time.deltaTime;
        InterpolatePosition(timeElapsed / timeToReachTarget);
    }
    private void InterpolatePosition(float lerpAmount)
    {
        if (float.IsNaN(lerpAmount) || float.IsInfinity(lerpAmount))
        {
            Debug.LogWarning("Invalid lerpAmount detected");
            return;
        }

        if (from.Position == Vector3.positiveInfinity || from.Position == Vector3.negativeInfinity || to.Position == Vector3.positiveInfinity || to.Position == Vector3.negativeInfinity ||
            (from.Position == null) || to.Position  == null)
        {
            Debug.LogWarning("Invalid position detected");
            return;
        }
        if ((to.Position - previous.Position).sqrMagnitude < squareMovementThreshold)
        {
            if (to.Position != from.Position)
            {

                transform.position = Vector3.Lerp(from.Position, to.Position, lerpAmount);

            }
            return;
        }

        if(!(to.Position.IsUnityNull() || from.Position.IsUnityNull() || Vector3.LerpUnclamped(from.Position, to.Position, lerpAmount).z > 100000))
        {
            try
            {
                transform.position = Vector3.LerpUnclamped(from.Position, to.Position, lerpAmount);
            }
            catch
            {
                print("Wierd error that i dont know how to fix");

            }

        }
    }
    public void NewUpdate(ushort tick, Vector3 position)
    {
        
        if (tick <= NetworkManager.Singleton.InterpolationTick)
        {
            return;
        }
        for(int i = 0; i < futureTransformUpdates.Count; i++)
        {
            if(tick < futureTransformUpdates[i].Tick)
            {
                futureTransformUpdates.Insert(i, new TransformUpdate(tick, position));
                return;
            }
        }

        futureTransformUpdates.Add(new TransformUpdate(tick, position));
    }
}
