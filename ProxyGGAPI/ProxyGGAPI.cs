using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProxyGG
{

    //Formats
    public enum Format
    {
        JSON,
        DOM,
        TABLE
    }

    //Types of proxies
    public enum ProxyType
    {
        HTTP,
        HTTPS,
        SOCKS,
        GOOGLE,
        SNEAKER,
        SHOPIFY,
        EBAY,
        STRAWPOLL
    }
    public class ProxyGGAPI
    {
        private readonly string API_ENDPOINT;
        private readonly Regex proxyRegex = new Regex(@"<proxy>((\d{1,3}.){3}\d{1,3}:\d{2,5})<\/proxy>", RegexOptions.Compiled);

        public ProxyGGAPI(string API_KEY)
        {
            this.API_ENDPOINT = $"https://api.proxy.gg/get.php?key={API_KEY}";
            //&format={0}&type={1}&qty={2}
        }

        //Default format is JSON
        public async Task<string> GetRawProxiesAsync(ProxyType proxyType, int quantity, Format format, string country = null)
        {
            ensureQuantity(quantity);

            string constructedEndpoint = 
                $"{this.API_ENDPOINT}&format={GetFormat(format)}&type={GetProxyType(proxyType)}&qty={quantity}";


            if (country != null)
                constructedEndpoint += $"&country={country}";

            return await GetUrlAsync(constructedEndpoint);
        }

        public async Task<LinkedList<string>> GetProxiesAsync(ProxyType proxyType, int quantity, string country = null)
        {
            LinkedList<string> proxies = new LinkedList<string>();

            string rawProxies = await GetRawProxiesAsync(proxyType, quantity, Format.DOM, country);

            MatchCollection matches = proxyRegex.Matches(rawProxies);

            foreach(Match match in matches)
            {
                proxies.AddLast(match.Groups[1].ToString());
            }

            return proxies;
        }

        private async Task<string> GetUrlAsync(string url)
        {
            using(HttpClient httpClient = new HttpClient())
            {
                return await httpClient.GetStringAsync(url);
            }
        }

        private string GetFormat(Format format)
        {
            switch(format)
            {
                case Format.JSON: return "json";
                case Format.DOM: return "dom";
                case Format.TABLE: return "table";
                default:
                    throw new InvalidFormatException("Format not recognized. Use either json, dom or table");
            }
        }

        private string GetProxyType(ProxyType proxyType)
        {
            switch(proxyType)
            {
                case ProxyType.HTTP: return "http";
                case ProxyType.HTTPS: return "https";
                case ProxyType.SOCKS: return "socks";
                case ProxyType.GOOGLE: return "google";
                case ProxyType.SNEAKER: return "sneaker";
                case ProxyType.SHOPIFY: return "shopify";
                case ProxyType.EBAY: return "ebay";
                case ProxyType.STRAWPOLL: return "strawpoll";
                default:
                    throw new InvalidProxyTypeException("Proxy type not recognized. Use either http, https, socks, google, sneaker, shopify, ebay or strawpoll.");
            }
        }

        private void ensureQuantity(int quantity)
        {
            if(quantity < 1 || quantity > 1000)
            {
                throw new QuantityOutOfBoundsException("Quantity must be more than 1 and under 1000");
            }
        }
    }
}
