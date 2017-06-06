using System.Security.Policy;

namespace Sitecore.Support.Analytics.Aggregation.Pipeline
{
    using Sitecore.Analytics.Aggregation.Pipeline;
    using Sitecore.Analytics.Model;
    using Sitecore.Diagnostics;
    using System;

    internal class TruncateLongDataProcessor : AggregationProcessor
    {
        protected override void OnProcess(AggregationPipelineArgs args)
        {
            VisitData visit = args.Context.Visit;
            if ((visit.Keywords != null) && (visit.Keywords.Length > 400))
            {
                Log.Warn("Sitecore.Support.97934 The length of the Keywords field exceeded the limit of 400 and has been truncated", this);
                visit.Keywords = visit.Keywords.Substring(0, 400);
            }
            if ((visit.Pages != null) && (0 < visit.Pages.Count))
            {
                foreach (PageData data2 in visit.Pages)
                {
                    var url = data2.Url.ToString();

                    if (url.Length > 450)
                    {
                        Log.Warn(
                            "Sitecore.Support.97934 The length of the Pages.Url field exceeded the limit of 450 and has been truncated",
                            this);

                        if (data2.Url.Path.Length > 450)
                        {
                            data2.Url.QueryString = "";
                            data2.Url.Path = data2.Url.Path.Substring(0, 450);
                        }

                        url = data2.Url.ToString();

                        if (!string.IsNullOrEmpty(data2.Url.QueryString) && url.Length > 450)
                        {
                            data2.Url.QueryString = data2.Url.QueryString.Substring(0, 450 - data2.Url.Path.Length);
                        }

                    }
                }
            }
        }
    }
}
