import pyodbc
import sys
from textblob import TextBlob
from langdetect import detect
sys.path.append("..") # Adds higher directory to python modules path.
from config import settings_cfg_cls
from urdu_sentiment_NLP_analysis_lib import get_urdu_sentiment
import traceback
from config import settings_cfg_cls

def get_en_textblob_sentiment(inputtxt):
    # statementInput = """
    # The most idiot person in the world award goes to one nd only Nawaz Sharif
    # """
    statementInput = inputtxt
    print(statementInput)
    statementInputOutput = TextBlob(statementInput)
    #print("Sentiment Score: ", statementInputOutput.sentiment.polarity) # Result = 0.909999
    if float(statementInputOutput.sentiment.polarity) < float("-0.2") :
        outputtxt=("negative")
    elif float(statementInputOutput.sentiment.polarity)> float("-0.2") and float(statementInputOutput.sentiment.polarity)<float("0.2") :
        outputtxt=("neutral")
    else:
        #print("postive/nautral/non-sentimental")
        outputtxt=("positive")
    return outputtxt

def get_ur_sentiment_NLP_analysis(inputtxt):
#     urdu_input_sentence="""
# آپ جیسا اچھا بندہ نہیں دیکھا میں نے   
# """
    urdu_input_sentence=inputtxt
    urdu_sentiment_output=get_urdu_sentiment(urdu_input_sentence)
    #print(urdu_sentiment_output)
    return urdu_sentiment_output
try:
    pyodbc.pooling = False

    conn = pyodbc.connect(settings_cfg_cls.osint_scraper_db_conn_str)

    csr = conn.cursor()
    # csrUpdate = conn.cursor()
    rows=csr.execute("""SELECT [id],[tweetlanguage],[tweettext] FROM [TwitterRecord]
     where sentiment = 'pending' """).fetchall()
    how_many = conn.getinfo(pyodbc.SQL_MAX_CONCURRENT_ACTIVITIES)
    print("Records: ")
    print(how_many)
    # for row in cursor:
    #    print('row = %r' % (row,))
    for row in rows:
        try:
            tweet_id=row[0]
            tweet_language=row[1]
            tweeet_sentiment_result="postive/nautral/non-sentimental"
            tweet_text=row[2]
            
            if tweet_language=="en": #english
                tweeet_sentiment_result=get_en_textblob_sentiment(tweet_text)
            elif tweet_language=="ur": #urdu
                tweeet_sentiment_result = get_ur_sentiment_NLP_analysis(tweet_text)
            else:
                tweeet_sentiment_result = "not-supported"
            print(tweeet_sentiment_result)

            query_update = """update TwitterRecord set Sentiment='{0}' where Id='{1}';"""
            query_update=query_update.format(
            tweeet_sentiment_result,tweet_id)
            # print(query_update)
            csr.execute(query_update)
            csr.commit()
        except Exception  as detail:
            print('skip this record for sentiment updates Reason:',detail)
            traceback.print_exc()

    csr.close()
    del csr

except Exception  as detail:
    print('Main Func Error Reason:',detail)
    traceback.print_exc()
    csr.close()
    del csr