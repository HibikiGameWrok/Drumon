using System.Collections;
using UniRx;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [SerializeField]
    private IntReactiveProperty m_readyTimer = new IntReactiveProperty(3);
    [SerializeField]
    private IntReactiveProperty m_battleTimer = new IntReactiveProperty(10);
    [SerializeField]
    private BoolReactiveProperty m_isBattleFinish = new BoolReactiveProperty(false);
    [SerializeField]
    private UnityEngine.UI.Button m_button;

    [SerializeField]
    private ReactiveProperty<Player> m_player = new ReactiveProperty<Player>();

    [SerializeField]
    private ReactiveProperty<TestSingletonObject> m_test = new ReactiveProperty<TestSingletonObject>();

    public IReadOnlyReactiveProperty<int> ReadyTimer => m_readyTimer;
    public IReadOnlyReactiveProperty<int> BattleTimer => m_battleTimer;
    public IReadOnlyReactiveProperty<bool> IsBattleFinish => m_isBattleFinish;
    public UnityEngine.UI.Button Button => m_button;

    public IReadOnlyReactiveProperty<Player> Player => m_player;
    public IReadOnlyReactiveProperty<TestSingletonObject> Test => m_test;
    private void Start()
    {
        m_player.Value = GameObject.Find("CPlayer").GetComponent<Player>();
        m_test.Value = GameObject.Find("Stage").GetComponent<TestSingletonObject>();    
    }

    public void InitilaizeCountDownStart()
    {
        StartCoroutine(CountCoroutine());
    }

    public void BattleResultCountDonwStart()
    {
        StartCoroutine(CountDownBattleResult());
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

    private IEnumerator CountDownBattleResult()
    {
        while (m_battleTimer.Value > 0)
        {
            m_battleTimer.Value--;
            yield return new WaitForSecondsRealtime(1);
        }

        if(m_battleTimer.Value <= 0)
        {
            m_isBattleFinish.Value = true;
            yield return null;
        }
    }
}
