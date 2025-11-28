@echo off

setlocal enabledelayedexpansion

:: 设置 JSON 文件路径
set JSON_FILE=settings.json

:: 读取 JSON 文件内容并处理
for /F "tokens=1,2 delims=:,{}" %%A in ('type "%JSON_FILE%" ^| findstr /v /c:"{" /c:"}" /c:"[" /c:"]" /c:"}"') do (
    set "key=%%A"
    set "value=%%B"
    
    :: 去除引号和多余的空格
    set "key=!key:"=!"
    set "value=!value:"=!"
    
    set "key=!key: =!"
    set "value=!value: =!"

    :: 设置环境变量
    set "!key!=!value!"
)

:: 输出所有读取的值进行验证
echo build_date=%build_date%
echo build_date_year=%build_date_year%
echo data_ver=%data_ver%
echo build_R01_Only=%build_R01_Only%
echo build_R01_SEA=%build_R01_SEA%
echo build_R12=%build_R12%
echo build_R21=%build_R21%
echo build_R22=%build_R22%
echo build_R23=%build_R23%
echo build_R24=%build_R24%
echo build_R41=%build_R41%
echo build_R99=%build_R99%


REM call each converter 
echo TWOnly
echo r01_388
echo excel in output folders\R01\20250331_v92TWOnly\388
call "bin\388\10Mio Converter.exe"
echo OUT\%data_ver%\Release\R01\%build_date_year%_v%build_R01_Only%TWOnly\388\
call copy /Y bin\388\*.raw OUT\%data_ver%\Release\R01\%build_date_year%_v%build_R01_Only%TWOnly\388\
call del bin\388\*.raw

echo r01_LW_ABBY_AMY
echo excel in output folders\R01\20250331_v92TWOnly\LW_ABBY_AMY-V92.0331.01.01-TWN
call bin\538_568_618_698\speedcam_add_platform_region_string_180123\speedcamtool.exe /u
call copy /Y bin\538_568_618_698\speedcam_add_platform_region_string_180123\*.bin OUT\%data_ver%\Release\R01\%build_date_year%_v%build_R01_Only%TWOnly\LW_ABBY_AMY-V%build_R01_Only%.%build_date%.01.01-TWN\*.bin
call del bin\538_568_618_698\speedcam_add_platform_region_string_180123\*.bin

echo r01_R28
echo excel in output folders\R01\20250331_v92TWOnly\R28
call "bin\R28P\GPs_r52V12_twn.exe"
call copy /Y bin\R28P\*.bin OUT\%data_ver%\Release\R01\%build_date_year%_v%build_R01_Only%TWOnly\R28\*.bin

echo r01_R52
echo excel in output folders\R01\20250331_v92TWOnly\R52
call "bin\R28P\GPs_r52V12_twn.exe"
call copy /Y bin\R28P\*.bin OUT\%data_ver%\Release\R01\%build_date_year%_v%build_R01_Only%TWOnly\R52\*.bin

echo r01_R60R62
echo excel in output folders\R01\20250331_v92TWOnly\R60R62
call "bin\R28P\GPs_r52V12_twn.exe"
call copy /Y bin\R28P\*.bin OUT\%data_ver%\Release\R01\%build_date_year%_v%build_R01_Only%TWOnly\R60R62\*.bin

echo r01_R58
echo excel in output folders\R01\20250331_v92TWOnly\R58
call "bin\R58\GPs_r52V12_twn.exe"
call copy /Y bin\R58\*.bin OUT\%data_ver%\Release\R01\%build_date_year%_v%build_R01_Only%TWOnly\R58\*.bin

echo TWSEAnoTT
echo r01_SEA_LW_ABBY_AMY
echo excel in output folders\R01\20250331_v95TWSEAnoTT\LW_ABBY_AMY-V95.0331.01.01-ROW
call "bin\538_568_618_698\speedcam_add_platform_region_string_180123\speedcamtool.exe" /u
call copy /Y bin\538_568_618_698\speedcam_add_platform_region_string_180123\*.bin OUT\%data_ver%\Release\R01\%build_date_year%_v%build_R01_SEA%TWSEAnoTT\LW_ABBY_AMY-V%build_R01_SEA%.%build_date%.01.01-ROW\*.bin
call del bin\538_568_618_698\speedcam_add_platform_region_string_180123\*.bin

