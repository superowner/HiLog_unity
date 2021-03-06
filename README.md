# HiDebug_unity
----------------------
[中文说明](https://github.com/hiramtan/HiDebug_unity/blob/master/README_zh.md)


### How to use
 You can download unity package from here: [![Github Releases](https://img.shields.io/github/downloads/atom/atom/total.svg)](https://github.com/hiramtan/HiDebug_unity/releases)

 or you can download from unity asset store: [https://www.assetstore.unity3d.com/en/#!/content/104658](https://www.assetstore.unity3d.com/en/#!/content/104658)

---------

### Features

>- Support multiple platform(unity editor, exe, Android, iOS, WP...).
>- Enable or disable all logs in one switch(debug mode set true let logs on, release mode set false disable all logs out put).
>- Whether enable logs on screen(so that you can still check logs if you don't want to connect Android Studio or xcode)
>- Whether enable write logs into a text(default path is persistent folder, when crash can check logs in text)
>- Adding data and time append to you logs.
>- Display stack on screen or record stack in text.
>- There is only a DLL, you can copy this to your project to use whole functionality


### Details

1. Hidebug's Logs On Console:

``` csharp
Hidebug.EnableDebuger(true);
```

If you use Debuger.Log or Debuger.LogWarnning or Debuger.LogError print logs, you can disable all of them just set Hidebug.EnableDebuger(false).

Also, it will automatically add data and time to your logs.

[![](https://i.imgur.com/9qjXKea.png)](https://i.imgur.com/9qjXKea.png)

Of course, you can select don't use this function, and prefer to use unityengine's Debug.Log, those logs can also display on screen or write into text.

2. Logs in text:

``` csharp
Hidebug.EnableOnText(true);
```

Will record logs and stacks into a text, the default path is Application.persistentDataPath.

[![](https://imgur.com/AaGtUT4)](https://imgur.com/AaGtUT4)

3. Logs on screen:

``` csharp
Hidebug.EnableOnScreen(true);
```
Will display a button on your screen,you can drag this button to anywhere you want(don't cover your game's button)

When you click this button, will open a panel to display logs and stacks.

>- Click per out put log to select to show its stack.
>- Toggle log or warnning or error to select only display this kind of logs.
>- Clear all logs on screen.
>- Close this panel back to your game display.
>- Set font size display on screen.

[![](https://i.imgur.com/AdoD6UA.gif)](https://i.imgur.com/AdoD6UA.gif)

----------
#### Example1
```csharp
void Start()
    {
        HiDebug.EnableOnText(true);
        HiDebug.EnableOnScreen(true);

        Use_Debuger();
        Use_Debug();
    }

    /// <summary>
    /// use debuger, you can enable or disable logs just one switch
    /// and also it automatically add time to your logs 
    /// </summary>
    void Use_Debuger()
    {
        //you can set all debuger's out put logs disable just set this value false(pc,android,ios...etc)
        //it's convenient in release mode, just set this false, and in debug mode set this true.
        HiDebug.EnableDebuger(true);

        for (int i = 0; i < 100; i++)
        {
            Debuger.Log(i);
            Debuger.LogWarning(i);
            Debuger.LogError(i);
        }
    }

    /// <summary>
    /// if you donnt want use Debuger.Log()/Debuger.LogWarnning()/Debuger.LogError()
    /// you can still let UnityEngine's Debug on your screen or write them into text
    /// </summary>
    void Use_Debug()
    {
        for (int i = 0; i < 100; i++)
        {
            Debug.Log(i);
            Debug.LogWarning(i);
            Debug.LogError(i);
        }
    }
```
[![](https://i.imgur.com/8TPMvcW.png)](https://i.imgur.com/8TPMvcW.png)
#### Example2
``` csharp
    [SerializeField]
    private bool _isLogOn;//set this value from inspector
    [SerializeField]
    private bool _isLogOnText;
    [SerializeField]
    private bool _isLogOnScreen;
    // Use this for initialization
    void Start()
    {
        HiDebug.EnableDebuger(_isLogOn);
        HiDebug.EnableOnText(_isLogOnText);
        HiDebug.EnableOnScreen(_isLogOnScreen);
        for (int i = 0; i < 100; i++)
        {
            Debuger.Log(i);
            Debuger.LogWarning(i);
            Debuger.LogError(i);
        }
        HiDebug.FontSize = 20;//set size of font
    }
```


[![](https://i.imgur.com/EgvKDUn.png)](https://i.imgur.com/EgvKDUn.png)

#### Example3

Use unityengine's Debug.Log, still can record on screen or write into text.

``` csharp
    [SerializeField]
    private bool _isLogOnText;
    [SerializeField]
    private bool _isLogOnScreen;
    // Use this for initialization
    void Start()
    {
        HiDebug.EnableOnText(_isLogOnText);
        HiDebug.EnableOnScreen(_isLogOnScreen);


        //unity engine's debug.log
        for (int i = 0; i < 100; i++)
        {
            Debug.Log(i);
            Debug.LogWarning(i);
            Debug.LogError(i);
        }
    }
```
[![](https://i.imgur.com/95Wcmqx.png)](https://i.imgur.com/95Wcmqx.png)


support: hiramtan@live.com


***********

MIT License

Copyright (c) [2017] [Hiram]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
