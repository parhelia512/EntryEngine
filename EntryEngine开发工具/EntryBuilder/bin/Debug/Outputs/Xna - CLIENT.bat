copy ..\..\..\..\..\EntryEngine����ʱ\Xna\bin\Debug\*.dll Xna\
::copy ..\..\..\..\..\EntryEngine����ʱ\Xna\bin\Debug\*.pdb Xna\
move Xna\fmodex.dll fmodex.dll

copy ..\..\..\..\..\EntryEngine\bin\CLIENT\*.dll Xna\
::copy ..\..\..\..\..\EntryEngine\bin\CLIENT\*.pdb Xna\

..\EntryBuilder.exe BuildLinkShell Xna\ 3.5 10 Xna.exe "" "" true ""
del fmodex.dll