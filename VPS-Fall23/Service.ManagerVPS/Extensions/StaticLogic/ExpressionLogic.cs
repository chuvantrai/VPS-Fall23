using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using MongoDB.Driver;

namespace Service.ManagerVPS.Extensions.StaticLogic;

public static class ExpressionLogic
{
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
    {
        var p = a.Parameters[0];

        var visitor = new SubstExpressionVisitor
        {
            Subst =
            {
                [b.Parameters[0]] = p
            }
        };

        Expression body = Expression.And(a.Body, visitor.Visit(b.Body));
        return Expression.Lambda<Func<T, bool>>(body, p);
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
    {
        var p = a.Parameters[0];

        var visitor = new SubstExpressionVisitor
        {
            Subst =
            {
                [b.Parameters[0]] = p
            }
        };

        Expression body = Expression.Or(a.Body, visitor.Visit(b.Body));
        return Expression.Lambda<Func<T, bool>>(body, p);
    }

    private class SubstExpressionVisitor : ExpressionVisitor
    {
        public readonly Dictionary<Expression, Expression> Subst = new();

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return Subst.TryGetValue(node, out var newValue) ? newValue : node;
        }
    }


    public static IQueryable<T> SortBy<T>(this IQueryable<T> source, params string[] sortExpression) where T : class
    {
        if (sortExpression == null || sortExpression.Length == 0)
            return source;

        IOrderedQueryable<T> orderedQuery = null;
        for (var index = 0; index < sortExpression.Length; index++)
        {
            var exp = sortExpression[index];
            if (string.IsNullOrEmpty(exp))
            {
                continue;
            }

            var sortField = Regex.Replace(sortExpression[index], @"[\+\-]", string.Empty);
            if (sortExpression[index].StartsWith("-"))
            {
                orderedQuery = index == 0
                    ? source.OrderByDescending(sortField)
                    : orderedQuery.ThenByDescending(sortField);
            }
            else
            {
                orderedQuery = index == 0 ? source.OrderBy(sortField) : orderedQuery.ThenBy(sortField);
            }
        }

        return orderedQuery ?? source;
    }

    public static IQueryable<T> SortBy<T>(this IQueryable<T> query, SortDirection sortDirection,
        params Expression<Func<T, object>>[] sortExpressions)
    {
        if (sortExpressions == null || sortExpressions.All(t => t == null)) return query;
        IOrderedQueryable<T> orderedQuery = null;
        for (var i = 0; i < sortExpressions.Length; i++)
        {
            var sortExpItem = sortExpressions[i];
            if (sortExpItem == null) continue;
            if (sortDirection == SortDirection.Descending)
            {
                orderedQuery = i == 0
                    ? query.OrderByDescending(sortExpItem)
                    : orderedQuery?.ThenByDescending(sortExpItem);
            }
            else
            {
                orderedQuery = i == 0 ? query.OrderBy(sortExpItem) : orderedQuery?.ThenBy(sortExpItem);
            }
        }

        return orderedQuery ?? query;
    }

