REM ������վ·����˿�
set path=WebSite
set port=8080

@echo off
echo ΢������ѧϰ����ϵͳ ������...
echo.

REM ������վ����·����Ĭ��Ϊ��ǰ��ǰ�ļ���
set BASE_DIR=%~dp0
set SITE_PATH=%BASE_DIR%%path%

::echo %BASE_DIR%
::echo %SITE_PATH%

REM ����web������
cd iis express
start /b iisexpress /path:"%SITE_PATH%" /port:%port% /clr:v4.0 

REM ���������������վ
start "" "http://localhost:"%port% 

pause