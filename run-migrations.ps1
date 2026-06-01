$ErrorActionPreference = "Stop"
$baseDir = $PSScriptRoot

# IdentityServer
Write-Host "Updating IdentityServer..."
Set-Location "$baseDir\IdentityServer\MultiShop.IdentityServer"
dotnet ef database update

# Cargo
Write-Host "Updating Cargo..."
Set-Location "$baseDir\Services\Cargo\MultiShop.Cargo.WebApi"
dotnet ef database update -p ../MultiShop.Cargo.DataAccessLayer

# Comment
Write-Host "Updating Comment..."
Set-Location "$baseDir\Services\Comment\MultiShop.Comment"
dotnet ef database update

# Discount
Write-Host "Updating Discount..."
Set-Location "$baseDir\Services\Discount\MultiShop.Discount"
dotnet ef database update

# Message
Write-Host "Updating Message..."
Set-Location "$baseDir\Services\Message\MultiShop.Message"
dotnet ef database update

# Order
Write-Host "Updating Order..."
Set-Location "$baseDir\Services\Order\Presentation\MultiShop.Order.WebApi"
dotnet ef database update -p ../../Infrastructure/MultiShop.Order.Persistence

Set-Location $baseDir
Write-Host "All migrations applied successfully."
