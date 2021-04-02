using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CardFez
{
    public  interface IRestClient
    {
        Task<T> GetAsync<T>(string url, bool UseAuthToken = true);
    }
}
