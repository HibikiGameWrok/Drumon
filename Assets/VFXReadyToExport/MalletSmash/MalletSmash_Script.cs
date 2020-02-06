using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INSTRUCTIONS:
//Attach the provided object as a child of the Drumon using this move
//This script will disable itself after finishing
//Do not enable this script with other conflicting VFX
//Make sure the Garfish's rotation is correct

public class MalletSmash_Script : MonoBehaviour
{

    [SerializeField]
    private GameObject Streaks_Prefab = null;
    [SerializeField]
    private GameObject GroundImpact_Prefab = null;

    [SerializeField]
    private Transform ObjectPosition = null;

    private Vector3 Origin;
    private Vector3 Destination;
    private Quaternion OriginRotation;

    private float timer = 0.0f;
    public float timerMax = 0.5f;


    private float DestinationTracker = 0;
    private float RotationTracker = 0;

    public float DestinationDistance = 8;

    private Vector3 InitialForward = new Vector3();
    private Vector3 InitialUp = new Vector3();
    private Vector3 InitialRight = new Vector3();

    private Vector3 NearEndPos = new Vector3();
    private Vector3 NearEndRot = new Vector3();

    public bool DisableOnFinish = true;

    // Start is called before the first frame update
    void Start()
    {
        Origin = ObjectPosition.position;
        OriginRotation = ObjectPosition.rotation;

        Destination = Origin + transform.forward * DestinationDistance;

        InitialForward = transform.forward;
        InitialUp = transform.up;
        InitialRight = transform.right;
    }

    void OnEnable()
    {
        Origin = ObjectPosition.position;
        OriginRotation = ObjectPosition.rotation;

        Destination = Origin + transform.forward * DestinationDistance;

        InitialForward = transform.forward;
        InitialUp = transform.up;
        InitialRight = transform.right;
    }

    void IntervalCheck(float dt, float max)
    {
        if (timer - dt <= max && max <= timer)
        {
            RotationTracker = 0;
            DestinationTracker = 0;

            if (max > timerMax * 0.6f)
            {
                NearEndPos = transform.position;
                NearEndRot = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);

                GameObject groundimpact = Instantiate(GroundImpact_Prefab);
                groundimpact.transform.position = transform.position + InitialForward * 3 - InitialUp;
            }
            else if (max > timerMax * 0.4f)
            {
                GameObject groundstreaks = Instantiate(Streaks_Prefab);
                groundstreaks.transform.position = transform.position + InitialForward * 4 + InitialUp * 2;
            }

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= timerMax) //End
        {
            ObjectPosition.position = Origin;
            ObjectPosition.rotation = OriginRotation;
            timer = 0;
            timer += Time.deltaTime;

            RotationTracker = 0;
            DestinationTracker = 0;

            if (DisableOnFinish)
            {
                this.enabled = false;
                return;
            }
        }
        else
        {
            IntervalCheck(Time.deltaTime, timerMax * 0.25f);
            IntervalCheck(Time.deltaTime, timerMax * 0.50f);
            IntervalCheck(Time.deltaTime, timerMax * 0.65f);
        }

        if (timer >= timerMax * 0.65) //Phase 4 - Return
        {
            float magnitude = (NearEndPos - Origin).magnitude;

            float DistanceDelta = magnitude * Time.deltaTime / (timerMax * 0.35f);
            if (DestinationTracker + DistanceDelta >= magnitude)
            {
                DistanceDelta = magnitude - DestinationTracker;
                DestinationTracker = 0;
            }
            transform.position = Vector3.MoveTowards(transform.position, Origin, magnitude * Time.deltaTime / (timerMax * 0.35f));

            transform.rotation = Quaternion.RotateTowards(transform.rotation, OriginRotation, 100 * Time.deltaTime / (timerMax * 0.35f));

        }
        else if (timer >= timerMax * 0.50) //Phase 3 - Going Down
        {
            float DistanceDelta = -DestinationDistance * Time.deltaTime / (timerMax * 0.15f);
            if (DestinationTracker + DistanceDelta <= -DestinationDistance)
            {
                DistanceDelta = DestinationDistance - DestinationTracker;
                DestinationTracker = 0;
            }
            transform.position += DistanceDelta * InitialUp;

            float RotationDelta = 45 * Time.deltaTime / (timerMax * 0.15f);
            if (RotationTracker + RotationDelta >= 45)
            {
                RotationDelta = 45 - RotationTracker;
                RotationTracker = 0;
            }
            transform.Rotate(0, -RotationDelta, 0);
        }
        else if (timer >= timerMax * 0.25) //Phase 2 - Going Up
        {
            float DistanceDelta = DestinationDistance * Time.deltaTime / (timerMax * 0.25f);
            if (DestinationTracker + DistanceDelta >= DestinationDistance)
            {
                DistanceDelta = DestinationDistance - DestinationTracker;
                DestinationTracker = 0;
            }
            transform.position += DistanceDelta * InitialForward;
            transform.position += DistanceDelta * InitialUp;

            float RotationDelta = 45 * Time.deltaTime / (timerMax * 0.25f);
            if (RotationTracker + RotationDelta >= 90)
            {
                RotationDelta = 90 - RotationTracker;
                RotationTracker = 0;
            }
            transform.Rotate(0, 0, RotationDelta);

        }
        else if (timer >= timerMax * 0.00) //Phase 1 - Facing Up
        {
            {
                float RotationDelta = -45 * Time.deltaTime / (timerMax * 0.25f);

                if (RotationTracker + RotationDelta <= -45)
                {
                    RotationDelta = -45 - RotationTracker;
                    RotationTracker = 0;
                }
                else
                {
                    RotationTracker += RotationDelta;
                }
                transform.Rotate(RotationDelta, 0, 0);
            }
            {
                float RotationDelta = 45 * Time.deltaTime / (timerMax * 0.25f);
                if (RotationTracker + RotationDelta >= 45)
                {
                    RotationDelta = 45 - RotationTracker;
                    RotationTracker = 0;
                }
                transform.Rotate(0, 0, RotationDelta);
            }
        }

    }
}
