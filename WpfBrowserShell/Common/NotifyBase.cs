using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;


namespace WpfBrowserShell.Common
{
    static class NotifyExtension
    {
        internal static bool ChangeAndNotify<T>(this PropertyChangedEventHandler handler,
            ref T field, T value, Expression<Func<T>> memberExpression)
        {
            if (memberExpression == null)
            {
                throw new ArgumentNullException("memberExpression");
            }
            var body = memberExpression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("Lambda must return a property.");
            }
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            field = value;
            var vmExpression = body.Expression as ConstantExpression;
            if (vmExpression != null)
            {
                LambdaExpression lambda = Expression.Lambda(vmExpression);
                Delegate vmFunc = lambda.Compile();
                object sender = vmFunc.DynamicInvoke();

                if (handler != null)
                {
                    handler(sender, new PropertyChangedEventArgs(body.Member.Name));
                }
            }

            
            return true;
        }
    }

    public class NotifyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetValue<T>(ref T field, T value, Expression<Func<T>> memberExpression)
        {
            return PropertyChanged.ChangeAndNotify(ref field, value, memberExpression);
        }
    }
}