    public static IQueryable<T> SortBy<T>(this IQueryable<T> query, SortDirection sortDirection,
        IEnumerable<Expression<Func<T, object>>> sortExpressions)
    {
        return query.SortBy(sortDirection, sortExpressions?.ToArray());
    }

    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string fieldName) where T : class
    {
        var resultExp = GenerateMethodCall(source, "OrderBy", fieldName);
        return source.Provider.CreateQuery<T>(resultExp) as IOrderedQueryable<T>;
    }

    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string fieldName)
        where T : class
    {
        var resultExp = GenerateMethodCall(source, "OrderByDescending", fieldName);
        return source.Provider.CreateQuery<T>(resultExp) as IOrderedQueryable<T>;
    }

    public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string fieldName)
        where T : class
    {
        var resultExp = GenerateMethodCall(source, "ThenByDescending", fieldName);
        return source.Provider.CreateQuery<T>(resultExp) as IOrderedQueryable<T>;
    }

    public static MethodCallExpression GenerateMethodCall<T>(IQueryable<T> source, string methodName,
        string fieldName) where T : class
    {
        var type = typeof(T);
        var selector = GenerateSelector<T>(fieldName, out var selectorResultType);
        var resultExp = Expression.Call(typeof(Queryable), methodName,
            new[] { type, selectorResultType },
            source.Expression, Expression.Quote(selector));
        return resultExp;
    }

    private static LambdaExpression GenerateSelector<T>(string propertyName, out Type resultType) where T : class
    {
        // Create a parameter to pass into the Lambda expression (Entity => Entity.OrderByField).
        var parameter = Expression.Parameter(typeof(T), "Entity");
        //  create the selector part, but support child properties
        PropertyInfo property;
        Expression propertyAccess;
        if (propertyName.Contains('.'))
        {
            // support to be sorted on child fields.
            var childProperties = propertyName.Split('.');
            property = typeof(T).GetProperty(childProperties[0],
                BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
            propertyAccess = Expression.MakeMemberAccess(parameter, property);
            for (var i = 1; i < childProperties.Length; i++)
            {
                property = property.PropertyType.GetProperty(childProperties[i],
                    BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
            }
        }
        else
        {
            property = typeof(T).GetProperty(propertyName,
                BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
            propertyAccess = Expression.MakeMemberAccess(parameter, property);
        }

        resultType = property.PropertyType;
        // Create the order by expression.
        return Expression.Lambda(propertyAccess, parameter);
    }

    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string fieldName) where T : class
    {
        var resultExp = GenerateMethodCall(source, "ThenBy", fieldName);
        return source.Provider.CreateQuery<T>(resultExp) as IOrderedQueryable<T>;
    }


    public static IEnumerable<T> SortBy<T>(this IEnumerable<T> source, params string[] sortExpression)
        where T : class
    {
        if (sortExpression == null || sortExpression.Length == 0)
            return source;

        for (var index = 0; index < sortExpression.Length; index++)
        {
            var exp = sortExpression[index];
            if (string.IsNullOrEmpty(exp))
            {
                continue;
            }

            var sortField = Regex.Replace(sortExpression[index], @"[\+\-]", string.Empty);
            PropertyDescriptor sortProperty = TypeDescriptor.GetProperties(typeof(T)).Find(sortField, true);
            if (sortExpression[index].StartsWith("-"))
            {
                source = source.OrderByDescending(a => sortProperty.GetValue(a)).ToList();
            }
            else
            {
                source = source.OrderBy(a => sortProperty.GetValue(a)).ToList();
            }
        }

        return source;
    }

    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> predicate)
    {
        return Expression.Lambda<Func<T, bool>>(Expression.Not(predicate.Body), predicate.Parameters[0]);
    }

    public static string[] AddToLastArrayStrings(this string[] listSort,string add)
    {
        // string[] listSort = new string[]{};
        Array.Resize(ref listSort, listSort.Length + 1);
        listSort[listSort.Length - 1] = add;
        return listSort;
    }
    
    public static string GetEnumDescription(Enum value)
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);

        if (name == null) return value.ToString();
        var field = type.GetField(name);
        if (field == null) return value.ToString();
        if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
        {
            return attr.Description;
        }
        return value.ToString();
    }
    
    public static string ConvertPriceToString(double number)
    {
        const double billion = 1000000000;
        const double million = 1000000;
        const double thousand = 1000;

        return number switch
        {
            >= billion => $"{Math.Round(number / billion, 2, MidpointRounding.AwayFromZero)}Tỷ",
            >= million => $"{Math.Round(number / million, 2, MidpointRounding.AwayFromZero)}Triệu",
            >= thousand => $"{Math.Round(number / thousand, 2, MidpointRounding.AwayFromZero)}K",
            _ => number.ToString(CultureInfo.CurrentCulture)
        };
    }
}