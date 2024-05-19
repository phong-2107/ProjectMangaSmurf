using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;

namespace ProjectMangaSmurf.Models
{
    public class PayPalClient
    {
        public static PayPalEnvironment Environment()
        {
            return new LiveEnvironment("AWP0_RnOapUKzsJv4iUl0THkMSupShZukWMd61q-NIUwf97lll_DOjP-yqQmtvm7MorxdlJ8jyGQaMW2", "EH2uEe6ZrdjPhCy5imGXLtvxOUlr-dd3ldT0WPhZ6AM81NbFMp5Ivfi36oX-IXfrKmOo_iwrznJh5XZv");
        }

        public static PayPalHttp.HttpClient Client()
        {
            return new PayPalHttpClient(Environment());
        }

    }
}
