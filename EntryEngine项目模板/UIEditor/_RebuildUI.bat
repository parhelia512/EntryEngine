:: ���������ļ�����Ŀ
call _CopyToCode.bat
:: ���±�����Ŀ
cd ..\
EntryBuilder.exe BuildDll Code\Client\Client\ Launch\Client\Client.dll 3.5 "" false ""
:: �������������ɵķ��������Ŀ¼
copy Design\LANGUAGE.csv Launch\Client\Content\Tables\LANGUAGE.csv
:: ������շ����
EntryBuilder.exe BuildOutputCSV Launch\Client\Content\Tables\LANGUAGE.csv ""
:: �л�������Ŀ¼����Ӧ�ÿͻ��˳���
cd Launch\Client
call PCRun.exe