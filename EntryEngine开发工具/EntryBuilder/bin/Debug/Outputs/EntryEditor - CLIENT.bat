copy ..\..\..\..\�༭��\EntryEditor\bin\Debug\EntryEditor.exe EntryEditor\
copy ..\..\..\..\�༭��\EntryEditor\bin\Debug\*.dll EntryEditor\

::copy ..\..\..\..\�༭��\EntryEditor\bin\Debug\*.pdb EntryEditor\
copy ..\..\..\..\..\EntryEngine����ʱ\Xna\bin\CLIENT\*.dll EntryEditor\

::copy ..\..\..\..\..\EntryEngine����ʱ\Xna\*.dll EntryEditor\

copy ..\..\..\..\..\EntryEngine\bin\Debug\*.dll EntryEditor\

del EntryEditor\Microsoft.Xna.Framework.dll
del EntryEditor\Microsoft.Xna.Framework.Game.dll

move EntryEditor\fmodex.dll fmodex.dll
..\EntryBuilder.exe BuildLinkShell EntryEditor\ 3.5 10 EntryEditor.exe "x86" "" true ""
del fmodex.dll