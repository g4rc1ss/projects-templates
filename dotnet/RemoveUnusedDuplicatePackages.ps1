Write-Output (Get-Item $PSCommandPath).FullName
Set-Location -Path (Get-Item $PSCommandPath).DirectoryName

$packages = Select-Xml -Path "Directory.Packages.props" -XPath "//PackageVersion"
$unusedPackages = @()

foreach ($pkgNode in $packages)
{
    $found = Get-ChildItem -Recurse -Include *.csproj, Directory.Build.props -File | Select-String -Pattern $pkgNode.Node.Include

    if (-not $found)
    {
        #        Write-Output "$pkg not used in any .csproj"
        $unusedPackages += $pkgNode.Node.Include
    }
}

if ($unusedPackages.Count -eq 0)
{
    Write-Output "No unused packages found."
    exit 0
}

[xml]$doc = Get-Content "Directory.Packages.props" -Raw

foreach ($unused in $unusedPackages)
{
    $node = $doc.SelectSingleNode("//PackageVersion[@Include='$unused']")
    if ($null -ne $node)
    {
        $null = $node.ParentNode.RemoveChild($node)
        Write-Host "Removed $unused"
    }
}

$doc.Save((Resolve-Path "Directory.Packages.props"))
Write-Output "Updated Directory.Packages.props with unused packages removed."
