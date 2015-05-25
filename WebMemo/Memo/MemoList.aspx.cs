using Biz.WebMemo;
using Dac.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Memo_MemoList : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!IsPostBack)
    {
      if (Request.Cookies["LoginInfo"] == null)
      {
        Response.Redirect(@"/Public/Login.aspx");
      }
      else
      {
        AdminCheck();
      }
    }
  }
  protected void AdminCheck()
  {
    if (Request.Cookies["LoginInfo"]["LVL"] == "1")
    {
      btnAdmin.Visible = true;
    }
    else
    {
      btnAdmin.Visible = false;
    }
  }
  protected void btnLogout_ServerClick(object sender, EventArgs e)
  {
    Response.Cookies["LoginInfo"].Expires = DateTime.Now.AddDays(-1);
    ScriptManager.RegisterStartupScript(this,
                                            Type.GetType("System.String"),
                                            "",
                                            "javascript:Logout();",
                                            true
                                            );
  }
}