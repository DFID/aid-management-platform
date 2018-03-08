using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AMP.Models;
using AMP.ViewModels;

namespace AMP.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString HyperLink(this HtmlHelper helper, string url, string target, string text)
        {
            return MvcHtmlString.Create(string.Format(@"<a href=""{0}{1}"">{2}</a>", url, target, text));
        }

        public static MvcHtmlString HyperLink(this HtmlHelper helper, string url, string text)
        {
            return MvcHtmlString.Create(string.Format(@"<a href=""{0}"">{1}</a>", url, text));
        }


        public static string Input(this HtmlHelper helper, string dateformat, string controlclass)
        {
            return string.Format(@"<input type = ""text"" class=""{0}"", date-date-format=""{1}"" />", controlclass, dateformat);
        }

        public static MvcHtmlString ReviewPerformanceDropDown(this HtmlHelper helper, string labelname, int index)
        {
            return MvcHtmlString.Create(string.Format(@"<div class=""grid-row"">
  <div class=""column-third"">
	<label for=""ReviewVm_ReviewOutputVm_OutputScore_{0}"" class=""form-label-bold"">{1}</label>
	   <select class=""form-control"" data-val=""true"" data-val-required=""Please select performance rating"" id=""ReviewVm_ReviewOutputVm_OutputScore_{0}"" name=""ReviewVm.ReviewOutputVm.OutputScore"">
	  <option value="""">
		Select performance rating
	  </option>
	  <option value=""A++"">
		A++ Outputs Substantially exceeded expectation
	  </option>
	  <option value=""A+"">
		A+ Outputs Moderately exceeded expectation
	  </option>
	  <option value=""A"">
		A Outputs Met expectation
	  </option>
	  <option value=""B"">
		B Outputs Moderately did not meet expectation
	  </option>
	  <option value=""C"">
		C Outputs Substantially did not meet expectation
	  </option>
	</select>
	<span class=""field-validation-valid""
	   data-valmsg-for=""ReviewVm.ReviewOutputVm.OutputScore"" data-valmsg-replace=""true"">
	</span>
  </div>
</div>
", index, labelname));
        }

        public static MvcHtmlString ReviewRiskDropDown(this HtmlHelper helper, string labelname, int index, string hintText)
        {
            return MvcHtmlString.Create(string.Format(@"<div class=""grid-row"">

 <div class=""column-third"">
	<label for=""ReviewVm_ReviewOutputVm_Risk_{0}"" class=""form-label-bold"">
	  {1}
	</label>

<span class=""form-hint"">  {2} </span>

        <select class=""form-control"" data-val=""true"" data-val-required=""Please select risk type""
	   id=""ReviewVm_ReviewOutputVm_Risk_{0}"" name=""ReviewVm.ReviewOutputVm.Risk"">
	  <option value="""">
		Select risk type
	  </option>
	  <option value=""R1"">
		Minor
	  </option>
	  <option value=""R2"">
		Moderate
	  </option>
	  <option value=""R3"">
		Major
	  </option>
	  <option value=""R4"">
		Severe
	  </option>
	</select>
	<span class=""field-validation-valid"" data-valmsg-for=""ReviewVm.ReviewOutputVm.Risk""
	   data-valmsg-replace=""true"">
	</span>
  </div>
</div>
", index, labelname, hintText));

        }


        public static MvcHtmlString ReviewOverallRiskDropDown(this HtmlHelper helper, string labelname, int index, string hintText)
        {
            return MvcHtmlString.Create(string.Format(@"<div class=""grid-row"">

 <div class=""column-third"">
	<label for=""ReviewVm_ReviewOutputVm_OverAllRisk_{0}"" class=""form-label-bold"">
	  {1}
	</label>

<span class=""form-hint"">  {2} </span>

        <select class=""form-control"" data-val=""true"" data-val-required=""Please select risk type""
	   id=""ReviewVm_ReviewOutputVm_OverAllRisk_{0}"" name=""ReviewVm.ReviewOutputVm.Risk"">
	  <option value="""">
		Select risk type
	  </option>
	  <option value=""R1"">
		Minor
	  </option>
	  <option value=""R2"">
		Moderate
	  </option>
	  <option value=""R3"">
		Major
	  </option>
	  <option value=""R4"">
		Severe
	  </option>
	</select>
	<span class=""field-validation-valid"" data-valmsg-for=""ReviewVm.ReviewOutputVm.Risk""
	   data-valmsg-replace=""true"">
	</span>
  </div>
</div>
", index, labelname, hintText));

        }


        public static MvcHtmlString DateBlock(this HtmlHelper helper, string datename, string labelname, DateTime? Date, String HintText) //int day, int month, int year
        {
            String ControlName = datename;
            String ControlID = datename.Replace(".", "_");

            // Decompose date
            string Day = "", Month = "", Year = "";

            if (Date != null)
            {
                DateTime newDateTime = Date.Value;

                if (!Date.Equals(DateTime.MinValue))
                {
                    Day = Date.Value.Day.ToString();
                    Month = Date.Value.Month.ToString();
                    Year = Date.Value.Year.ToString();
                }
            } //int Day = Date.Day;
            //int Month = Date.Month;
            //int Year = Date.Year;

            return MvcHtmlString.Create(string.Format(@"
            <div id=""{0}_Section"">
                    <div class=""form-group"">
                                    <div class=""form-label-bold"">{1}</div>
                                    <div class=""form-date"">
                                        <p class=""form-hint"">{6}</p>
                                        <div class=""form-group form-group-day"">
                                            <label for=""{5}_Day"">Day</label>
                                            <input class=""form-control"" id=""{5}_Day"" name=""{0}_Day"" type=""number"" min=""1"" max=""31"" value=""{2}"">
                                        </div>
                                        <div class=""form-group form-group-month"">
                                            <label for=""{5}_Month"">Month</label>
                                            <input class=""form-control"" id=""{5}_Month"" name=""{0}_Month"" type=""number"" min=""0"" max=""12"" value=""{3}"">
                                        </div>
                                        <div class=""form-group form-group-year"">
                                            <label for=""{5}_Year"">Year</label>
                                           <input class=""form-control"" id=""{5}_Year"" name=""{0}_Year"" type=""number"" min=""1900"" max=""2100"" value=""{4}"">
                                        </div>
                                    </div>
                                
                            </div>
                            </div>
                    ", ControlName, labelname, Day, Month, Year, ControlID, HintText));

            //Old Razor code for showing date block with day, month, year individually  @Html.DateBlock("ComponentDate.OperationalStartDate", "Operational Start Date", @Model.ComponentDate.OperationalStartDate_Day, @Model.ComponentDate.OperationalStartDate_Month, @Model.ComponentDate.OperationalStartDate_Year, @Model.ComponentDate.StartDate.Value)
        }
        public static MvcHtmlString DateBlockWithoutFieldset(this HtmlHelper helper, string datename, string labelname, DateTime? Date, String HintText, string forid) //int day, int month, int year
        {
            String ControlName = datename;
            String ControlID = datename.Replace(".", "_");

            // Decompose date
            string Day = "", Month = "", Year = "";

            if (Date != null)
            {
                DateTime newDateTime = Date.Value;

                if (!Date.Equals(DateTime.MinValue))
                {
                    Day = Date.Value.Day.ToString();
                    Month = Date.Value.Month.ToString();
                    Year = Date.Value.Year.ToString();
                }
            }
            //int Day = Date.Day;
            //int Month = Date.Month;
            //int Year = Date.Year;

            return MvcHtmlString.Create(string.Format(@"
                    <div class=""form-group"">                               
                                    <label class=""form-label-bold"" for=""{7}_Day"" >{1}</label>
                                    <div class=""form-date"">
                                        <p class=""form-hint"">{6}</p>
                                        <div class=""form-group form-group-day"">
                                            <label for=""{5}_Day"">Day</label>
                                            <input class=""form-control"" id=""{5}_Day"" name=""{0}_Day"" type=""number"" min=""1"" max=""31"" value=""{2}"">
                                        </div>
                                        <div class=""form-group form-group-month"">
                                            <label for=""{5}_Month"">Month</label>
                                            <input class=""form-control"" id=""{5}_Month"" name=""{0}_Month"" type=""number"" min=""0"" max=""12"" value=""{3}"">
                                        </div>
                                        <div class=""form-group form-group-year"">
                                            <label for=""{5}_Year"">Year</label>
                                           <input class=""form-control"" id=""{5}_Year"" name=""{0}_Year"" type=""number"" min=""1900"" max=""2100"" value=""{4}"">
                                        </div>
                                    </div>                               
                            </div>
                    ", ControlName, labelname, Day, Month, Year, ControlID, HintText, forid));

            //Old Razor code for showing date block with day, month, year individually  @Html.DateBlock("ComponentDate.OperationalStartDate", "Operational Start Date", @Model.ComponentDate.OperationalStartDate_Day, @Model.ComponentDate.OperationalStartDate_Month, @Model.ComponentDate.OperationalStartDate_Year, @Model.ComponentDate.StartDate.Value)
        }
        public static MvcHtmlString PartnersNestedList(this HtmlHelper helper, List<DeliveryChainListVM> deliveryChain)
        {
            StringBuilder sb = new StringBuilder();
            if (deliveryChain != null)
            {
                List<DeliveryChainListVM> parentList = deliveryChain.Where(x => x.ParentID == x.ChildID && x.ParentNodeID == null).ToList();

                sb.Append("<ul>");
                foreach (var parent in parentList)
                {
                    sb.Append(BuildListTag(parent.ParentID));
                    sb.Append(parent.ChildName);
                    sb.Append(BuildButton(parent.ID));
                    deliveryChain.Remove(parent);
                    BuildList(deliveryChain, parent.ParentID, parent.ChainID, sb);
                }
                sb.Append("</li></ul>");

            }

            return MvcHtmlString.Create(sb.ToString());
        }

        private static void BuildList(List<DeliveryChainListVM> partners, string  parentId, string parentChainId, StringBuilder sb)
        {
            if (partners.Any(x => x.ParentNodeID == parentId))
            {

                // ****** REMOVING CHECK ON parentChainId TO SEE IF HELPS WITH CHAINING IDENTICAL PARENT AND CHILD
                List<DeliveryChainListVM> children = partners.Where(x => x.ParentNodeID == parentId).ToList();
                //**************************************************************
                //List<DeliveryChainListVM> children = partners.Where(x => x.ParentID == parentId).ToList();
                if (children.Any())
                {
                    sb.Append("<ul>");
                    foreach (DeliveryChainListVM child in children)
                    {
                        sb.Append(BuildListTag(child.ChildID));
                        sb.Append(child.ChildName);
                        sb.Append(BuildButton(child.ID));
                        // this BuildList call repeats continually on parent-childs with same supplier/partner IDs - need to get it to only execute once.

                        BuildList(partners,child.ChildID,child.ChainID,sb);
                        sb.Append("</li>");
                    }
                    sb.Append("</ul>");
                }
            }
        }


        private static string BuildListTag(Int32 id)
        {
            return BuildListTag(id.ToString());
        }

        private static string BuildListTag(string id)
        {
            return string.Format(@"<li id=""partner-{0}"">", id);
        }

        private static string BuildButton(Int32 Id)
        {
            return BuildButton(Id.ToString());
        }


        // ******* THIS IS WHAT IS CAUSING THE STACK OVERFLOW ISSUE ***********
        private static string BuildButton(string Id)
        {
            return String.Format(@"&nbsp;<button data-url=""/Component/SearchPartner/{0}"" class=""button Add-Partner-Editor"" style=""padding:0px;"" tooltip=""Remove"">&nbsp;Add&nbsp;</button>&nbsp;<button data-url=""/Component/DeleteChain/{0}""  class=""Delete-Partner-Editor red"" style=""padding:0px;"" tooltip=""Delete"">&nbsp;Delete&nbsp;</button>", Id);
        }
    }
}