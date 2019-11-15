using System.Collections;
using UniRx;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [SerializeField]
    private IntReactiveProperty m_readyTimer = new IntReactiveProperty(3);


    public IReadOnlyReactiveProperty<int> ReadyTimer => m_readyTimer;


    public void CountDownStart()
    {
        StartCoroutine(CountCoroutine());
    }

    private IEnumerator CountCoroutine()
    {
        m_readyTimer.SetValueAndForceNotify(m_readyTimer.Value);
        yield return new WaitForSecondsRealtime(1);

        while(m_readyTimer.Value >= 0)
        {
            m_readyTimer.SetValueAndForceNotify(m_readyTimer.Value - 1);
            yield return new WaitForSecondsRealtime(1);
        }
    }
}
