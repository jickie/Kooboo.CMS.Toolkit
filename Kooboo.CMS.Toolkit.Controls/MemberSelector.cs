﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Form;
using Kooboo.CMS.Form.Html;
using Kooboo.CMS.Common.Runtime;

namespace Kooboo.CMS.Toolkit.Controls
{
    public class MemberSelector : Form.Html.Controls.DropDownList
    {
        public override string Name
        {
            get
            {
                return "MemberSelector";
            }
        }

        public override bool HasDataSource
        {
            get
            {
                return true;
            }
        }

        protected override string RenderInput(IColumn column)
        {
            StringBuilder sb = new StringBuilder(string.Format(@"@{{ var dropDownDefault_{0} =  @""{1}"";}}
                <select name=""{0}"" class=""long"">", column.Name, column.DefaultValue.EscapeQuote()));
            string emptyOption = "";
            if (column.AllowNull)
            {
                emptyOption = @"<option value=""""></option>";
            }
            //var manager = EngineContext.Current.Resolve<Kooboo.CMS.Membership.Services.MembershipUserManager>();
            //manager.All()
            
            sb.AppendFormat(@"
            @{{
                var membership = Site.Current.GetMembership();
                var manager = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<Kooboo.CMS.Membership.Services.MembershipUserManager>();
                var query_{0} = manager.All(membership, """");
            }}
            {1}
            @foreach (var item in query_{0})
            {{                            
                <option value=""@item.UUID"" @((Model.{0} != null && Model.{0}.ToString().ToLower() == @item.UUID.ToLower()) || (Model.{0} == null && dropDownDefault_{0}.ToLower() == @item.UUID.ToLower()) ? ""selected"" : """")>@item.UserName</option>
            }}
            ", column.Name,  emptyOption);
            sb.Append("</select>");

            return sb.ToString();
        }
    }
}