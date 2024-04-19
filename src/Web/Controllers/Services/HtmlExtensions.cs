﻿using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace DMS.Web.Controllers.Services;

public static class HtmlExtensions
{
    public static IHtmlContent DescriptionFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
    {
        if (html == null) throw new ArgumentNullException(nameof(html));
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        var expressionProvider = html.ViewContext?.HttpContext?.RequestServices?.GetService<ModelExpressionProvider>()
            ?? new ModelExpressionProvider(html.MetadataProvider);
        var modelExpression = expressionProvider.CreateModelExpression(html.ViewData, expression);

        return new HtmlString(modelExpression.Metadata.Description);
    }
}