if not exist __APPǰ�� (
   svn co https://svn������IP/svn/��Ŀ��/ __��ҳǰ��\ --username svn�˺��� --password svn����
)
svn update ..\Code\WebClient\dist\
..\EntryBuilder DeleteContentExcept __��ҳǰ�� .svn
xcopy /D /Y /E ..\Code\WebClient\dist\*.* __��ҳǰ��

::svn add __��ҳǰ�� --auto-props --force
::svn commit __��ҳǰ�� -m "Publish __��ҳǰ�� Commit"
pause