echo R12
echo r12_LW_ABBY_AMY
echo excel in output folders\R12\LW_ABBY_AMY-V77.0331.01.12-ROW
call "bin\538_568_618_698\speedcam_add_platform_region_string_180123\speedcamtool.exe" /u
call copy /Y bin\538_568_618_698\speedcam_add_platform_region_string_180123\*.bin OUT\%data_ver%\Release\R12\LW_ABBY_AMY-V%build_R12%.%build_date%.01.12-ROW\*.bin
call del bin\538_568_618_698\speedcam_add_platform_region_string_180123\*.bin

echo R21
echo r21_Hector
echo excel in output folders\R21\Hector_v45.0331.01.21
call "bin\538_568_618_698_AddState_newtypeandformat_2302_AUSsize_2310_excel\speedcamtool.exe" /u
call copy /Y bin\538_568_618_698_AddState_newtypeandformat_2302_AUSsize_2310_excel\*.bin OUT\%data_ver%\Release\R21\Hector_v%build_R21%.%build_date%.01.21\*.bin
call del bin\538_568_618_698_AddState_newtypeandformat_2302_AUSsize_2310_excel\*.bin

echo R22
echo r22_Hector
echo excel in output folders\R22\Hector_v49.0331.01.22
call "bin\538_568_618_698_AddState_newtypeandformat_2302_AUSsize_2310_excel\speedcamtool.exe" /u
call copy /Y bin\538_568_618_698_AddState_newtypeandformat_2302_AUSsize_2310_excel\*.bin OUT\%data_ver%\Release\R22\Hector_v%build_R22%.%build_date%.01.22\*.bin
call del bin\538_568_618_698_AddState_newtypeandformat_2302_AUSsize_2310_excel\*.bin

echo R23
echo r23_6in1
echo excel in output folders\R23\V43.0331.01.23-ROW
call "bin\538_568_618_698_AddState_newtypeandformat_2302_AUSsize_2310_excel\speedcamtool.exe" /u
call copy /Y bin\538_568_618_698_AddState_newtypeandformat_2302_AUSsize_2310_excel\*.bin OUT\%data_ver%\Release\R23\V%build_R23%.%build_date%.01.23-ROW\*.bin
call del bin\538_568_618_698_AddState_newtypeandformat_2302_AUSsize_2310_excel\*.bin

echo R24
echo r24_9in1
echo excel in output folders\R24\V12.0331.01.24-ROW
call "bin\538_568_618_698-AddState-newtypeandformat-2302_AUSsize_TW_9in1\speedcamtool.exe" /u
call copy /Y bin\538_568_618_698-AddState-newtypeandformat-2302_AUSsize_TW_9in1\*.bin OUT\%data_ver%\Release\R24\V%build_R24%.%build_date%.01.24-ROW\*.bin
call del bin\538_568_618_698-AddState-newtypeandformat-2302_AUSsize_TW_9in1\*.bin

echo R41
echo r41_York
echo excel in output folders\R41\York-V38_v20250331-ROW
call "bin\York\edog\speedCam\speedcamtool.exe" /u
call copy /Y bin\York\edog\speedCam\*.bin OUT\%data_ver%\Release\R41\York-V%build_R41%_v%build_date_year%-ROW\*.bin
call del bin\York\edog\speedCam\*.bin

echo R99
echo r99_9in1
echo excel in output folders\R99\V2.0331.01.99-ROW
call "bin\speedcamtool_V99\speedcamtool.exe" /u
call copy /Y bin\speedcamtool_V99\*.bin OUT\%data_ver%\Release\R99\V%build_R99%.%build_date%.01.99-ROW\*.bin
call del bin\speedcamtool_V99\*.bin


endlocal
pause
