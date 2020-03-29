using System.IO;
using System.Xml.Serialization;

namespace Yeeter
{
    public class XmlLoader
    {
        public static T Load<T>(string path)
        {
            using (var reader = new FileStream(path, FileMode.Open))
            {
                return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
            }
        }
    }
}