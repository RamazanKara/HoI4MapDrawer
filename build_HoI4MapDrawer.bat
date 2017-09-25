set /p version=Version Number?:

call "%VS140COMNTOOLS%\vsvars32.bat"
msbuild.exe "HoI4MapDrawer.sln" /p:Configuration=Release /m
mkdir "HoI4MapDrawer-%version%\"
copy "bin\Release\ProvinceMapper.exe" "HoI4MapDrawer-%version%\HoI4MapDrawer.exe"
copy "FAQ.txt" "HoI4MapDrawer-%version%\FAQ.txt"
copy "readme.txt" "HoI4MapDrawer-%version%\readme.txt"

cd "HoI4MapDrawer-%version%"
call "%SEVENZIP_LOC%\7z.exe" a -tzip -r "..\HoI4MapDrawer-%version%.zip" "*" -mx5
cd ..
