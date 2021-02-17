using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Portal.Classes;
using Portal.Data;
using Portal.Models;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    public class DataSourceController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DataSourceController(ApplicationDbContext context)
        {
            _context = context;
        }
 

        [HttpGet]
        public object GetTweets(int id, DataSourceLoadOptions loadOptions)
        {
            var records = _context.TwitterRecord.Where(n => n.TopicID == id).ToList();
            return DataSourceLoader.Load(records, loadOptions);
        }

        [HttpGet]
        public object GetAllTweets(DataSourceLoadOptions loadOptions)
        {
            var records = (from rec in _context.TwitterRecord
                          join t in _context.Topic
                          on rec.TopicID equals t.ID
                          select new TwitterRecord
                          {
                              Id = rec.Id,
                              UserName = rec.UserName,
                              TweetCreationTime = rec.TweetCreationTime,
                              TweetHashTags = rec.TweetHashTags,
                              TweetLanguage = rec.TweetLanguage,
                              Sentiment = rec.Sentiment,
                              TweetText = rec.TweetText,
                              ProfileImageURL = rec.ProfileImageURL.Replace("_normal.jpg", ".jpg"),
                              TopicName = t.Name
                          });
            return DataSourceLoader.Load(records, loadOptions);
        }

        [HttpGet]
        public object GetAllTopics(DataSourceLoadOptions loadOptions)
        {
            var records = (from t in _context.Topic
                                select new Topic
                                {
                                    ID = t.ID,
                                    Name = t.Name,
                                    Keywords = t.Keywords,
                                    StatusID = t.StatusID,
                                    DT = t.DT,
                                    TweetsCount = (from recc in _context.TwitterRecord where recc.TopicID == t.ID select recc.Id).Count(),
                                });
            return DataSourceLoader.Load(records, loadOptions);
        }

        [HttpGet]
        public object GetAllTopicsJson()
        {
            var records = (from t in _context.Topic
                           select new Topic
                           {
                               ID = t.ID,
                               Name = t.Name,
                               Keywords = t.Keywords,
                               StatusID = t.StatusID,
                               DT = t.DT,
                               TweetsCount = (from recc in _context.TwitterRecord where recc.TopicID == t.ID select recc.Id).Count(),
                           });
            var json = JsonConvert.SerializeObject(records);
            return json;
        }

        [HttpGet]
        public object GetPieChartData(int tid, DataSourceLoadOptions loadOptions)
        {
            var records = _context.TwitterRecord.Where(n => n.TopicID == tid)
                .GroupBy(p => p.Sentiment)
                .Select(g => new { Sentiment = g.Key, Count = g.Count() })
                .ToList();

            return DataSourceLoader.Load(records, loadOptions);
        }

        [HttpGet]
        public object GetPieChartDataAllTweets(DataSourceLoadOptions loadOptions)
        {
            var records = _context.TwitterRecord
                .GroupBy(p => p.Sentiment)
                .Select(g => new { Sentiment = g.Key, Count = g.Count() })
                .ToList();
            return DataSourceLoader.Load(records, loadOptions);
        }

        [HttpGet]
        public object GetLineChartData(int tid, DataSourceLoadOptions loadOptions)
        {
            var query = _context.TwitterRecord
                .Select(i => new TwitterRecord {
                    Id = i.Id,
                    Sentiment = i.Sentiment,
                    TweetCreationTime= i.TweetCreationTime,
                    TopicID = i.TopicID })
                .Where(n => n.TopicID == tid)
                .OrderBy(x=>x.TweetCreationTime)
                .ToList();

            // Create a table from the query.
            DataTable dt = Global.ToDataTable<TwitterRecord>(query);


            var recordsGroupsBy = dt.AsEnumerable()
                .GroupBy(row => new
                {
                    Date = row.Field<DateTime>("TweetCreationTime").Date,
                    Hour = row.Field<DateTime>("TweetCreationTime").Hour//,
                    //Minute = row.Field<DateTime>("TweetCreationTime").Minute
                });

            var records = new List<LineChart>();
            foreach (var items in recordsGroupsBy)
            {
                int PositiveCount = 0, NegativeCount = 0, NeutralCount = 0;
                foreach(var item in items)
                {
                    if(item[27].ToString().Equals("neutral"))
                    {
                        NeutralCount++;
                    }
                    else if (item[27].ToString().Equals("positive"))
                    {
                        PositiveCount++;
                    }
                    else if (item[27].ToString().Equals("negative"))
                    {
                        NegativeCount++;
                    }
                }
                var ddd = items.Key.Date.ToString().Substring(0, 9);
                var record = new LineChart{ 
                    Name = ddd+"-"+items.Key.Hour.ToString(),//+":"+items.Key.Minute.ToString(),
                    PositiveRecords = PositiveCount,
                    NegativeRecords = NegativeCount,
                    NeutralRecords = NeutralCount
                };
                records.Add(record);
            }

            records = records.ToList();
            return DataSourceLoader.Load(records, loadOptions);
        }

        [HttpGet]
        public object GetLineChartDataAllTweets(DataSourceLoadOptions loadOptions)
        {
            var query = _context.TwitterRecord
                .Select(i => new TwitterRecord
                {
                    Id = i.Id,
                    Sentiment = i.Sentiment,
                    TweetCreationTime = i.TweetCreationTime,
                    TopicID = i.TopicID
                })
                .OrderBy(x => x.TweetCreationTime)
                .ToList();

            // Create a table from the query.
            DataTable dt = Global.ToDataTable<TwitterRecord>(query);


            var recordsGroupsBy = dt.AsEnumerable()
                .GroupBy(row => new
                {
                    Date = row.Field<DateTime>("TweetCreationTime").Date//,
                    //Hour = row.Field<DateTime>("TweetCreationTime").Hour//,
                    //Minute = row.Field<DateTime>("TweetCreationTime").Minute
                });

            var records = new List<LineChart>();
            foreach (var items in recordsGroupsBy)
            {
                int PositiveCount = 0, NegativeCount = 0, NeutralCount = 0;
                foreach (var item in items)
                {
                    if (item[27].ToString().Equals("neutral"))
                    {
                        NeutralCount++;
                    }
                    else if (item[27].ToString().Equals("positive"))
                    {
                        PositiveCount++;
                    }
                    else if (item[27].ToString().Equals("negative"))
                    {
                        NegativeCount++;
                    }
                }
                var ddd = items.Key.Date.ToString().Substring(0, 9);
                var record = new LineChart
                {
                    Name = ddd,// + "-" + items.Key.Hour.ToString() + ":" + items.Key.Minute.ToString(),
                    PositiveRecords = PositiveCount,
                    NegativeRecords = NegativeCount,
                    NeutralRecords = NeutralCount
                };
                records.Add(record);
            }

            records = records.ToList();
            return DataSourceLoader.Load(records, loadOptions);
        }
    }
}
