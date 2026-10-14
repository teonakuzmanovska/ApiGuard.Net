$url = "http://localhost:5069/api/sample/validateAndCreate"

$body = @{
    name = "John"
    age = 25
    currentDepartment = 0
    pastDepartments = @(1)
} | ConvertTo-Json

for ($i = 1; $i -le 4; $i++) {
    try {
        $response = Invoke-WebRequest `
            -Uri $url `
            -Method POST `
            -ContentType "application/json" `
            -Body $body `
            -UseBasicParsing

        Write-Host "Request $i -> $($response.StatusCode)"
    }
    catch {
        Write-Host "Request $i -> $($_.Exception.Response.StatusCode.value__)"
    }
}