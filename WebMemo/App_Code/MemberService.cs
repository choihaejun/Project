using Biz.WebMemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class MemberService : System.Web.Services.WebService
{
  public MemberService()
  {
    //디자인된 구성 요소를 사용하는 경우 다음 줄의 주석 처리를 제거합니다. 
    //InitializeComponent(); 
  }

  #region -회원정보수정 모달
  /// <summary>
  /// 회원정보수정 모달
  /// </summary>
  /// <param name="MemberNo">회원번호</param>
  /// <param name="MemberName">회원이름</param>
  /// <returns></returns>
  [WebMethod]
  public string MemberInfoModal(string MemberNo, string MemberName)
  {
    BizMember bizMember = new BizMember();
    string phone = bizMember.GetMemberInfoById(MemberNo);
    //bizMember.UpdateMemberInfo(MemberNo, Password, Phone);

    Context.Response.Output.Write("<div class='modal-dialog'>");
    Context.Response.Output.Write("<div class='modal-content' style='background-color:black; color:white;'>");
    Context.Response.Output.Write("<div class='modal-header'>");
    Context.Response.Output.Write("<button type='button' class='close' data-dismiss='modal' aria-hidden='true' id='btnRegClose'>×</button>");
    Context.Response.Output.Write("<h4 class='modal-title'>" + MemberName + " 님의 회원정보</h4>");
    Context.Response.Output.Write("</div>");
    Context.Response.Output.Write("<form role='form'>");
    Context.Response.Output.Write("<div class='modal-body'>");
    Context.Response.Output.Write("<div class='form-group'>");
    Context.Response.Output.Write("<label for='inputPassword'>Password</label>");
    Context.Response.Output.Write("<input type='password' class='form-control' id='txtRegPassword' maxlength='20' placeholder='Password'/><br />");
    Context.Response.Output.Write("<input type='password' class='form-control' id='txtRegPwConfirm' maxlength='20' placeholder='PasswordConfirm'/>");
    Context.Response.Output.Write("</div>");
    Context.Response.Output.Write("<div class='form-group'>");
    Context.Response.Output.Write("<label for='phone'>Phone</label>");
    Context.Response.Output.Write("<input type='tel' class='form-control' id='txtPhone' placeholder='Phone' maxlength='13' value='" + phone + "'/><br />");
    Context.Response.Output.Write("</div>");
    Context.Response.Output.Write("</div>");
    Context.Response.Output.Write("<div class='modal-footer'>");
    Context.Response.Output.Write("<input type='button' class='btn btn-default' id='btnEditInfo' value='수정' />");
    Context.Response.Output.Write("</div>");
    Context.Response.Output.Write("</form>");
    Context.Response.Output.Write("</div>");
    Context.Response.Output.Write("</div>");
    Context.Response.End();
    return String.Empty;
  }

  #endregion

  #region -회원정보수정
  /// <summary>
  /// 회원정보수정
  /// </summary>
  /// <param name="MemberNo">회원번호</param>
  /// <param name="Password">비밀번호</param>
  /// <param name="Phone">전화번호</param>
  /// <returns></returns>
  [WebMethod]
  public string EditMemberInfo(string MemberNo, string Password, string Phone)
  {
    BizMember bizMember = new BizMember();
    
    int result = bizMember.UpdateMemberInfo(MemberNo, Password, Phone);
    
    if (result > 0)
    {
      Context.Response.Output.Write("수정되었습니다.");
    }
    else 
    {
      Context.Response.Output.Write("수정실패");
    }

    Context.Response.End();
    return String.Empty;
  }
  #endregion

  #region -캣차
  [WebMethod(EnableSession=true)]
  public string CheckCaptcha(string CheckStr)
  {
    if (CheckStr == Session["CaptchaImageText"].ToString())
    {
      Context.Response.Output.Write("S");
    }
    else
    {
      Context.Response.Output.Write("F");
    }
    Context.Response.End();

    return String.Empty;
  }
  #endregion
}
