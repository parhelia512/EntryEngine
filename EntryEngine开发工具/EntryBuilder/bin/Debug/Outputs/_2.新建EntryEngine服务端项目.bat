cd ..\..\..\..\..\
set dir="EntryEngine��Ŀ\ͨ�ú�̨������ҳ"
xcopy /D /Y /E EntryEngine��Ŀģ�� %dir%
:: ɾ������˲���Ҫ������
cd %dir%
rmdir /Q /S Code\Client
del /Q /S Code\Project.sln
rmdir /Q /S Design
rmdir /Q /S Graphics
rmdir /Q /S Launch\Client
rmdir /Q /S Publish\IOS����
rmdir /Q /S Publish\Project
rmdir /Q /S Publish\WebGL
rmdir /Q /S Publish\����������
del /Q /S Publish\_�汾����-Unity.bat
rmdir /Q /S UIEditor
del /Q /S ˵��.txt