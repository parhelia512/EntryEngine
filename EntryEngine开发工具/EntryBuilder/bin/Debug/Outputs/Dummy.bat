del fmodex.dll
..\EntryBuilder.exe BuildDll ..\..\..\..\..\EntryEngine\ Dummy\EntryEngine.dll 3.5 "" true CLIENT;SERVER;DEBUG;

copy ..\..\..\..\..\EntryEngine����ʱ\Xna\bin\Debug\*.dll Xna\
..\EntryBuilder.exe BuildDll ..\..\..\..\..\EntryEngine����ʱ\Xna\ Dummy\Xna.dll 3.5 "System.Drawing.dll|System.Windows.Forms.dll|Xna\EntryEngine.dll|..\..\..\..\..\EntryEngine����ʱ\Xna\Microsoft.Xna.Framework.dll|..\..\..\..\..\EntryEngine����ʱ\Xna\Microsoft.Xna.Framework.Game.dll" true ""

..\EntryBuilder.exe BuildDll ..\..\..\..\..\EntryEngine����ʱ\Cmdline\ Dummy\Cmdline.dll 3.5 "..\EntryEngine.dll" true ""

..\EntryBuilder.exe BuildDll ..\..\..\..\..\EntryEngine����ʱ\Unity\ Dummy\Unity.dll 3.5 "..\EntryEngine.dll|..\..\..\..\..\EntryEngine����ʱ\Unity\bin\Debug\UnityEngine.dll" true ""