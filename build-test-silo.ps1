Write-Host "Starting build of Docker container"
Write-Host "Creating staging folder for DemoCluster.Silo"

$stagingDirectory = ".\SiloStaging"

if (Test-Path $stagingDirectory) {
    Write-Host $stagingDirectory " was found....removing...."
    Remove-Item $stagingDirectory -Recurse
}

New-Item -Name $stagingDirectory -ItemType Directory | Out-Null

Copy-Item -Path .\src\DemoCluster -Destination $stagingDirectory -Recurse -Exclude "bin/", "obj/"
Copy-Item -Path .\src\DemoCluster.DAL -Destination $stagingDirectory -Recurse -Exclude "bin/", "obj/"
Copy-Item -Path .\src\DemoCluster.GrainImplementations -Destination $stagingDirectory -Recurse -Exclude "bin/", "obj/"
Copy-Item -Path .\src\DemoCluster.GrainInterfaces -Destination $stagingDirectory -Recurse -Exclude "bin/*", "obj/"
Copy-Item -Path .\src\DemoCluster.Hosting -Destination $stagingDirectory -Recurse -Exclude "bin/", "obj/"
Copy-Item -Path .\src\Orleans.Storage.Redis -Destination $stagingDirectory -Recurse -Exclude "bin/", "obj/"
Copy-Item -Path .\src\DemoCluster.Silo -Destination $stagingDirectory -Recurse -Exclude "bin/", "obj/", "Dockerfile", "*.Development.json"
Copy-Item -Path .\src\DemoCluster.Silo\Dockerfile -Destination $stagingDirectory

Write-Host "Files having been copied to " $stagingDirectory " starting Docker build process"
cd $stagingDirectory
docker build -t testsilo .

Write-Host "Docker image successful resetting directory"
cd ..