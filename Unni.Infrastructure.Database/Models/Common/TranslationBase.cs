#pragma warning disable CS8618
#pragma warning disable CS8601
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Unni.Infrastructure.Database.Extensions;

namespace Unni.Infrastructure.Database.Models.Common
{
    public class TranslationBase
    {
        public string TranslationsJson
        {
            get
            {
                return JsonConvert.SerializeObject(Translations);
            }
            set
            {
                Translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
            }
        }
        [NotMapped]
        public Dictionary<string, string> Translations { get; set; }
        public string GetLocal()
        {
            return CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        }
        public string GetName()
        {
            string local = GetLocal();
            if (Translations.ContainsKey(local))
                return Translations[local];
            if (Translations.ContainsKey("en"))
                return Translations["en"];
            return Translations.FirstOrDefault().Value;
        }
        public void SetName(string name)
        {
            Translations[GetLocal()] = name;
        }
        public void EraseTranslationsExceptCurrent()
        {
            string name = GetName();
            Translations = new Dictionary<string, string>();
            SetName(name);
        }
        public void SetFirstCharToUpper()
        {
            foreach (var translation in Translations)
            {
                Translations[translation.Key] = translation.Value?.FirstCharToUpper();
            }
        }
    }
}
