using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class VFXCreater_Script : MonoBehaviour
{
    public static void CreateEffect(string name, Transform trans)
    {
        GameObject obj = Resources.Load("VFX/" + Regex.Replace(name, @"[^a-z,A-Z]", "")) as GameObject;
        if (!obj) return;

        obj = Instantiate(obj, trans.position, trans.rotation * obj.transform.rotation);

        AudioManager_Script.Get.PlaySE(name);

        Destroy(obj, 3.0f);
    }
}
