if not exist __����� (
   svn co https://svn������IP/svn/��Ŀ��/ __�����\ --username svn�˺��� --password svn����
)
svn update ..\Launch\Server
xcopy /D /Y /E ..\Launch\Server\*.dll __�����
xcopy /D /Y /E ..\Launch\Server\*.pdb __�����
xcopy /D /Y /E ..\Launch\Server\Server.exe __�����
xcopy /D /Y /E ..\Launch\Server\*.csv __�����

::svn add __����� --auto-props --force
::svn commit __����� -m "Publish __����� Commit"
pause