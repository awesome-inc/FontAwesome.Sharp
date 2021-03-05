function Update-Files {
    param(
        [Parameter(Mandatory = $True)] [string]$source,
        [Parameter(Mandatory = $True)] [string]$destination
    )
    # Work around npm `Back to the future`-issue, cf.:
    # - https://github.com/angular/components/issues/11009#issuecomment-384445226
    $lastWrite = Get-Date
    Get-ChildItem -Recurse -Path $source | ForEach-Object {
        $f = $_
        Write-Host $f
        $f.LastWriteTime = $lastWrite
        Copy-Item -Path $f -Destination $destination
    }
}

Write-Host Updating node packages...
npm i
ncu -u
npm i

Write-Host Updating web fonts...
#Get-ChildItem -Recurse -Path .\node_modules\@fortawesome\*.ttf | $.LastWriteTime = $lastWrite | Copy-Item -Destination .\FontAwesome.Sharp\fonts
#Get-ChildItem -Recurse -Path .\node_modules\@mdi\*.ttf | $.LastWriteTime = $lastWrite | Copy-Item -Destination .\TestWpf\fonts
Update-Files .\node_modules\@fortawesome\*.ttf .\FontAwesome.Sharp\fonts
Update-Files .\node_modules\@mdi\*.ttf .\FontAwesome.Sharp.Material\fonts

Write-Host Updating css...
Update-Files .\node_modules\@fortawesome\fontawesome-free\css\fontawesome.css  .\FontEnumGenerator\Content
Update-Files .\node_modules\@mdi\font\css\materialdesignicons.css .\FontEnumGenerator\Content

Write-Host Rebuild .\FontEnumGenerator
dotnet build .\FontEnumGenerator -c Release

Write-Host Generating IconEnum classes...
Push-Location .\FontEnumGenerator\bin\Release

.\FontEnumGenerator.exe --css .\Content\fontawesome.css --name IconChar
Copy-Item -Path .\IconChar.cs -Destination ..\..\..\FontAwesome.Sharp

.\FontEnumGenerator.exe --css .\Content\materialdesignicons.css --pattern "\.mdi-(.+):before" --name MaterialIcons
Copy-Item -Path .\MaterialIcons.cs -Destination ..\..\..\FontAwesome.Sharp.Material

Pop-Location
