param (
    [string]$SolutionPath = $null
)

function Add-Comments {
    $yearFolders = Get-ChildItem -Directory -Filter "Year*"
    $lineEnding = "`r`n"

    foreach ($folder in $yearFolders) {
        $year = $folder.Name -replace 'Year ', ''

        $codeFiles = Get-ChildItem -Path "$($folder.FullName)\*" -Recurse -File |
            Where-Object { $_.Extension -in '.cs', '.fs' }

        foreach ($file in $codeFiles) {
            $day = ($file.BaseName -replace '[^0-9]', '').TrimStart('0')

            $comment = "// Task: https://adventofcode.com/$year/day/$day"

            $lines = Get-Content $file.FullName -Raw
            $lineArray = $lines -split $lineEnding

            if (-not ($lineArray[0] -match '^//')) {
                $lineArray = @($comment, "") + $lineArray
            }

            $updatedContent = ($lineArray -join $lineEnding)

            [System.IO.File]::WriteAllText($file.FullName, $updatedContent, [System.Text.Encoding]::UTF8)
        }
    }

    Write-Host "All Challenge Solution files have been updated."
}

try
{
    $originalLocation = Get-Location
    $baseFolderPath = if (-not $SolutionPath) { "$originalLocation\Advent of Code\Challenge Solutions"} else {$SolutionPath}

    Set-Location $baseFolderPath
    Add-Comments
}
catch
{
    Write-Error "An error occurred: $_"
}
finally
{
    Set-Location $originalLocation
}
