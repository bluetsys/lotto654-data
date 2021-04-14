using System;
using System.Linq;
using System.Collections;
using System.Data;
using System.Text;

namespace Lotto645Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var _Body = new System.Net.WebClient()
            {
                Encoding = System.Text.Encoding.GetEncoding(51949),
                Headers = new System.Net.WebHeaderCollection
         {
             "Accept-Language: ko-KR,ko;q=0.9,en-US;q=0.8,en;q=0.7,und;q=0.6,la;q=0.5"
         }
            }.DownloadString("https://dhlottery.co.kr/gameResult.do?method=allWinExel&nowPage=96&drwNoStart=1&drwNoEnd=2000");

            // 딱 1시간만에 완료

            // robots.txt 확인
            //new System.Net.WebClient()
            //{
            //	Encoding = System.Text.Encoding.Default
            //}.DownloadString("https://www.dhlottery.co.kr/robots.txt").Dump();

            var _HtmlDocument = new HtmlAgilityPack.HtmlDocument();
            _HtmlDocument.LoadHtml(_Body);

            var _Nodes = _HtmlDocument.DocumentNode.SelectNodes("//table[2]/tr");

            var _Query = _Nodes.Select(tr => tr
                    .Elements("td")
                    .Select(td => td.InnerText.Trim())
                    .ToArray()
            );

            var _MaxValue = _Query.Select(r => r.Length).Max();

            var _Table = new System.Data.DataTable();

            for (int i = 0; i < _MaxValue; i++)
            {
                _Table.Columns.Add($"co-{i.ToString("00")}", typeof(object));
            }

            foreach (var element in _Query)
            {
                var _Temp = new string[_MaxValue];

                if (element.ToArray().Length == 19)
                {
                    Array.Copy(element, 0, _Temp, 1, element.Length);
                }
                else if (element.ToArray().Length == 20)
                {
                    Array.Copy(element, 0, _Temp, 0, element.Length);
                }

                _Table.Rows.Add(_Temp);
            }

            _Table.Rows.RemoveAt(0);
            _Table.Rows.RemoveAt(0);
            _Table.Columns.Remove("co-00");

            var _Query2 = from row in _Table.AsEnumerable()
                          select new Lotto645
                          {
                              회차 = int.Parse(row.Field<object>(0).ToString()),
                              추첨일 = DateTime.Parse(row.Field<string>(1).Replace(".", "-")),
                              당첨자수1 = int.Parse(row.Field<string>(2).Replace(".", "")),
                              당첨금1 = long.Parse(row.Field<object>(3).ToString().Replace("원", "").Replace(",", "")),
                              당첨자수2 = int.Parse(row.Field<string>(4).Replace(",", "")),
                            당첨금2 = long.Parse(row.Field<string>(5).Replace("원", "").Replace(",", "")),
                              당첨자수3 = int.Parse(row.Field<string>(6).Replace(",", "")),
                                당첨금3 = long.Parse(row.Field<string>(7).Replace("원", "").Replace(",", "")),
                              당첨자수4 = int.Parse(row.Field<string>(8).Replace(",", "")),
                              당첨금4 = long.Parse(row.Field<string>(9).Replace("원", "").Replace(",", "")),
                              당첨자수5 = int.Parse(row.Field<string>(10).Replace(",", "")),
                              당첨금5 = long.Parse(row.Field<string>(11).Replace("원", "").Replace(",", "")),
                              당첨번호 = new int[] {
                              int.Parse(row.Field<string>(12)),
                            int.Parse(row.Field<string>(13)),
                            int.Parse(row.Field<string>(14)),
                            int.Parse(row.Field<string>(15)),
                            int.Parse(row.Field<string>(16)),
                            int.Parse(row.Field<string>(17)),
                        },
                              보너스 = int.Parse(row.Field<string>(18))
                          };

            _Body = Newtonsoft.Json.JsonConvert.SerializeObject(_Query2.ToArray(), Newtonsoft.Json.Formatting.Indented);

            var _Path = "Data";
            System.IO.Directory.CreateDirectory(_Path);
            _Path = System.IO.Path.Combine(_Path, @"legacy-latest.xml");

            System.IO.File.WriteAllText(_Path, _Body, System.Text.Encoding.UTF8);
        }
    }
}
