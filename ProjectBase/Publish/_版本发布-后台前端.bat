if not exist __��̨ǰ�� (
   svn co https://svn������IP/svn/��Ŀ��/ __��̨ǰ��\ --username svn�˺��� --password svn����
)
svn update ..\Code\WebCenter\dist\
..\EntryBuilder DeleteContentExcept __��̨ǰ�� .svn
xcopy /D /Y /E ..\Code\WebCenter\dist\*.* __��̨ǰ��

::svn add __��̨ǰ�� --auto-props --force
::svn commit __��̨ǰ�� -m "Publish __��̨ǰ�� Commit"
pause