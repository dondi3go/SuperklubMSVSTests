using Superklub;
using System.Net.Http;


/// <summary>
/// An implementation of Superklub.IHTTPClient
/// for MS dotnet
/// </summary>
public class MSHttpClient : Superklub.IHttpClient
{
    private HttpClient httpClient = new HttpClient();
    private string requestPath;
        
    /// <summary>
    /// 
    /// </summary>
    public MSHttpClient(Uri baseAddress, string apiPath, string channelName)
    { 
        httpClient.BaseAddress = baseAddress;
        this.requestPath = string.Join("/", apiPath, channelName);
    }

    /// <summary>
    /// 
    /// </summary>
    public async Task<HttpResponse> PostAsync(string jsonStringInput)
    {
        // Create HTTP Content
        var httpContent = new StringContent(jsonStringInput);

        // Perform HTTP request
        HttpResponseMessage response;
        try
        {
            response = await httpClient.PostAsync(requestPath, httpContent);
        }
        catch
        {
            return new HttpResponse(0, "");
        }

        // Check HTTP return code
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch
        {
            return new HttpResponse((int)response.StatusCode, "");
        }

        var jsonStringOutput = await response.Content.ReadAsStringAsync();

        return new HttpResponse((int)response.StatusCode, jsonStringOutput);
    }

    /// <summary>
    /// 
    /// </summary>
    public async Task<HttpResponse> GetAsync()
    {
        // Perform HTTP request
        HttpResponseMessage response;
        try
        {
            response = await httpClient.GetAsync(requestPath);
        }
        catch
        {
            return new HttpResponse(0, "");
        }

        // Check HTTP return code
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch
        {
            return new HttpResponse((int)response.StatusCode, "");
        }

        var jsonStringOutput = await response.Content.ReadAsStringAsync();

        return new HttpResponse((int)response.StatusCode, jsonStringOutput);
    }
}