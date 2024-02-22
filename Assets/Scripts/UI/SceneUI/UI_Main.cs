using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main : UI_Scene
{
    private Stack<Image> _hpImages = new Stack<Image>();

    enum HUD_Panel
    {
        HPImages_Panel,
        Controll_Panel,
    }
    enum Texts
    {
        CharacterName_Text,
    }

    protected override void Init()
    {
        base.Init();
        Bind<Transform>(typeof(HUD_Panel));        
        BindText(typeof(Texts));
        
    }

}
