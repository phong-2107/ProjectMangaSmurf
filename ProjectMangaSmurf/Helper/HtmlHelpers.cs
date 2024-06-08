using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace ProjectMangaSmurf.Helper
{
    public static class HtmlHelpers
    {
        public static IHtmlContent RenderStars(this IHtmlHelper htmlHelper, int rating)
        {
            var starsOutput = new StringBuilder();
            int remainingStars = 5 - rating;

            for (int i = 0; i < rating; i++)
            {
                starsOutput.Append("<i class='ri-star-fill' style='color: #FAB818;'></i>");
            }

            for (int i = 0; i < remainingStars; i++)
            {
                starsOutput.Append("<i class='ri-star-fill'></i>");
            }

            return new HtmlString(starsOutput.ToString());
        }
    }
}
