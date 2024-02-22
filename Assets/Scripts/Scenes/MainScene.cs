using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scenes.Main;
        SoundManager.Instance.ChangeBackGroundMusic(Resources.Load<AudioClip>("Sounds/Main_BGM"), 0.1f);
        Managers.UI.ShowSceneUI<UI_Main>();
        Managers.RM.Instantiate("Player/Player");
        Managers.RM.Instantiate("Map/TempMap");
    }
}
