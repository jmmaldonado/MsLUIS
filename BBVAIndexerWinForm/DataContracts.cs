using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBVAIndexerWinForm {

    #region LUIS Data Contract

    // Type created for JSON at <<root>>
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class LUISResponse {

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string query;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public Intents[] intents;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public Entities[] entities;
    }

    // Type created for JSON at <<root>> --> intents
    [System.Runtime.Serialization.DataContractAttribute(Name = "intents")]
    public partial class Intents {

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string intent;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public double score;
    }

    // Type created for JSON at <<root>> --> entities
    [System.Runtime.Serialization.DataContractAttribute(Name = "entities")]
    public partial class Entities {

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string entity;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string type;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int startIndex;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int endIndex;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public double score;
    }

    #endregion



    #region PowerBI Data Contracts

    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class IntentsPowerBi {

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string query;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string queryKeywords;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string financialKeywords;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] financialKeywordsArray;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string intent;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public double score;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public double sentiment;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ProcessedDateTime;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int processedYear;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int processedMonth;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int processedDay;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int processedHour;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int processedMinute;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int processedSecond;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string eventGUID;

    }


    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class EntitiesPowerBi {

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string query;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string queryKeywords;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string financialKeywords;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public Entities[] entities;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string entity;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string entityType;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public double score;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public double sentiment;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ProcessedDateTime;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int processedYear;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int processedMonth;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int processedDay;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int processedHour;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int processedMinute;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int processedSecond;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string eventGUID;

    }

    #endregion





    #region Sentiment Analysis Data Contracts

    public class KeyPhraseResult {
        public List<string> KeyPhrases { get; set; }
    }

    /// <summary>
    /// Class to hold result of Sentiment call
    /// </summary>
    public class SentimentResult {
        public double Score { get; set; }
    }


    #endregion


}
