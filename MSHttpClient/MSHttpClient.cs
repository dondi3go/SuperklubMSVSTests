using Superklub;
using System.Linq;
using System.Net.Http;


/// <summary>
/// An implementation of Superklub.IHTTPClient
/// for MS dotnet
/// </summary>
public class MSHttpClient : Superklub.IHttpClient
{
    private HttpClient httpClient = new HttpClient();
    
    /// <summary>
    /// 
    /// </summary>
    public async Task<HttpResponse> PostAsync(
        string url,
        string jsonStringInput,
        List<CustomRequestHeader>? customRequestHeaders,
        CustomResponseHeader? customResponseHeader)
    {
        // Create POST request
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url),
            Content = new StringContent(jsonStringInput),
            Method = HttpMethod.Post,
        };

        // Add request header (if exists)
        AddRequestHeaders(request, customRequestHeaders);

        // Send request
        HttpResponseMessage response;
        try
        {
            response = await httpClient.SendAsync(request);
        }
        catch (Exception e)
        {
            return new HttpResponse(0, e.Message);
        }

        // Check return code
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            return new HttpResponse((int)response.StatusCode, e.Message);
        }

        // Get response payload
        var jsonStringOutput = await response.Content.ReadAsStringAsync();

        // Get response header (if exists)
        var headerValue = GetResponseHeader(response, customResponseHeader);

        return new HttpResponse((int)response.StatusCode, jsonStringOutput, headerValue);
    }

    /// <summary>
    /// 
    /// </summary>
    public async Task<HttpResponse> GetAsync(
        string url,
        List<CustomRequestHeader>? customRequestHeaders,
        CustomResponseHeader? customResponseHeader)
    {
        // Create GET request
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url),
            Method = HttpMethod.Get,
        };

        // Add request header (if exists)
        AddRequestHeaders(request, customRequestHeaders);

         // Send request
         HttpResponseMessage response;
        try
        {
            response = await httpClient.SendAsync(request);
        }
        catch (Exception e)
        {
            return new HttpResponse(0, e.Message);
        }

        // Check return code
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            return new HttpResponse((int)response.StatusCode, e.Message);
        }

        // Get response payload
        var jsonStringOutput = await response.Content.ReadAsStringAsync();

        // Get response header (if exists)
        var headerValue = GetResponseHeader(response, customResponseHeader);

        return new HttpResponse((int)response.StatusCode, jsonStringOutput, headerValue);
    }

    /// <summary>
    /// Add a header (key string and value) to an HttpRequestMessage
    /// </summary>
    private void AddRequestHeaders(HttpRequestMessage request, List<CustomRequestHeader>? customRequestHeaders)
    {
        if (customRequestHeaders == null)
        {
            return;
        }
        foreach (var customRequestHeader in customRequestHeaders)
        {
            if(!customRequestHeader.IsEmpty())
            {
                request.Headers.Add(customRequestHeader.Key, customRequestHeader.Value);
            }
        }
    }

    /// <summary>
    /// Extract a header value from an HttpResponseMessage
    /// </summary>
    private string GetResponseHeader(HttpResponseMessage response, CustomResponseHeader? customResponseHeader)
    {
        if (customResponseHeader != null && !customResponseHeader.IsEmpty())
        {
            if (response.Headers.Contains(customResponseHeader.Key))
            {
                var headerValues = response.Headers.GetValues(customResponseHeader.Key);
                if (headerValues.Count() > 0)
                {
                    return headerValues.First();
                }
            }
        }
        return string.Empty;
    }
}