using System;
using System.Reflection;
using System.Resources;
using Plugin.Multilingual;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DVRMobile.Internationalization
{
    [ContentProperty("Text")]
    public class StringResources : IMarkupExtension
    {
        const string ResourceId = "DVRMobile.Internationalization.AppResources";

        static readonly Lazy<ResourceManager> resmgr =
            new Lazy<ResourceManager>(() =>
            new ResourceManager(ResourceId, typeof(StringResources).GetTypeInfo().Assembly));

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            var translation = resmgr.Value.GetString(Text, CrossMultilingual.Current.CurrentCultureInfo);
            if (translation == null)
                translation = Text;
            return translation;
        }

        public static string Find(string key)
        {
            string content = resmgr.Value.GetString(key, CrossMultilingual.Current.CurrentCultureInfo);
            if (content == null)
                return key;

            return content;
        }
    }
}
