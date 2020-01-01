using ClientWeb.Models.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ClientWeb.Models.BLL
{
    public static class Tools
    {

        public static string GetHtmldetail(string ConfigPath, string FileName, string F_UserName)
        {
            string path = Tools.ReturnPathPhysicalMode(ConfigPath, F_UserName, "AdminAddress", "GetHtmldetail()");

            //WebClient WebClient = new WebClient();
            //string YourContent = WebClient.DownloadString(path + ContentFour);
            //return YourContent
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = client.GetAsync(path + FileName).Result)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (HttpContent content = response.Content)
                            {
                                return content.ReadAsStringAsync().Result;
                            }
                        }
                        else
                            return "<p class=\"text-center\">این پست حاوی محتوا نمی باشد</p>";
                    }
                }
            }
            catch { return GetHtmldetail(ConfigPath, FileName, F_UserName); }

        }
        public static List<SelectListItem> MenuTypesCombo()
        {
            List<SelectListItem> Items = new List<SelectListItem>();
            Items.Add(new SelectListItem() { Text = "استاتیکی", Value = "Static" });
            Items.Add(new SelectListItem() { Text = "داینامیکی", Value = "Dynamic" });
            Items.Add(new SelectListItem() { Text = "هیچ کدام", Value = "None" });
            return Items;
        }

        public static string PriceString(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string Result = "";
                string temp = "";
                int count = 0;
                for (var i = str.Length - 1; i > -1; i--)
                {
                    temp = str[i] + "";
                    if (count == 3)
                    {
                        Result = temp + ',' + Result;
                        count = 0;
                    }
                    else
                        Result = temp + Result;
                    count++;
                }
                return Result.TrimEnd(',');
            }
            return "";
        }
        public static SelectList GetStates(int SelectedId)
        {
            List<SelectListItem> Items = new List<SelectListItem>();
            Items.Add(new SelectListItem() { Text = "استان محل استقرار ...", Value = "", Selected = true });
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Utility/GetStates");
            var Object = JsonConvert.DeserializeObject<List<AddressDataModel>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            foreach (var item in Object as List<AddressDataModel>)
            {
                Items.Add(new SelectListItem() { Text = item.Name, Value = item.ID + "" });
            }
            return new SelectList(Items, "Value", "Text", SelectedId);
        }
        public static SelectList GetCity(int SelectedId, int ID = 0)
        {
            List<SelectListItem> Items = new List<SelectListItem>();
            if (ID == 0)
                Items.Add(new SelectListItem() { Text = "شهر محل استقرار ...", Value = "" });
            if (ID != 0)
            {
                var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Utility/GetCity?stateId=" + ID);
                var Object = JsonConvert.DeserializeObject<List<AddressDataModel>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                foreach (var item in Object as List<AddressDataModel>)
                {
                    Items.Add(new SelectListItem() { Text = item.Name, Value = item.ID + "" });
                }
            }
            return new SelectList(Items, "Value", "Text", SelectedId);
        }

        public static List<SelectListItem> ChartTypes()
        {
            List<SelectListItem> Items = new List<SelectListItem>();
            Items.Add(new SelectListItem { Text = "نمودار دایره ای", Value = "PieChart" });
            Items.Add(new SelectListItem { Text = "نمودار حلقه ای", Value = "DoughnutChart" });
            Items.Add(new SelectListItem { Text = "نمودار میله ای", Value = "BarChart" });
            return Items;
        }

        public static string ReturnPath(string ConfigPath, string F_UserName, string Caller)
        {
            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("UsersFoldersPath");
            string Path = string.Format(section[ConfigPath], F_UserName);
            MakeVaidPath(Path, Caller, false, F_UserName);
            return Path;
        }

        public static string ImageSave_Gallery(HttpPostedFileBase Content_Two, string PathForSave)
        {
            string path = PathForSave;
            string extension = Path.GetExtension(Content_Two.FileName);
            string curFile = "";
            string RandomValueString;
            Random rnd = new Random();
            do
            {

                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                RandomValueString = new string(Enumerable.Repeat(chars, 12)
                 .Select(s => s[rnd.Next(s.Length)]).ToArray());
                curFile = path + "/" + RandomValueString + extension;  //Your path
            } while (File.Exists(curFile));
            WebImage img = new WebImage(Content_Two.InputStream);
            string newextension = img.ImageFormat;
            if (newextension.ToLower() == "jpeg")
            {
                newextension = "jpg";
            }
            if (img.Width < 790 || img.Height < 460)
            {
                int wi;
                int hi;
                // maintain the aspect ratio despite the thumbnail size parameters
                if (img.Width > img.Height)
                {
                    wi = 790;
                    hi = (int)(img.Height * ((decimal)790 / img.Width));
                }
                else
                {
                    hi = 460;
                    wi = (int)(img.Width * ((decimal)460 / img.Height));
                }
                img.Resize(wi, hi);
            }
            img.Save(path + "/" + RandomValueString + "." + newextension);

            return RandomValueString + "." + newextension;
        }

        public static string ReturnPathPhysicalMode(string ConfigPath, string F_UserName, string DomainAddress, string Caller)
        {
            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("UsersFoldersPath");
            string Path = ConfigurationManager.AppSettings[DomainAddress] + string.Format(section[ConfigPath], F_UserName);
            return Path; 
        }
        public static string ReturnPathVirtualMode(string ConfigPath, string F_UserName, string DomainAddress, string Caller)
        {
            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("UsersFoldersPath");
            string Path = string.Format(section[ConfigPath], F_UserName);
            return Path;
        }

        public static string CacheHtml(string ViewPath,string FileName,string profile)
        {
            string htmlresult;
            string url = Tools.ReturnPathPhysicalMode(ViewPath, profile, "AdminAddress", "PageDetail()") + FileName;
            var partialUri = new Uri(url);
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        htmlresult = content.ReadAsStringAsync().Result;
                    }
                }
            }

            var partialLocalName = Path.ChangeExtension(partialUri.LocalPath.Replace('/', '_'), "cshtml");

      string _remotePartialsPath="~" +ReturnPath("ViewPath", profile, "CacheHtml()");
  
            var partialMappedPath =HttpContext.Current.Server.MapPath(_remotePartialsPath +"/"+ partialLocalName);
            File.WriteAllText(partialMappedPath, htmlresult, System.Text.Encoding.UTF8);

            // add to cache


            return _remotePartialsPath + "/" + partialLocalName;
        }


        public static string ViewPath(string profile , string action,string controller) 
        {
            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("UsersFoldersPath");
            return  "~"+string.Format(section["WebsiteThemePath"], profile)+"Views/"+ controller +"/"+ action+ ".cshtml";


        }

        public static string[] ProfilesPathList()
        {
            string[] profiles = ConfigurationManager.AppSettings["Profiles"].Split(',');
            //List<string> ProfilesPath = new List<string>();
            //NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("UsersFoldersPath");

            //for (int i= 0; i < profiles.Count(); i++)
            //{
            //    ProfilesPath.Add(string.Format(section["WebsiteThemeViewPath"], profiles[i]));
            //}

            //  return ProfilesPath.ToArray();
            return profiles;
        }




        public static string ReturnPathPhysicalMode(string ConfigPath, string F_UserName, string Caller)
        {
            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("UsersFoldersPath");
            string Path = HttpContext.Current.Server.MapPath("~" + string.Format(section[ConfigPath], F_UserName));
            MakeVaidPath(Path, Caller, true, F_UserName);
            return Path;
        }

        public static void MakeVaidPath(string Path, string Caller, bool isPhysicalPath, string F_UserName)
        {
            if (!isPhysicalPath)
            {
                Path = HttpContext.Current.Server.MapPath(Path);
            }
            if (!System.IO.Directory.Exists(Path))
            {
                System.IO.Directory.CreateDirectory(Path);
                PipoLog(string.Format("Log : Directory Not Exist <<{0}>> Called by {1} At Time {2} For User {3}", Path, Caller, DateTime.Now, F_UserName));
            }
        }
        public static void PipoLog(string Content)
        {
            System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/App_Data/PipoLog/PipoLog.txt"), Content + Environment.NewLine);
        }

        public static List<SelectListItem> MenuWeightsCombo()
        {
            List<SelectListItem> Weights = new List<SelectListItem>();
            for (int i = 0; i < 10; i++)
                Weights.Add(new SelectListItem() { Text = i + "", Value = i + "" });
            return Weights;
        }

        public static WebsiteProfileSetting GetWebsiteProfileSetting(string profile,string url)
        {
            using (var client = new HttpClient()) 
            {
                HttpRequestMessage req = new HttpRequestMessage();
                req.RequestUri = new Uri(ConfigurationManager.AppSettings["APIAddress"] + "/api/account/GetExistUser?username=" + profile+"&url="+url);
                req.Method = HttpMethod.Get;
                HttpResponseMessage response = Task.Run(() => client.SendAsync(req)).Result;

                var Result = Task.Run(() => response.Content.ReadAsStringAsync()).Result;
                var Object = JsonConvert.DeserializeObject<WebsiteProfileSetting>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                return Object != null ? Object : null;
            }
        }

        public static PageTitleDataModel MenuParents(int F_MenuID)
        {

            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Menues/GetParentMenues?id=" + F_MenuID);
            var Object = JsonConvert.DeserializeObject<PageTitleDataModel>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new PageTitleDataModel() { Name = "یافت نشد" };
        }

        public static AccountAuthorizeModel IsAuthenticated(string Token)
        {
            using (var client = new HttpClient())
            {
                HttpRequestMessage req = new HttpRequestMessage();
                req.RequestUri = new Uri(ConfigurationManager.AppSettings["APIAddress"] + "/api/account/isAuthorized");
                req.Method = HttpMethod.Post;
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                HttpResponseMessage response = Task.Run(() => client.SendAsync(req)).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                 return  JsonConvert.DeserializeObject<AccountAuthorizeModel>(Task.Run(() => response.Content.ReadAsStringAsync()).Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                }
                return new AccountAuthorizeModel();
            }
        }




        public static HttpStatusCode SendRequestToUrl(dynamic Obj, string Url, HttpMethod method, string Token = "")
        {
            using (var client = new HttpClient())
            {
                HttpRequestMessage req = new HttpRequestMessage();
                req.Content = new StringContent(JsonConvert.SerializeObject(Obj), Encoding.UTF8, "application/json");
                req.RequestUri = new Uri(Url);
                if (!string.IsNullOrEmpty(Token))
                    req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                req.Method = method;
                try
                {
                    HttpResponseMessage response = Task.Run(() => client.SendAsync(req)).Result;
                    return response.StatusCode;
                }
                catch
                {
                   return SendRequestToUrl(Obj, Url, method, Token);
                }
            }
        }
        public static HttpResponseMessage RegisterSendRequest(dynamic Obj, string Url, HttpMethod method, string Token = "")
        {
            using (var client = new HttpClient())
            {
                HttpRequestMessage req = new HttpRequestMessage();
                req.Content = new StringContent(JsonConvert.SerializeObject(Obj), Encoding.UTF8, "application/json");
                req.RequestUri = new Uri(Url);
                if (!string.IsNullOrEmpty(Token))
                    req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                req.Method = method;
                try
                {
                    return Task.Run(() => client.SendAsync(req)).Result;
                }
                catch
                {
                    return RegisterSendRequest(Obj, Url, method, Token);
                }
            }
        }

        public static async System.Threading.Tasks.Task<HttpStatusCode> SendRequestToUrlAsync(dynamic Obj, string Url, HttpMethod method, string Token = "")
        {
            using (var client = new HttpClient())
            {
                HttpRequestMessage req = new HttpRequestMessage();
                req.Content = new StringContent(JsonConvert.SerializeObject(Obj), Encoding.UTF8, "application/json");
                req.RequestUri = new Uri(Url);
                if (!string.IsNullOrEmpty(Token))
                    req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                req.Method = method;
                try
                {
                    HttpResponseMessage response = await client.SendAsync(req);
                    return response.StatusCode;
                }
                catch
                {
                    return SendRequestToUrlAsync(Obj, Url, method, Token);
                }
            }
        }

        public static async System.Threading.Tasks.Task<string> SendRequestToUrlGetObjectAsync(dynamic Obj, string Url, HttpMethod method, string Token = "")
        {
            using (var client = new HttpClient())
            {
                string result = "";
                HttpRequestMessage req = new HttpRequestMessage();
                req.Content = new StringContent(JsonConvert.SerializeObject(Obj), Encoding.UTF8, "application/json");
                req.RequestUri = new Uri(Url);
                req.Method = method;
                if (!string.IsNullOrEmpty(Token))
                    req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                try
                {
                    HttpResponseMessage response = await client.SendAsync(req);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        result = response.Content.ReadAsStringAsync().Result;
                    return result;
                }
                catch
                {
                    return SendRequestToUrlGetObjectAsync(Obj, Url, method, Token);
                }
            }
        }

        public static async System.Threading.Tasks.Task<string> GetObjectFromRequestAsync(string Url, string Token = "")
        {
            using (var client = new HttpClient())
            {
                string result = "";
                HttpRequestMessage req = new HttpRequestMessage();
                req.RequestUri = new Uri(Url);
                req.Method = HttpMethod.Get;
                if (!string.IsNullOrEmpty(Token))
                    req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                try
                {
                    HttpResponseMessage response = await client.SendAsync(req);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        result = response.Content.ReadAsStringAsync().Result;
                    return result;
                }
                catch
                {
                    return await GetObjectFromRequestAsync(Url,Token);
                }
            }
        }

        public static dynamic GetObjectFromRequest(string Url, string Token = "")
        {
            using (var client = new HttpClient())
            {
                string result = "";
                HttpRequestMessage req = new HttpRequestMessage();
                req.RequestUri = new Uri(Url);
                req.Method = HttpMethod.Get;
                if (!string.IsNullOrEmpty(Token))
                    req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                try
                {
                    HttpResponseMessage response = Task.Run(() => client.SendAsync(req)).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        result = response.Content.ReadAsStringAsync().Result;
                    return result;
                }
                catch
                {
                    return  GetObjectFromRequest(Url, Token);
                }
            }
        }







        public static string ImageSave(HttpPostedFileBase Content_Two, string Type,string profile="@#")
        {
            if (Content_Two != null)
            {
                string path = profile == "@#" ? Tools.ReturnPathPhysicalMode(Type) : Tools.ReturnPathPhysicalMode(Type, profile, "ImageSave");
                string extension = Path.GetExtension(Content_Two.FileName);
                string curFile = "";
                string RandomValueString;
                Random rnd = new Random();
                do
                {
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    RandomValueString = new string(Enumerable.Repeat(chars, 12)
                     .Select(s => s[rnd.Next(s.Length)]).ToArray());
                    curFile = path + "/" + RandomValueString + extension;  //Your path
                } while (File.Exists(curFile));
                WebImage img = new WebImage(Content_Two.InputStream);
                string newextension = img.ImageFormat;
                if (newextension.ToLower() == "jpeg")
                {
                    newextension = "jpg";
                }
                if (img.Width < 790 || img.Height < 460)
                {
                    int wi;
                    int hi;
                    // maintain the aspect ratio despite the thumbnail size parameters
                    if (img.Width > img.Height)
                    {
                        wi = 790;
                        hi = (int)(img.Height * ((decimal)790 / img.Width));
                    }
                    else
                    {
                        hi = 460;
                        wi = (int)(img.Width * ((decimal)460 / img.Height));
                    }
                    img.Resize(wi, hi);
                }
                img.Save(path + "/" + RandomValueString + "." + newextension);
                return RandomValueString + "." + newextension;
            }
            else
                return "NotSaved";
        }

        public static string ReturnPathPhysicalMode(string ConfigPath)
        {
            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("UsersFoldersPath");
            string Path = HttpContext.Current.Server.MapPath("~" + section[ConfigPath]);
            MakeVaidPath(Path, true);
            return Path;
        }

        public static void MakeVaidPath(string Path, bool isPhysicalPath)
        {
            if (!isPhysicalPath)
                Path = HttpContext.Current.Server.MapPath(Path);
            if (!System.IO.Directory.Exists(Path))
                System.IO.Directory.CreateDirectory(Path);
        }



        public static string ConvertNativeDigits(this string text)
        {

            if (text == null)
                return null;
            if (text.Length == 0)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (char character in text)
            {
                if (char.IsDigit(character))
                    sb.Append(char.GetNumericValue(character));
                else
                    sb.Append(character);
            }
            return sb.ToString();


        }
        private static readonly CultureInfo arabic = new CultureInfo("fa-IR");
        private static readonly CultureInfo latin = new CultureInfo("en-US");

        /// <summary>
        /// in tabe jahate tabdile zabane english be arabic ( ta hududi farsi ) estefade mishavad
        /// </summary>
        /// <param name="input">reshteye morede nazar baraye tabdil</param>
        /// <returns>
        /// string
        /// reshteye tabdil shode
        /// </returns>
        public static string ToArabic(string input)
        {
            var arabicDigits = arabic.NumberFormat.NativeDigits;
            for (int i = 0; i < arabicDigits.Length; i++)
            {
                input = input.Replace(i.ToString(), arabicDigits[i]);
            }
            return input;
        }

        public static string ToLatin(string input)
        {
            var latinDigits = latin.NumberFormat.NativeDigits;
            var arabicDigits = arabic.NumberFormat.NativeDigits;
            for (int i = 0; i < latinDigits.Length; i++)
            {
                input = input.Replace(arabicDigits[i], latinDigits[i]);
            }
            return input;
        }

        /// <summary>
        /// in tabe tarikhe miladi ra dar forme datetime gerefte va tarikhe jalali ra dar forme string baz migardanad
        /// </summary>
        /// <param name="date">tarikhe morede nazar jahate tabdil</param>
        /// <returns>
        /// string
        /// tarikhe tabdil shode be jalali
        /// </returns>
        public static string GetDateTimeReturnJalaliDate(DateTime date)
        {
            PersianCalendar p = new PersianCalendar();
            int Month = p.GetMonth(date);
            int Year = p.GetYear(date);
            int Day = p.GetDayOfMonth(date);
            int Hour = p.GetHour(date);
            int Minute = p.GetMinute(date);
            int Second = p.GetSecond(date);
            string result1 = "";
            string result = Tools.ToArabic(Year.ToString()) + '/';
            if (Month.ToString().Count() == 2)
                result += Tools.ToArabic(Month.ToString()) + '/';
            else
                result += '۰' + Tools.ToArabic(Month.ToString()) + '/';
            if (Day.ToString().Count() == 2)
                result += Tools.ToArabic(Day.ToString());
            else
                result += '۰' + Tools.ToArabic(Day.ToString());
            if (Hour.ToString().Count() == 2)
                result1 += Tools.ToArabic(Hour.ToString()) + ':';
            else
                result1 += '۰' + Tools.ToArabic(Hour.ToString()) + ':';
            if (Minute.ToString().Count() == 2)
                result1 += Tools.ToArabic(Minute.ToString()) + ':';
            else
                result1 += '۰' + Tools.ToArabic(Minute.ToString()) + ':';
            if (Second.ToString().Count() == 2)
                result1 += Tools.ToArabic(Second.ToString());
            else
                result1 += '۰' + Tools.ToArabic(Second.ToString());
            string FinalResult = result + " " + result1;

            return FinalResult;
        }

        public static string SpecialJalaliFormat(DateTime date)
        {
            if (date != null && date.ToShortDateString() != "1/1/0001")
            {
                PersianCalendar p = new PersianCalendar();
                int Month = p.GetMonth(date);
                string Week = p.GetDayOfWeek(date).ToString();
                int Year = p.GetYear(date);
                int Day = p.GetDayOfMonth(date);
                List<string> WeekDay = new List<string>() { "شنبه", "یکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنجشنبه", "جمعه" };
                List<string> EnWeekDay = new List<string>() { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
                List<string> Months = new List<string>() { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
                return WeekDay[EnWeekDay.IndexOf(Week)] + " " + Day + " " + Months[Month - 1] + " / سال " + Tools.ToArabic(Year.ToString());
            }
            else
                return "";
        }
        public static string JalaliDateWithoutHour(DateTime date)
        {
            PersianCalendar p = new PersianCalendar();
            int Month = p.GetMonth(date);
            int Year = p.GetYear(date);
            int Day = p.GetDayOfMonth(date);
            string result = Tools.ToArabic(Year.ToString()) + '/';
            if (Month.ToString().Count() == 2)
                result += Tools.ToArabic(Month.ToString()) + '/';
            else
                result += '۰' + Tools.ToArabic(Month.ToString()) + '/';
            if (Day.ToString().Count() == 2)
                result += Tools.ToArabic(Day.ToString());
            else
                result += '۰' + Tools.ToArabic(Day.ToString());

            return result;
        }

        /// <summary>
        /// in tabe tarikhe jalali ra dar forme string gerefte va tarikhe miladi ra dar forme datetime baz migardanad
        /// </summary>
        /// <param name="date">tarikhe jalaliye morede nazar dar forme string</param>
        /// <param name="_Date"></param>
        /// <returns>
        ///  
        /// </returns>
        public static bool GetJalaliDateReturnDateTime(string date, out DateTime _Date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                Regex rex = new Regex(@"^[۰-۹0-9]{4}\/[۰-۹0-9]{2}\/[۰-۹0-9]{2} [۰-۹0-9]{2}:[۰-۹0-9]{2}:[۰-۹0-9]{2}$");
                if (rex.Match(date).Success)
                {
                    string firstpart = date.Substring(0, date.IndexOf(':') - 2);
                    string SecondPart = date.Substring(date.IndexOf(':') - 2);
                    string[] persianDatePartsStart = firstpart.Split('/');
                    string[] persianDatePartsStartHour = SecondPart.Split(':');


                    int persianYearStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStart[0]));
                    int persianMonthStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStart[1]));
                    int persianDayStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStart[2]));
                    int persianHourStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStartHour[0]));
                    int persianMinStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStartHour[1]));
                    int persianSecondStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStartHour[2]));

                    string datetimeString = string.Format("{0}-{1}-{2} {3}:{4}:{5}", persianYearStart, persianMonthStart, persianDayStart, persianHourStart, persianMinStart, persianSecondStart);

                    PersianCalendar pc = new PersianCalendar();
                    try
                    {
                        DateTime start = new DateTime(persianYearStart, persianMonthStart, persianDayStart, persianHourStart, persianMinStart, persianSecondStart, pc);
                        _Date = start;
                        return true; ;
                    }
                    catch
                    {
                        _Date = DateTime.Now;
                        return false;
                    }
                }
            }
            _Date = DateTime.Now;
            return false; ;
        }
    }
}