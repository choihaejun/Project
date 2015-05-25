using Biz.WebMemo;
using Dac.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Public_Login : System.Web.UI.Page
{
  BizMember bizMember = new BizMember();
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!IsPostBack)
    {
      if (Request.Cookies["LoginID"] != null)
      {
        txtEmail.Value = Request.Cookies["LoginID"].Value;
        chkRemember.Checked = true;
      }

      if(Request.Cookies["LoginInfo"] != null)
      {
        Response.Redirect("/Memo/MemoList.aspx");
      }
    }
  }

  #region -로그인버튼 이벤트
  /// <summary>
  /// 로그인버튼 이벤트
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  protected void LoginBtn_Click(object sender, EventArgs e)
  {
    if (Id_Check())
    {
      if (Password_Check())
      {
        if (chkRemember.Checked)
        {
          Response.Cookies["LoginID"].Value = txtEmail.Value;
          Response.Cookies["LoginID"].Expires = DateTime.Now.AddYears(1);
        }
        else
        {
          Response.Cookies["LoginID"].Expires = DateTime.Now.AddYears(-1);
        }
        
        ScriptManager.RegisterStartupScript(this,
                                            Type.GetType("System.String"),
                                            "",
                                            "javascript:Login();",
                                            true
                                            );
      }
      else
      {
        CallAlert("비밀번호가 잘못되었습니다.");
      }
    }
    else
    {
      CallAlert("잘못된 아이디입니다.");
    }
  }

  #endregion

  #region -아이디 체크
  /// <summary>
  /// 아이디 체크
  /// </summary>
  /// <returns></returns>
  protected bool Id_Check()
  {
    int result = bizMember.CheckId(txtEmail.Value);

    if (result > 0)
    {
      return true;
    }
    else
    {
      return false;
    }
  }
  #endregion

  #region - 비밀번호 체크
  /// <summary>
  /// 비밀번호 체크
  /// </summary>
  /// <returns></returns>
  protected bool Password_Check()
  {
    Member loginMember = bizMember.CheckPassword(txtEmail.Value, txtPassword.Value);

    if (loginMember.NO != 0)
    {
      MemberInfoCookieCreate(loginMember);
      return true;
    }
    else
    {
      return false;
    }
  }

  #endregion

  #region -로그인멤버 쿠키생성

  protected void MemberInfoCookieCreate(Member LoginMember)
  {
    HttpCookie cookie = new HttpCookie("LoginInfo");
    cookie["NO"] = LoginMember.NO.ToString();
    cookie["NAME"] = Server.UrlEncode(LoginMember.NAME);
    cookie["LVL"] = LoginMember.LVL.ToString();
    cookie.Expires = DateTime.Now.AddHours(1);
    Response.Cookies.Add(cookie);
  }

  #endregion

  #region - Alert창 호출
  /// <summary>
  /// Alert창 호출
  /// </summary>
  /// <param name="Message"></param>
  protected void CallAlert(string Message)
  {
    ScriptManager.RegisterStartupScript(this,
                                        Type.GetType("System.String"),
                                        "",
                                        "javascript:window.alert('" + Message + "');",
                                        true
                                        );
  }
  #endregion

  #region -회원가입
  /// <summary>
  /// 회원가입
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  protected void btnReg_Click(object sender, EventArgs e)
  {
    string[] regInfo = hfValue.Value.Split(',');

    string alertText = bizMember.AddMember(regInfo[0], regInfo[1], regInfo[2], regInfo[3]);

    CallAlert(alertText);
  }

  #endregion
}