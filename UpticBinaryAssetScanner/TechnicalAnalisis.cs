using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpticBinaryAssetScanner
{
    public delegate void AnalisisResult(TechnicalAnalisis result);

    public class TechnicalAnalisis
    {
       // var myList = new List<string>();
        private WebBrowser browser;
        private Dictionary<String, int> analisis;
        private Dictionary<string,List<string>> assests;
        public event AnalisisResult OnAnalisisResult;

        public TechnicalAnalisis(JObject quote)
        {
            browser = new WebBrowser();

            browser.Navigated += Browser_Navigated;
            browser.ScriptErrorsSuppressed = true;
            analisis = new Dictionary<String, int>();
            assests = new Dictionary<string, List<string>>();
            Quote = quote;
        }

        public JObject Quote
        {
            get;
            private set;
        }

        public Dictionary<String, int> AnalisisResult
        {
            get
            {
                return analisis;
            }
        }

        public Dictionary<String, List<string>> assestsResult
        {
            get
            {
                return assests;
            }
        }

        public void Analize()
        {
            analisis = new Dictionary<String, Int32>();
            assests = new Dictionary<string, List<string>>();
            browser.Navigate(Quote["Url"].Value<String>());
        }

        private void Browser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            try
            {
                var html = browser.DocumentText
                    .Replace("\n", "")
                    .Replace("\r", "")
                    .Replace("\t", "");

                var pattern = @"<table.*?class=(""|')(?<class>.*?)(""|').*?>.*?</table>";

                var regex = new Regex(pattern);

                if (regex.IsMatch(html))
                {
                    var matches = regex.Matches(html);

                    foreach (var table in matches)
                    {
                        if (table.ToString().Contains("technicalIndicatorsTbl"))
                        {
                            var txtTable = table.ToString();
                            var patterBody = @"<tbody>.*?</tbody>";

                            this.analisis = new Dictionary<String, int>();
                            this.assests = new Dictionary<String,List<string>>();

                            regex = new Regex(patterBody);

                            if (regex.IsMatch(txtTable))
                            {
                                var tbody = regex.Matches(txtTable)[0].ToString();
                                var patterRow = @"<tr.*?id=(""|')(?<id>.*?)(""|').*?>.*?</tr>";

                                regex = new Regex(patterRow);

                                if (regex.IsMatch(tbody))
                                {
                                    var trs = regex.Matches(tbody);

                                    foreach (var row in trs)
                                    {
                                        var tds = row.ToString();
                                        var patternTd = @"<td.*?>.*?</td>";

                                        regex = new Regex(patternTd);

                                        if (regex.IsMatch(tds))
                                        {
                                            var cols = regex.Matches(tds);
                                            var result = cols[2].ToString().Split('>')[2].Split('<')[0];
                                            var assetname=cols[0].ToString().Split('>')[1].Split('<')[0];

                                            if (analisis.Keys.Contains<String>(result))
                                            {
                                               
                                                 analisis[result]++;
                                                // string []x = assests[result];
                                                //var list = new List<string>();
                                                //list.Add(assetname);
                                                assests[result].Add(assetname);

                                            }
                                            else
                                            {
                                                analisis.Add(result,1);
                                                var list = new List<string>();
                                                list.Add(assetname);
                                                assests.Add(result, list);
                                            }
                                        }
                                    }
                                    

                                      OnAnalisisResult?.Invoke(this);
                                    //OnAnalisisResult += Form1.TecnicalAnalisis_OnAnalisisResult;
                                  //  TecnicalAnalisis_OnAnalisisResult(tecnicalAnalisis);
                                }
                            }
                        }
                    }
                }
              
            }
            catch { }
        }
    }
}
