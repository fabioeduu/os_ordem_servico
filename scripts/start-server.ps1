# Stop any existing dotnet processes
Stop-Process -Name dotnet -Force -ErrorAction SilentlyContinue
Start-Sleep -Seconds 2

# Navigate to API folder
cd "C:\Users\grana\Desktop\sprintC\API"

# Start the server with stdin redirected from $null
Write-Host "🚀 Iniciando servidor..."
& dotnet run --no-launch-browser 2>&1 | Out-Host
