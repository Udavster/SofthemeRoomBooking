using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace SofthemeRoomBooking.Helpers
{
    public static class Validator
    {

        public static MvcHtmlString SignalValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> ex, object htmlAttributes)
        {
            var expression = ExpressionHelper.GetExpressionText(ex);
            var modelName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expression);
            var modelState = htmlHelper.ViewData.ModelState[modelName];
            var modelErrors = modelState == null ? null : modelState.Errors;
            var modelError = ((modelErrors == null) || (modelErrors.Count == 0))
                ? null
                : modelErrors.FirstOrDefault(m => !String.IsNullOrEmpty(m.ErrorMessage)) ?? modelErrors[0];
            var validationMessage = htmlHelper.ValidationMessageFor(ex);

            var containerBuilder = GetMessageContainerBuilder<TModel>(validationMessage.ToString(), htmlAttributes);

            if (modelError != null)
            {
                return MvcHtmlString.Create(containerBuilder.ToString(TagRenderMode.Normal));
            }
            containerBuilder.AddCssClass("hidden");

            return MvcHtmlString.Create(containerBuilder.ToString(TagRenderMode.Normal));
        }
        public static MvcHtmlString BigInvalidMessage<TModel>(this HtmlHelper<TModel> htmlHelper, string message, object messageHtmlAttributes)
        {
            object signalHtmlAttributes = new { @class = "error-box__signal-big" };
            object containerHtmlAttributes = new { @class = "form__big-error" };
            var containerBuilder = GetMessageContainerBuilder<TModel>(message, messageHtmlAttributes,
                signalHtmlAttributes, containerHtmlAttributes);

            //containerBuilder.AddCssClass("hidden");

            return MvcHtmlString.Create(containerBuilder.ToString(TagRenderMode.Normal));
        }

        private static TagBuilder GetMessageContainerBuilder<TModel>(string validationMessage, object messageHtmlAttributes,
            object signalHtmlAttributes = null, object containerHtmlAttributes = null)
        {
            TagBuilder containerBuilder = new TagBuilder("div");
            containerBuilder.MergeAttributes(new RouteValueDictionary(containerHtmlAttributes));
            containerBuilder.AddCssClass("error-box");

            TagBuilder signalBuilder = new TagBuilder("i");
            signalBuilder.MergeAttributes(new RouteValueDictionary(signalHtmlAttributes));
            signalBuilder.AddCssClass("fa");
            signalBuilder.AddCssClass("fa-exclamation-circle");
            signalBuilder.AddCssClass("error-box__signal");
            signalBuilder.MergeAttribute("aria-hidden", "true");

            TagBuilder textBuilder = new TagBuilder("span");
            textBuilder.MergeAttributes(new RouteValueDictionary(messageHtmlAttributes));
            textBuilder.InnerHtml += validationMessage;

            containerBuilder.InnerHtml += signalBuilder.ToString(TagRenderMode.Normal);
            containerBuilder.InnerHtml += textBuilder.ToString(TagRenderMode.Normal);

            return containerBuilder;
        }
    }
}