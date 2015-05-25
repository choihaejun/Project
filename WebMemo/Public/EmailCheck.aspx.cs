using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Biz.WebMemo;

public partial class Public_EmailCheck : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        EmailCheck();
    }

    #region -이메일체크
    /// <summary>
    /// 이메일체크
    /// </summary>
    protected void EmailCheck()
    {
        BizMember bizmember = new BizMember();
        string Email = Request.Form["email"];

        int result = bizmember.CheckId(Email);

        if (result > 0)
        {
            Response.Write("NO");
        }
        else
        {
            Response.Write("YES");
        }
    }

    #endregion
}