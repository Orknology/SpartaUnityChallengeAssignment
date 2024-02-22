using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditionalSceneManager
{
    public BaseScene CurrentScene => UnityEngine.Object.FindObjectOfType<BaseScene>();    

    public void LoadScene(Define.Scenes scene)
    {
        Managers.Clear();

        SceneManager.LoadScene(GetSceneName(scene));
    }

    private string GetSceneName(Define.Scenes scene)
    {
        string sceneName = System.Enum.GetName(typeof(Define.Scenes), scene);
        return sceneName;
    }    

    public void Clear()
    {
        CurrentScene.Clear();        
    }
}
