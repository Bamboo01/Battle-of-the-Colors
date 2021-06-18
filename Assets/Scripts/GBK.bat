@echo off
 powershell.exe -command "dir * .cs | foreach-object {(Get-Content $ _ FullName -Encoding Default.) | Set-Content $ _ FullName -Encoding UTF8.}; Write-Host 'conversion is complete .. . ' "
 powershell.exe -command "dir * .cs | foreach-object {[void] [System.IO.File] :: WriteAllBytes ($ _ FullName, [System.Text.Encoding] :: Convert ([System. Text.Encoding] :: GetEncoding ( 'GBK'), [System.Text.Encoding] :: UTF8, [System.IO.File] :: ReadAllBytes ($ _ FullName.)))}; Write-Host 'complete conversion ... ' "
pause