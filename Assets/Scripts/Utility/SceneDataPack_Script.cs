/*----------------------------------------------------------*/
//  file:      SceneDataPack_Scripts.cs				            |
//				 											                    |
//  brief:    シーンをまたいでデータを受け渡す		            | 
//              			                                                    |
//															                    |
//  date:	2019.11.1										            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/


using System;

public abstract class SceneDataPack
{
    /// <summary>
    /// 前のシーン
    /// </summary>
    public abstract GameScenes PreviousGameScene { get; }


    /// <summary>
    /// 前シーンで追加ロードしていたシーン一覧
    /// </summary>
    public abstract GameScenes[] PreviousAdditiveScene { get; }
}


/// <summary>
/// デフォルト実装
/// </summary>
public class DefalutSceneDataPack : SceneDataPack
{
    private readonly GameScenes m_prevGameScenes;
    private readonly GameScenes[] m_additiveScenes;

    public GameScenes[] AdditiveScenes
    {
        get { return m_additiveScenes; }
    }

    public override GameScenes PreviousGameScene
    {
        get { return m_prevGameScenes; }
    }

    public override GameScenes[] PreviousAdditiveScene
    {
        get { return null; }
    }

    public DefalutSceneDataPack(GameScenes prev, GameScenes[] additive)
    {
        m_prevGameScenes = prev;
        m_additiveScenes = additive;
    }
}