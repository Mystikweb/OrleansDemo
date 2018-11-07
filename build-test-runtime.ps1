Write-Host "Starting build of Docker container"
Write-Host "Creating staging folder for DemoCluster.Runtime"

$stagingDirectory = ".\RuntimeStaging"

if (Test-Path $stagingDirectory) {
    Write-Host $stagingDirectory " was found....removing...."
    Remove-Item $stagingDirectory -Recurse
}

New-Item -Name $stagingDirectory -ItemType Directory | Out-Null

Copy-Item -Path .\src\DemoCluster -Destination $stagingDirectory -Recurse -Exclude "bin/", "obj/"
Copy-Item -Path .\src\DemoCluster.DAL -Destination $stagingDirectory -Recurse -Exclude "bin/", "obj/"
Copy-Item -Path .\src\DemoCluster.Hosting -Destination $stagingDirectory -Recurse -Exclude "bin/", "obj/"
Copy-Item -Path .\src\DemoCluster.GrainInterfaces -Destination $stagingDirectory -Recurse -Exclude "bin/", "obj/"
Copy-Item -Path .\src\DemoCluster.Runtime -Destination $stagingDirectory -Recurse -Exclude "bin/", "obj/", "Dockerfile"
Copy-Item -Path .\src\DemoCluster.Runtime\Dockerfile -Destination $stagingDirectory

Write-Host "Files having been copied to " $stagingDirectory " starting Docker build process"
cd $stagingDirectory
docker build -t testruntime .

Write-Host "Docker image successful resetting directory"
cd ..