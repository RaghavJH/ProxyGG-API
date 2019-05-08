using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProxyGG
{

    /// <summary>
    /// The formats available from the API.
    /// Format.JSON, Format.DOM, Format.TABLE
    /// </summary>
    public enum Format
    {
        JSON,
        DOM,
        TABLE
    }

    /// <summary>
    /// The types of proxies you can get from the API.
    /// ProxyType.HTTP, ProxyType.HTTPS, ProxyType.SOCKS, ProxyType.GOOGLE,
    /// ProxyType.SNEAKER, ProxyType.SHOPIFY, ProxyType.EBAY, ProxyType.STRAWPOLL
    /// </summary>
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

        /// <summary>
        /// Class <c>ProxyGGAPI</c> allows you to get proxies based on type and format from proxy.gg
        /// </summary>
        /// <param name="API_KEY"></param>
        public ProxyGGAPI(string API_KEY)
        {
            this.API_ENDPOINT = $"https://api.proxy.gg/get.php?key={API_KEY}";
            //&format={0}&type={1}&qty={2}
        }

        /// <summary>
        /// Get proxies based on the proxy type, quantity, and format specified.
        /// Country is optional.  May throw network-related exceptions
        /// that need to be handled.
        /// </summary>
        /// <param name="proxyType">
        /// The type of proxy, e.g. ProxyType.HTTPS
        /// This method may throw an InvalidProxyTypeException, so handle accordingly. 
        /// </param>
        /// <param name="quantity">
        /// The number of proxies constrained between 1 and 1000.
        /// This method may throw a QuantityOutOfBoundsException, so handle accordingly. 
        /// </param>
        /// <param name="format">
        /// The format to receive the proxies in, e.g. Format.JSON
        /// This method may throw an InvalidFormatException, so handle accordingly.
        /// </param>
        /// <param name="country">
        /// Optional parameter. Will filter the proxies by 2-letter country code specified.
        /// If an invalid country code is specified, response will be empty.
        /// </param>
        /// <returns>The proxies as a string</returns>
        public async Task<string> GetRawProxiesAsync(ProxyType proxyType, int quantity, Format format, string country = null)
        {
            ensureQuantity(quantity);

            string constructedEndpoint = 
                $"{this.API_ENDPOINT}&format={GetFormat(format)}&type={GetProxyType(proxyType)}&qty={quantity}";


            if (country != null)
                constructedEndpoint += $"&country={country}";

            return await GetUrlAsync(constructedEndpoint);
        }

        /// <summary>
        /// Get proxies as a list of strings (LinkedList) May throw network-related exceptions
        /// that need to be handled.
        /// </summary>
        /// <param name="proxyType">
        /// The type of proxy, e.g. ProxyType.HTTPS
        /// This method may throw an InvalidProxyTypeException, so handle accordingly. 
        /// </param>
        /// <param name="quantity">
        /// The number of proxies constrained between 1 and 1000.
        /// This method may throw a QuantityOutOfBoundsException, so handle accordingly. 
        /// </param>
        /// <param name="country">
        /// Optional parameter. Will filter the proxies by 2-letter country code specified.
        /// If an invalid country code is specified, response will be empty.
        /// </param>
        /// <returns>A LinkedList of strings with each proxy in format ip:port</returns>
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

        /// <summary>
        /// Sends a HTTP GET request to a specified URL asynchronously.
        /// </summary>
        /// <param name="url">The URL to send the GET request to</param>
        /// <returns>The body of the response</returns>
        private async Task<string> GetUrlAsync(string url)
        {
            using(HttpClient httpClient = new HttpClient())
            {
                return await httpClient.GetStringAsync(url);
            }
        }

        /// <summary>
        /// Evaluates the format from an enum to a string
        /// </summary>
        /// <param name="format">The format as an enum object</param>
        /// <returns>The format string that fits the API parameter</returns>
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

        /// <summary>
        /// Evaluates the proxy type from an enum to a string
        /// </summary>
        /// <param name="proxyType">The type of proxy as an enum object</param>
        /// <returns>The proxy type string that fits the API parameter</returns>
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

        /// <summary>
        /// Makes sure the quantity entered is within bounds. Throws QuantityOutOfBoundsException.
        /// </summary>
        /// <param name="quantity">The quantity entered by the user</param>
        private void ensureQuantity(int quantity)
        {
            if(quantity < 1 || quantity > 1000)
            {
                throw new QuantityOutOfBoundsException("Quantity must be more than 1 and under 1000");
            }
        }
    }
}
