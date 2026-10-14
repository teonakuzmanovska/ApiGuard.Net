$url = "http://localhost:5069/api/sample/rateLimiting/policy1"

for ($i = 1; $i -le 2; $i++) {
    try {
        $response = Invoke-WebRequest `
            -Uri $url `
            -Method GET `
            -UseBasicParsing

        Write-Host "Policy 1 - Request $i -> $($response.StatusCode)"
    }
    catch {
        Write-Host "Policy 1 - Request $i -> $($_.Exception.Response.StatusCode.value__)"
    }
}