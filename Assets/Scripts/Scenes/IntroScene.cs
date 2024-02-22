using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scenes.Intro;
        
        Managers.Pool.Init();
        Managers.UI.ShowSceneUI<UI_Intro>();
        Managers.RM.Instantiate($"SoundManager/SoundManager");
        SoundManager.Instance.ChangeBackGroundMusic(Resources.Load<AudioClip>("Sounds/Intro_BGM"), 0.2f);
    }
    
}
