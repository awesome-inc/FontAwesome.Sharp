Write-Host Updating node packages...
ncu -u

Write-Host Updating web fonts...
Get-ChildItem -Recurse -Path .\node_modules\@fortawesome\*.ttf | Copy-Item -Destination .\FontAwesome.Sharp\fonts
Get-ChildItem -Recurse -Path .\node_modules\@mdi\*.ttf | Copy-Item -Destination .\TestWpf\fonts

Write-Host Updating css...
Copy-Item -Path .\node_modules\@fortawesome\fontawesome-free\css\fontawesome.css -Destination .\FontEnumGenerator\Content
Copy-Item -Path .\node_modules\@mdi\font\css\materialdesignicons.css -Destination .\FontEnumGenerator\Content

Write-Host Generating IconEnum classes...
.\build.bat /v:m .\FontEnumGenerator\FontEnumGenerator.csproj
Push-Location .\FontEnumGenerator\bin\Release

.\FontEnumGenerator.exe --css .\Content\fontawesome.css --name IconChar
Copy-Item -Path .\IconChar.cs -Destination ..\..\..\FontAwesome.Sharp

.\FontEnumGenerator.exe --css .\Content\materialdesignicons.css --prefix .mdi- --name MaterialIcons
Copy-Item -Path .\MaterialIcons.cs -Destination ..\..\..\TestWpf\MaterialDesign

Pop-Location
