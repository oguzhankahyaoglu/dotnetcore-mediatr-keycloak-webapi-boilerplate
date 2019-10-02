$path = "C:\_rename-test"
$find = "NotificationAPI"
$replaceWith = "SampleAPI"


foreach ($f in gci -r -include "*.cs", "*.csproj", "*.sln", "*.config", "*.json", "*.pubxml", "*.gitignore" -path $path) 
    { (gc $f.fullname) |
       foreach { $_ -replace $find,$replaceWith }  |
       sc $f.fullname 
    }

Get-ChildItem $path -Recurse | 
    Where {$_.Name -Match $find} | 
    Rename-Item -NewName {$_.Name -replace $find,$replaceWith } 

