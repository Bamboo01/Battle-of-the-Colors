using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//消息
  public class Message
    {
    //类型
    public byte Type;
    //命令
    public int Comannd;
    //参数
    public object Content;
    public Message(){}
    public Message(byte type,int comannd,object content)
    {
        Type = type;
        Comannd = comannd;
        Content = content;
    }

 }

//消息类型
public class MessageType
{
    //audio=声音 UI=UI Player=Player和机甲 Skill=技能 Paint=涂染
    public static byte Type_Audio = 1;
    public static byte Type_UI= 2;
    public static byte Type_Player = 3;
    public static byte Type_Skill = 4;
    public static byte Type_paint = 5;

    public static int Audio_playSound = 100;
    public static int Audio_playmusic = 101;


    //打开面板
    public static int UI_ShowPanel = 200;
    //加分
    public static int UI_AddScore = 201;




}

