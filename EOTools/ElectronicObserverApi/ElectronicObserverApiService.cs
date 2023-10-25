﻿using EOTools.Tools;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EOTools.ElectronicObserverApi;

public class ElectronicObserverApiService
{
    private HttpClient Client { get; }

    public ElectronicObserverApiService()
    {
        Client = new HttpClient();
        Initialize();
        // TODO : reinit after changing parameters
    }

    public void Initialize()
    {
        Client.BaseAddress = new Uri(AppSettings.ElectronicObserverApiUrl);
        Client.DefaultRequestHeaders.Clear();
        Client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", AppSettings.ElectronicObserverApiKey);
    }

    public async Task<T?> GetJson<T>(string url)
    {
        return await Client.GetFromJsonAsync<T>(url).ConfigureAwait(false);
    }

    public async Task<bool> Put(string url)
    {
        try
        {
            HttpResponseMessage response = await Client.PutAsync(url, null);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (HttpRequestException ex)
        {
            await App.ShowErrorMessage(ex);
            return false;
        }
    }
}

