using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct WaterRipple
{
    public float x;
    public float y;
    public float time;
    public float InitialTime;
}

struct ObjectInWater
{
    public GameObject gameObject;
    public Vector3 PrevRipplePos;

    public ObjectInWater(GameObject _gameObject, Vector3 _PrevRipplePos)
    {
        gameObject = _gameObject;
        PrevRipplePos = _PrevRipplePos;
    }

}

public class RippleManager_Script : MonoBehaviour {

    [SerializeField]
    private Renderer WaterTopMaterial_Ref;

    private Dictionary<GameObject, ObjectInWater> ObjInWaterList = new Dictionary<GameObject, ObjectInWater>();

    private WaterRipple[] ripple;
    public int rippleAmount = 0;

    [Range(0, 40)]
    public int MaxRippleAmount = 16;

    public float NextRippleDist = 1; //This system is not good at the moment, swap it out at some point

    // Use this for initialization
    void Start () {
        ripple = new WaterRipple[MaxRippleAmount];
        for (int i = 0; i < MaxRippleAmount; ++i)
        {
            ripple[i] = new WaterRipple();
        }

        WaterTopMaterial_Ref = GetComponent<MeshRenderer>();
        //WaterTopMaterial_Ref.material.SetTexture("_RenderTexture", texture);
    }
	
	// Update is called once per frame
	void Update () {

        Vector4[] thelist = new Vector4[MaxRippleAmount];
        int counter = 0;

        for (int i = 0; i < MaxRippleAmount; ++i)
        {
            if (ripple[i].time > 0)
            {
                ripple[i].time -= Time.deltaTime;
                if (ripple[i].time <= 0)
                {
                    rippleAmount--;
                    WaterTopMaterial_Ref.material.SetInt("RipplesAmount", rippleAmount);
                }
                else
                {
                    thelist[counter] = new Vector4(ripple[i].x, ripple[i].y, ripple[i].time, ripple[i].InitialTime);
                    counter++;
                }
            }
        }

        WaterTopMaterial_Ref.material.SetVectorArray("Ripples", thelist);

        List<GameObject> ReAdd = new List<GameObject>();
        foreach (ObjectInWater obj in ObjInWaterList.Values)
        {
            Vector3 PosToCompare = new Vector3(obj.gameObject.transform.position.x, obj.PrevRipplePos.y, obj.gameObject.transform.position.z);
            if ((PosToCompare - obj.PrevRipplePos).magnitude > NextRippleDist)
            {
                CreateRipple(PosToCompare);
                ReAdd.Add(obj.gameObject);
            }
        }
        for (int i = 0; i < ReAdd.Count; ++i)
        {
            ObjInWaterList[ReAdd[i]] = new ObjectInWater(ReAdd[i], ReAdd[i].transform.position);
        }

    }

    void OnTriggerExit(Collider col)
    {
        ObjInWaterList.Remove(col.gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        ObjInWaterList.Add(col.gameObject, new ObjectInWater(col.gameObject, col.gameObject.transform.position));
        
        CreateRipple(col.gameObject.transform.position);

    }

    void CreateRipple(Vector3 location)
    {
        if (rippleAmount < MaxRippleAmount)
        {
            for (int i = 0; i < MaxRippleAmount; ++i)
            {
                if (ripple[i].time <= 0)
                {
                    //ripple[i].x = (col.transform.position.x - GetComponent<MeshCollider>().bounds.min.x) / GetComponent<MeshCollider>().bounds.size.x;
                    //ripple[i].x = Mathf.Clamp(ripple[i].x, 0, 1);
                    //ripple[i].y = (col.transform.position.z - GetComponent<MeshCollider>().bounds.min.z) / GetComponent<MeshCollider>().bounds.size.z;
                    //ripple[i].y = Mathf.Clamp(ripple[i].y, 0, 1);
                    ripple[i].x = (location.x);
                    ripple[i].y = (location.z);
                    //ripple[i].time = 1.6f + 0.35f * (float)Random.Range(2, 3 + 1);
                    //ripple[i].InitialTime = ripple[i].time;

                    //Trying to fix a bug
                    ripple[i].time = 1.3f + 0.25f * (float)Random.Range(2, 3 + 1) + 0.25f;
                    ripple[i].InitialTime = ripple[i].time;
                    ripple[i].time -= 0.25f;

                    break;
                }
            }

            rippleAmount++;
            WaterTopMaterial_Ref.material.SetInt("RipplesAmount", rippleAmount);
        }
    }

}
