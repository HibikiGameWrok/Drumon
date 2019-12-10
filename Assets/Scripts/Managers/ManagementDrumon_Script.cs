using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ManagementDrumon_Script : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_list;

    // Start is called before the first frame update
    void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                
            });
    }
}
