using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChildActiveObject_Script : MonoBehaviour
{
    // UIの表示
    public void OpenUI()
    {
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.activeSelf == false)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
    // UIの非表示
    public void CloseUI()
    {
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.activeSelf == true)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
