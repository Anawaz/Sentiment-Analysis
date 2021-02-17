from django.shortcuts import render
from django.http import HttpResponse
from django.template import loader
from django.views.decorators.csrf import csrf_exempt
from bs4 import BeautifulSoup
import requests
import time
import sys
import html2text
import tweepy
import pandas as pd
import json
#from cassandra.cluster import Cluster
import pyodbc 
import re
import random
import facebook
import requests
import time
from config import settings_cfg_cls
from config import settings_twitter_cfg_cls
import urllib

from contextlib import closing

randam_cities={'gujranwala':
'31.918551,74.066668',
'lahore':
'31.460484,74.231463',
'fasliabad':
'31.476654,73.114731',
'rawalpindi':
'33.551655,73.013107',
'islamabad':
'33.707164,73.048813',
'multan':
'30.395342,71.573898',
'karachi':
'24.974810,66.943161',
'Peshawar':
'34.196997,71.475021'
}

conn = pyodbc.connect(
    settings_cfg_cls.osint_scraper_db_conn_str
                      )
                    
def about(request):
 return HttpResponse("About Us!")

def home(request):
 return HttpResponse("Webserive Running Successfully!")

@csrf_exempt
def hashtag_tweetsFunc(request):
 #return HttpResponse("<h1>hashtag_tweetsFunc<h1>")
    
    if 'q' in request.POST:
     #message = 'You searched for: %r' % request.GET['q']
     keywordQueryStr=request.POST['q']
     sinceQueryStr=request.POST['since']
     untilQueryStr=request.POST['until']
     topicIdQueryStr=request.POST['topic']

     print("Topic ID: "+topicIdQueryStr+" Keywords: "+keywordQueryStr+" Since Date:"+sinceQueryStr+" Until Date:"+untilQueryStr)
  
     auth = tweepy.OAuthHandler(settings_twitter_cfg_cls.consumer_key, settings_twitter_cfg_cls.consumer_secret)
     auth.set_access_token(settings_twitter_cfg_cls.access_token, settings_twitter_cfg_cls.access_token_secret)
     api = tweepy.API(auth,wait_on_rate_limit=True)
     max_tweets = 1000
     counter=0;
     #searched_tweets = [status._json for status in tweepy.Cursor(api.search,q=keywordQueryStr,since=sinceQueryStr,until=untilQueryStr).items(max_tweets)]
     for tweet in tweepy.Cursor(api.search,q=keywordQueryStr,since=sinceQueryStr,until=untilQueryStr).items(max_tweets):
      try:
            #for tweet in tweets:
            tweep=tweet.author.screen_name.encode('utf8')
            #print(tweet)
            # tweet_json_str = json.dumps(tweet._json)
            #print(tweet._json)

            displayname=str(tweet.user.name)
            displayname=displayname.replace("'", '\\"')
            #username=str(tweet.author.screen_name)#profile unique name
            username=str(tweet.user.screen_name)#profile unique name
            username=username.replace("'", '\\"')
            tweetcreationtime=str(tweet.created_at)
            tweetlocation="";
            if tweet.coordinates is None:
                #print("tweet.coordinates is null use randam_lat_long")
                city,latlong = random.choice(list(randam_cities.items()))
                print(city)
                #print(latlong)
                lat, long = latlong.split(',',1)
                latRdm=lat[0:8]+str(random.randint(1,9))
                longRdm=long[0:8]+str(random.randint(1,9))
                print(latRdm)
                print(longRdm)
                tweetlocation=latRdm+","+longRdm
            else: 
                tweetlocationArr=tweet.coordinates
                print(tweetlocationArr)
                tweetlocationComSep = ",".join(tweetlocationArr)
                tweetlocation = str(tweetlocationComSep)
            if 'coordinates' in tweetlocation:
                tweetlocation="NULL"
                print("no coordinates found(coordinates has word type,coordinates)")   
                
            tweettext=str(tweet.text)
            #tweethashtagsArr=tweet.entities.get('hashtags')
            #print(tweethashtagsArr)
            tweethashtagsArr=re.findall(r"#(\w+)", tweettext)
            #print(tweethashtagsArr)
            tweethashtagsArrComSep = ",".join(tweethashtagsArr)
            tweethashtags=tweethashtagsArrComSep#"123,abc"#hashtags
            #print(tweethashtags)
            #tweettext=MySQLdb.escape_string(tweettext)
            tweettext=tweettext.replace("'", '\\"')
            inreplytoscreenname=str(tweet.in_reply_to_screen_name)
            inreplytoscreenname=inreplytoscreenname.replace("'", '\\"')
            tweethtml="selfhtml"#//self create it
            tweetid=str(tweet.id)
            tweeturl="https://twitter.com/" + username + "/status/"+tweetid
            print(tweeturl)
            tweetlanguage=str(tweet.lang)    
            description=str(tweet.user.description)
            description=description.replace("'", '\\"')
            isgeoenabled=tweet.user.geo_enabled
            isgeoenabled = int(isgeoenabled == 'true')
            isverified=tweet.user.verified
            isverified = int(isverified == 'true')
            language=str(tweet.user.lang)
            #if 'profile_banner_url' not in tweet.user:
            profilebannerurl=""
            #else:
            #profilebannerurl=str(tweet.user.profile_banner_url)
            profileimageurl=str(tweet.user.profile_image_url)
            profileurl="https://twitter.com/" + username
            timezone=str(tweet.user.time_zone)
            #print(timezone)
            totalfollowers=tweet.user.followers_count
            totalfollowing=tweet.user.friends_count
            statuscount=tweet.user.statuses_count
            listedcount=tweet.user.listed_count
            profilelocation=tweet.user.location
            #profilelocation = profilelocation.encode('utf-8')
            sentimentVal = 'pending'
            query_insert = """insert into TwitterRecord
            (Source,SourceScript,
            TweetCreationTime, TweetLocation, TweetHashTags, TweetText,
            InReplyToScreenName, TweetHtml, TweetId, TweetURL,
            TweetLanguage, DisplayName, UserName, Description,
            IsGeoEnabled, IsVerified, Language, ProfileBannerURL,
            ProfileImageURL, ProfileURL, TimeZone, TotalFollowers,
            ProfileLocation,TotalFollowing, StatusCount, ListedCount,TopicID,Sentiment) values
            ('twitter','twitter_tweepy',
            '{0}','{1}',N'{2}',N'{3}',
            N'{4}',N'{5}','{6}',N'{7}',
            '{8}',N'{9}',N'{10}',N'{11}',
            {12},{13},'{14}','{15}',
            '{16}',N'{17}',N'{18}',{19},
            N'{20}',{21},{22},{23},{24},N'{25}')"""
            query_insert=query_insert.format(
            tweetcreationtime,tweetlocation,tweethashtags,tweettext,
            inreplytoscreenname,tweethtml,tweetid,tweeturl,
            tweetlanguage,displayname,username,description,
            isgeoenabled,
            isverified,
            language,
            profilebannerurl,
            profileimageurl,
            profileurl,
            timezone,
            totalfollowers,
            profilelocation,
            totalfollowing,
            statuscount,
            listedcount,topicIdQueryStr,sentimentVal)
            
            query_insert=query_insert.replace("N'NULL'","NULL")
            
            csr = conn.cursor()
            csr.execute(query_insert)
            conn.commit()
            counter+=1
      except tweepy.TweepError as tweeptyErr:
          print('Skip this Tweet Tweepy Error Reason:',tweeptyErr)
      except Exception  as detail:
          print('Skip this Tweet Error Reason:',detail)

     if counter > 0:
         print("Tweets Inserted:"+str(counter)+" Topic ID: "+topicIdQueryStr+" Keywords: "+keywordQueryStr+" Since Date:"+sinceQueryStr+" Until Date:"+untilQueryStr)
         message=str(counter)
     else:
         print("Tweets Inserted:"+str(counter)+" Topic ID: "+topicIdQueryStr+" Keywords: "+keywordQueryStr+" Since Date:"+sinceQueryStr+" Until Date:"+untilQueryStr)
         message="0"
    else:
        message = 'You submitted an empty request paramaters.'
    return HttpResponse(message)