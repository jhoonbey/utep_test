using System;

namespace app.Model.Criterias
{
    public class BaseCriteriaModel
    {
        public string Keyword { get; set; }
        public DateTime? MinCreateDate { get; set; }
        public DateTime? MaxCreateDate { get; set; }
    }
}
