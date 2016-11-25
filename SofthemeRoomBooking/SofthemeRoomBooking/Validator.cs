using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace SofthemeRoomBooking
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

            TagBuilder containerBuilder = new TagBuilder("div");
            containerBuilder.AddCssClass("error-box");

            TagBuilder signalBuilder = new TagBuilder("i");
            signalBuilder.AddCssClass("fa");
            signalBuilder.AddCssClass("fa-exclamation-circle");
            signalBuilder.AddCssClass("error-box__signal");
            signalBuilder.MergeAttribute("aria-hidden", "true");

            TagBuilder textBuilder = new TagBuilder("span");
            var @class = htmlAttributes.GetType().GetProperty("class").GetValue(htmlAttributes) as String;
            if (@class != null)
            {
                textBuilder.AddCssClass(@class);
            }
            textBuilder.InnerHtml += validationMessage.ToString();
            
            containerBuilder.InnerHtml += signalBuilder.ToString(TagRenderMode.Normal);
            containerBuilder.InnerHtml += textBuilder.ToString(TagRenderMode.Normal);

            if (modelError != null)
            {
                return MvcHtmlString.Create(containerBuilder.ToString(TagRenderMode.Normal));

                //return MvcHtmlString.Create(string.Format("<div class=\"error-box\"><i class=\"fa fa-exclamation-circle error-box__signal\" aria-hidden=\"true\"></i>{0}</div>", result.ToHtmlString()));
            }

            containerBuilder.AddCssClass("hidden");
            //return MvcHtmlString.Create(string.Format("<div class=\"error-box hidden\"><i class=\"fa fa-exclamation-circle error-box__signal\" aria-hidden=\"true\"></i>{0}</div>", result.ToHtmlString()));

            return MvcHtmlString.Create(containerBuilder.ToString(TagRenderMode.Normal));
        }
        public static MvcHtmlString BigInvalidMessage<TModel>(this HtmlHelper<TModel> htmlHelper, string message, object htmlAttributes)
        {
            var validationMessage = message;

            TagBuilder containerBuilder = new TagBuilder("div");
            containerBuilder.AddCssClass("error-box");
            containerBuilder.AddCssClass("form__big-error");

            TagBuilder signalBuilder = new TagBuilder("i");
            signalBuilder.AddCssClass("fa");
            signalBuilder.AddCssClass("fa-exclamation-circle");
            signalBuilder.AddCssClass("error-box__signal");
            signalBuilder.AddCssClass("error-box__signal-big");
            signalBuilder.MergeAttribute("aria-hidden", "true");

            TagBuilder textBuilder = new TagBuilder("span");
            var @class = htmlAttributes.GetType().GetProperty("class").GetValue(htmlAttributes) as String;
            if (@class != null)
            {
                textBuilder.AddCssClass(@class);
            }
            textBuilder.InnerHtml += validationMessage.ToString();

            containerBuilder.InnerHtml += signalBuilder.ToString(TagRenderMode.Normal);
            containerBuilder.InnerHtml += textBuilder.ToString(TagRenderMode.Normal);

            
            return MvcHtmlString.Create(containerBuilder.ToString(TagRenderMode.Normal));
        }
    }
}