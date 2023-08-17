using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu<MainMenu>
{
    public void OnClickGame()
    {
        Debug.Log("Game Start");
    }

    public void OnClickSetting()
    {
        SettingMenu.Open();
    }

    public void OnClickHero()
    {
        Debug.Log("Hero ����");
    }

    public void OnClickQuest()
    {
        Debug.Log("Quest ����");
    }

}
